namespace Bosnet.Utilities
{
    using System.Data.OleDb;

    public class DatabaseHelper
    {
        public string? _connectionString;

        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        public OleDbConnection GetConnection()
        {
            return new OleDbConnection(_connectionString);
        }
    }

}
