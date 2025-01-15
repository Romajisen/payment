namespace Bosnet.Services
{
    using Bosnet.Models;
    using Bosnet.Utilities;
    using System.Data.OleDb;

    public class TransactionService
    {
        private readonly DatabaseHelper _dbHelper;

        public TransactionService(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        public string GenerateTransactionId(OleDbConnection connection, OleDbTransaction transaction)
        {
            string counterId = "001-COU";
            string datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            string query = "SELECT iLastNumber, iFirstPart FROM BOS_Counter WHERE szCounterId = ?";

            using (var command = new OleDbCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@szCounterId", counterId);
                using (var reader = command.ExecuteReader())
                {
                    long lastNumber = 0;
                    long firstPart = 0;

                    if (reader.Read())
                    {
                        lastNumber = reader["iLastNumber"] != DBNull.Value ? Convert.ToInt64(reader["iLastNumber"]) : 0;
                        firstPart = reader["iFirstPart"] != DBNull.Value ? Convert.ToInt64(reader["iFirstPart"]) : 0;
                    }
                    // Increment logic
                    if (lastNumber >= 99999)
                    {
                        if (firstPart >= 99999)
                        {
                            firstPart = 0;
                            lastNumber = 1;
                        }
                        else
                        {
                            lastNumber = 1;
                            firstPart++;
                        }
                    }
                    else
                    {
                        lastNumber++;
                    }
                    string updateQuery = "UPDATE BOS_Counter SET iLastNumber = ?, iFirstPart = ? WHERE szCounterId = ?";

                    using (var updateCommand = new OleDbCommand(updateQuery, connection, transaction))
                    {
                        updateCommand.Parameters.AddWithValue("@iLastNumber", lastNumber);
                        updateCommand.Parameters.AddWithValue("@iFirstPart", firstPart);
                        updateCommand.Parameters.AddWithValue("@szCounterId", counterId);
                        updateCommand.ExecuteNonQuery();
                    }
                    return $"{datePart}-{firstPart:D5}.{lastNumber:D5}";
                }
            }
        }

        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        public void Setor(SetorRequest request)
        {
            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        string transactionId = GenerateTransactionId(connection, transaction);

                        string insertQuery = @"
                INSERT INTO BOS_History (szTransactionId, szAccountId, szCurrencyId, dtmTransaction, decAmount, szNote)
                VALUES (?, ?, ?, GETDATE(), ?, ?)";

                        using (var command = new OleDbCommand(insertQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@szTransactionId", transactionId);
                            command.Parameters.AddWithValue("@szAccountId", request.AccountId);
                            command.Parameters.AddWithValue("@szCurrencyId", request.CurrencyId);
                            command.Parameters.AddWithValue("@decAmount", request.Amount);
                            command.Parameters.AddWithValue("@szNote", request.Note);
                            command.ExecuteNonQuery();
                        }

                        string updateBalanceQuery = @"
                MERGE INTO BOS_Balance AS Target
                USING (SELECT ? AS szAccountId, ? AS szCurrencyId, ? AS decAmount) AS Source
                ON Target.szAccountId = Source.szAccountId AND Target.szCurrencyId = Source.szCurrencyId
                WHEN MATCHED THEN
                    UPDATE SET Target.decAmount = Target.decAmount + Source.decAmount
                WHEN NOT MATCHED THEN
                    INSERT (szAccountId, szCurrencyId, decAmount) VALUES (Source.szAccountId, Source.szCurrencyId, Source.decAmount);";

                        using (var command = new OleDbCommand(updateBalanceQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@szAccountId", request.AccountId);
                            command.Parameters.AddWithValue("@szCurrencyId", request.CurrencyId);
                            command.Parameters.AddWithValue("@decAmount", request.Amount);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        public void Tarik(TarikRequest request)
        {
            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        string transactionId = GenerateTransactionId(connection, transaction);

                        string balanceQuery = "SELECT decAmount FROM BOS_Balance WHERE szAccountId = ? AND szCurrencyId = ?";
                        decimal currentBalance = 0;

                        using (var command = new OleDbCommand(balanceQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@szAccountId", request.AccountId);
                            command.Parameters.AddWithValue("@szCurrencyId", request.CurrencyId);
                            var result = command.ExecuteScalar();
                            currentBalance = result != null ? Convert.ToDecimal(result) : 0;
                        }

                        if (currentBalance < request.Amount)
                            throw new Exception("Saldo tidak mencukupi");

                        string insertQuery = @"
                    INSERT INTO BOS_History (szTransactionId, szAccountId, szCurrencyId, dtmTransaction, decAmount, szNote)
                    VALUES (?, ?, ?, GETDATE(), ?, ?)";

                        using (var command = new OleDbCommand(insertQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@szTransactionId", transactionId);
                            command.Parameters.AddWithValue("@szAccountId", request.AccountId);
                            command.Parameters.AddWithValue("@szCurrencyId", request.CurrencyId);
                            command.Parameters.AddWithValue("@decAmount", -request.Amount);
                            command.Parameters.AddWithValue("@szNote", request.Note);
                            command.ExecuteNonQuery();
                        }

                        string updateBalanceQuery = "UPDATE BOS_Balance SET decAmount = decAmount - ? WHERE szAccountId = ? AND szCurrencyId = ?";
                        using (var command = new OleDbCommand(updateBalanceQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@decAmount", request.Amount);
                            command.Parameters.AddWithValue("@szAccountId", request.AccountId);
                            command.Parameters.AddWithValue("@szCurrencyId", request.CurrencyId);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        public void TransferBagi(TransferRequest request)
        {
            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        string transactionId = GenerateTransactionId(connection, transaction);

                        // Get current balance of the source account
                        string balanceQuery = "SELECT decAmount FROM BOS_Balance WHERE szAccountId = ? AND szCurrencyId = ?";
                        decimal currentBalance = 0;

                        using (var command = new OleDbCommand(balanceQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@szAccountId", request.SourceAccountId);
                            command.Parameters.AddWithValue("@szCurrencyId", request.CurrencyId);
                            var result = command.ExecuteScalar();
                            currentBalance = result != null ? Convert.ToDecimal(result) : 0;
                        }

                        // Calculate total amount to deduct from source account
                        decimal totalAmountToDeduct = request.Amount * request.TargetAccountIds.Count;

                        if (currentBalance < totalAmountToDeduct)
                            throw new Exception("Saldo tidak mencukupi");

                        // Process each target account
                        foreach (var targetAccountId in request.TargetAccountIds)
                        {
                            // Insert history record for each target account
                            string insertQuery = @"
                                INSERT INTO BOS_History (szTransactionId, szAccountId, szCurrencyId, dtmTransaction, decAmount, szNote)
                                VALUES (?, ?, ?, GETDATE(), ?, ?)";
                            using (var command = new OleDbCommand(insertQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@szTransactionId", transactionId);
                                command.Parameters.AddWithValue("@szAccountId", targetAccountId);
                                command.Parameters.AddWithValue("@szCurrencyId", request.CurrencyId);
                                command.Parameters.AddWithValue("@decAmount", request.Amount);  // Full transfer amount for each target account
                                command.Parameters.AddWithValue("@szNote", request.Note);
                                command.ExecuteNonQuery();
                            }

                            // Update target account balance
                            string updateBalanceQuery = @"
                                MERGE INTO BOS_Balance AS Target
                                USING (SELECT ? AS szAccountId, ? AS szCurrencyId, ? AS decAmount) AS Source
                                ON Target.szAccountId = Source.szAccountId AND Target.szCurrencyId = Source.szCurrencyId
                                WHEN MATCHED THEN
                                    UPDATE SET Target.decAmount = Target.decAmount + Source.decAmount
                                WHEN NOT MATCHED THEN
                                    INSERT (szAccountId, szCurrencyId, decAmount) VALUES (Source.szAccountId, Source.szCurrencyId, Source.decAmount);";
                            using (var command = new OleDbCommand(updateBalanceQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@szAccountId", targetAccountId);
                                command.Parameters.AddWithValue("@szCurrencyId", request.CurrencyId);
                                command.Parameters.AddWithValue("@decAmount", request.Amount);  // Full amount for each target
                                command.ExecuteNonQuery();
                            }
                        }

                        // Update source account balance (deduct the total amount)
                        string updateSourceBalanceQuery = @"
                            UPDATE BOS_Balance
                            SET decAmount = decAmount - ?
                            WHERE szAccountId = ? AND szCurrencyId = ?";

                        using (var command = new OleDbCommand(updateSourceBalanceQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@decAmount", totalAmountToDeduct);  // Deduct total amount (Amount * number of target accounts)
                            command.Parameters.AddWithValue("@szAccountId", request.SourceAccountId);
                            command.Parameters.AddWithValue("@szCurrencyId", request.CurrencyId);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        public void Transfer(TransferRequest request)
        {
            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        string transactionId = GenerateTransactionId(connection, transaction);
                        string balanceQuery = "SELECT decAmount FROM BOS_Balance WHERE szAccountId = ? AND szCurrencyId = ?";
                        decimal currentBalance = 0;

                        using (var command = new OleDbCommand(balanceQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@szAccountId", request.SourceAccountId);
                            command.Parameters.AddWithValue("@szCurrencyId", request.CurrencyId);
                            var result = command.ExecuteScalar();
                            currentBalance = result != null ? Convert.ToDecimal(result) : 0;
                        }

                        // Calculate total amount to deduct from source account if divide change 
                        decimal totalAmountToDeduct = request.Amount * request.TargetAccountIds.Count;

                        if (currentBalance < totalAmountToDeduct)
                            throw new Exception("Saldo tidak mencukupi");

                        // Process each target account
                        foreach (var targetAccountId in request.TargetAccountIds)
                        {
                            // Insert history record for each target account
                            string insertQuery = @"
                                INSERT INTO BOS_History (szTransactionId, szAccountId, szCurrencyId, dtmTransaction, decAmount, szNote)
                                VALUES (?, ?, ?, GETDATE(), ?, ?)";
                            using (var command = new OleDbCommand(insertQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@szTransactionId", transactionId);
                                command.Parameters.AddWithValue("@szAccountId", targetAccountId);
                                command.Parameters.AddWithValue("@szCurrencyId", request.CurrencyId);
                                command.Parameters.AddWithValue("@decAmount", request.Amount);  // Full transfer amount for each target account
                                command.Parameters.AddWithValue("@szNote", request.Note);
                                command.ExecuteNonQuery();
                            }

                            // Update target account balance
                            string updateBalanceQuery = @"
                                MERGE INTO BOS_Balance AS Target
                                USING (SELECT ? AS szAccountId, ? AS szCurrencyId, ? AS decAmount) AS Source
                                ON Target.szAccountId = Source.szAccountId AND Target.szCurrencyId = Source.szCurrencyId
                                WHEN MATCHED THEN
                                    UPDATE SET Target.decAmount = Target.decAmount + Source.decAmount
                                WHEN NOT MATCHED THEN
                                    INSERT (szAccountId, szCurrencyId, decAmount) VALUES (Source.szAccountId, Source.szCurrencyId, Source.decAmount);";
                            using (var command = new OleDbCommand(updateBalanceQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@szAccountId", targetAccountId);
                                command.Parameters.AddWithValue("@szCurrencyId", request.CurrencyId);
                                command.Parameters.AddWithValue("@decAmount", request.Amount);  // Full amount for each target account
                                command.ExecuteNonQuery();
                            }
                        }

                        // Update source account balance (deduct the total amount)
                        string updateSourceBalanceQuery = @"
                            UPDATE BOS_Balance
                            SET decAmount = decAmount - ?
                            WHERE szAccountId = ? AND szCurrencyId = ?";

                        using (var command = new OleDbCommand(updateSourceBalanceQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@decAmount", totalAmountToDeduct);  // Deduct total amount (Amount * number of target accounts)
                            command.Parameters.AddWithValue("@szAccountId", request.SourceAccountId);
                            command.Parameters.AddWithValue("@szCurrencyId", request.CurrencyId);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        public List<TransactionHistory> GetHistory(string accountId, DateTime startDate, DateTime endDate)
        {
            var history = new List<TransactionHistory>();

            using (var connection = _dbHelper.GetConnection())
            {
                connection.Open();
                string query = @"
            SELECT szTransactionId, szAccountId, szCurrencyId, dtmTransaction, decAmount, szNote
            FROM BOS_History
            WHERE szAccountId = ? AND CAST(dtmTransaction AS DATE) BETWEEN ? AND ?";

                using (var command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@szAccountId", accountId);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            history.Add(new TransactionHistory
                            {
                                TransactionId = reader.GetString(0),
                                AccountId = reader.GetString(1),
                                CurrencyId = reader.GetString(2),
                                TransactionDate = reader.GetDateTime(3),
                                Amount = reader.GetDecimal(4),
                                Note = reader.GetString(5),
                                SourceAccountId = reader.GetString(1)
                            });
                        }
                    }
                }
            }

            return history;
        }

    }

}
