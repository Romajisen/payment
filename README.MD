# Bosnet_api
## How to install requirement
Download link tools below:
1. Microsoft visual studio code : https://code.visualstudio.com/Download
2. Microsoft dot.net core 8.0 ltz : https://dotnet.microsoft.com/en-us/download/dotnet/8.0
3. Microsoft SQL server : https://www.microsoft.com/en-us/sql-server/sql-server-downloads
4. Install postman : https://www.postman.com/downloads/

## How to run project Bosnet_api
Before run the project :
1.  Change stringconnection 
## struktur project path
``` bash
    Bosnet/ 
    └── appsettings.json
```
## change to
``` bash
"ConnectionStrings": {
    "DefaultConnection": "Provider=SQLOLEDB;Data Source={{Hostname}};Initial Catalog={{DatabaseName}};User ID={{UserID}};Password={{Password}};MultipleActiveResultSets=True;Trusted_Connection=True;TrustServerCertificate=True;"
  }
 .... 
```
2.  Copy script dll below on sql server tools:
    ``` bash
    -- ----------------------------
    -- Table structure for BOS_History
    -- ----------------------------
    IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[BOS_History]') AND type IN ('U'))
        DROP TABLE [dbo].[BOS_History]
    GO

    CREATE TABLE [dbo].[BOS_History] (
    [szTransactionId] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
    [szAccountId] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
    [szCurrencyId] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
    [dtmTransaction] datetime  NOT NULL,
    [decAmount] decimal(30,8)  NOT NULL,
    [szNote] nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL
    )
    GO

    ALTER TABLE [dbo].[BOS_History] SET (LOCK_ESCALATION = TABLE)
    GO


    -- ----------------------------
    -- Records of BOS_History
    -- ----------------------------
    INSERT INTO [dbo].[BOS_History] ([szTransactionId], [szAccountId], [szCurrencyId], [dtmTransaction], [decAmount], [szNote]) VALUES (N'20201231-00000.00001', N'000108757484', N'IDR', N'2024-12-20 23:32:14.907', N'34500000.00000000', N'SETOR')
    GO

    INSERT INTO [dbo].[BOS_History] ([szTransactionId], [szAccountId], [szCurrencyId], [dtmTransaction], [decAmount], [szNote]) VALUES (N'20201231-00000.00001', N'000108757484', N'SGD', N'2024-12-20 23:32:14.907', N'125.87500000', N'SETOR')
    GO

    INSERT INTO [dbo].[BOS_History] ([szTransactionId], [szAccountId], [szCurrencyId], [dtmTransaction], [decAmount], [szNote]) VALUES (N'20201231-00000.00002', N'000109999999', N'IDR', N'2024-12-20 23:32:14.907', N'1250.00000000', N'SETOR')
    GO

    INSERT INTO [dbo].[BOS_History] ([szTransactionId], [szAccountId], [szCurrencyId], [dtmTransaction], [decAmount], [szNote]) VALUES (N'20201231-00000.00003', N'000109999999', N'SGD', N'2024-12-20 23:32:14.907', N'128.00000000', N'SETOR')
    GO

    INSERT INTO [dbo].[BOS_History] ([szTransactionId], [szAccountId], [szCurrencyId], [dtmTransaction], [decAmount], [szNote]) VALUES (N'20201231-00000.00004', N'000108888888', N'SGD', N'2024-12-20 23:32:14.907', N'125.75000000', N'TRANSFER')
    GO

    INSERT INTO [dbo].[BOS_History] ([szTransactionId], [szAccountId], [szCurrencyId], [dtmTransaction], [decAmount], [szNote]) VALUES (N'20201231-00000.00004', N'000109999999', N'SGD', N'2024-12-20 23:32:14.907', N'-125.75000000', N'TRANSFER')
    GO

    INSERT INTO [dbo].[BOS_History] ([szTransactionId], [szAccountId], [szCurrencyId], [dtmTransaction], [decAmount], [szNote]) VALUES (N'20241220-00000.00005', N'000109999999', N'IDR', N'2024-12-21 00:07:18.877', N'-100.00000000', N'TARIK')
    GO

    INSERT INTO [dbo].[BOS_History] ([szTransactionId], [szAccountId], [szCurrencyId], [dtmTransaction], [decAmount], [szNote]) VALUES (N'20241220-00000.00006', N'000108888888', N'IDR', N'2024-12-21 00:09:30.300', N'110.00000000', N'TRANSFER')
    GO

    INSERT INTO [dbo].[BOS_History] ([szTransactionId], [szAccountId], [szCurrencyId], [dtmTransaction], [decAmount], [szNote]) VALUES (N'20241220-00000.00007', N'000108888888', N'IDR', N'2024-12-21 00:10:50.640', N'1110.00000000', N'SETOR')
    GO

    INSERT INTO [dbo].[BOS_History] ([szTransactionId], [szAccountId], [szCurrencyId], [dtmTransaction], [decAmount], [szNote]) VALUES (N'20241221-00000.00001', N'000108888888', N'IDR', N'2024-12-21 08:15:06.360', N'-510.00000000', N'TARIK')
    GO

    INSERT INTO [dbo].[BOS_History] ([szTransactionId], [szAccountId], [szCurrencyId], [dtmTransaction], [decAmount], [szNote]) VALUES (N'20241221-00000.99999', N'000109999999', N'IDR', N'2024-12-21 08:10:46.660', N'-210.00000000', N'TARIK')
    GO

    INSERT INTO [dbo].[BOS_History] ([szTransactionId], [szAccountId], [szCurrencyId], [dtmTransaction], [decAmount], [szNote]) VALUES (N'20241221-00001.99999', N'000109999999', N'IDR', N'2024-12-21 08:10:58.500', N'-250.00000000', N'TARIK')
    GO

    INSERT INTO [dbo].[BOS_History] ([szTransactionId], [szAccountId], [szCurrencyId], [dtmTransaction], [decAmount], [szNote]) VALUES (N'20241221-99999.99999', N'000109999999', N'IDR', N'2024-12-21 08:11:47.317', N'-50.00000000', N'TARIK')
    GO


    -- ----------------------------
    -- Table structure for BOS_Balance
    -- ----------------------------
    IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[BOS_Balance]') AND type IN ('U'))
        DROP TABLE [dbo].[BOS_Balance]
    GO

    CREATE TABLE [dbo].[BOS_Balance] (
    [szAccountId] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
    [szCurrencyId] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
    [decAmount] decimal(30,8)  NOT NULL
    )
    GO

    ALTER TABLE [dbo].[BOS_Balance] SET (LOCK_ESCALATION = TABLE)
    GO


    -- ----------------------------
    -- Records of BOS_Balance
    -- ----------------------------
    INSERT INTO [dbo].[BOS_Balance] ([szAccountId], [szCurrencyId], [decAmount]) VALUES (N'000108757484', N'IDR', N'34500000.00000000')
    GO

    INSERT INTO [dbo].[BOS_Balance] ([szAccountId], [szCurrencyId], [decAmount]) VALUES (N'000108757484', N'USD', N'125.87500000')
    GO

    INSERT INTO [dbo].[BOS_Balance] ([szAccountId], [szCurrencyId], [decAmount]) VALUES (N'000108888888', N'IDR', N'660.00000000')
    GO

    INSERT INTO [dbo].[BOS_Balance] ([szAccountId], [szCurrencyId], [decAmount]) VALUES (N'000108888888', N'SGD', N'125.75000000')
    GO

    INSERT INTO [dbo].[BOS_Balance] ([szAccountId], [szCurrencyId], [decAmount]) VALUES (N'000109999999', N'IDR', N'80.00000000')
    GO

    INSERT INTO [dbo].[BOS_Balance] ([szAccountId], [szCurrencyId], [decAmount]) VALUES (N'000109999999', N'SGD', N'2.25000000')
    GO


    -- ----------------------------
    -- Table structure for BOS_Counter
    -- ----------------------------
    IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[BOS_Counter]') AND type IN ('U'))
        DROP TABLE [dbo].[BOS_Counter]
    GO

    CREATE TABLE [dbo].[BOS_Counter] (
    [szCounterId] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
    [iLastNumber] bigint DEFAULT 0 NOT NULL,
    [iFirstPart] bigint DEFAULT 0 NULL
    )
    GO

    ALTER TABLE [dbo].[BOS_Counter] SET (LOCK_ESCALATION = TABLE)
    GO


    -- ----------------------------
    -- Records of BOS_Counter
    -- ----------------------------
    INSERT INTO [dbo].[BOS_Counter] ([szCounterId], [iLastNumber], [iFirstPart]) VALUES (N'001-COU', N'4', N'0')
    GO


    -- ----------------------------
    -- Primary Key structure for table BOS_History
    -- ----------------------------
    ALTER TABLE [dbo].[BOS_History] ADD CONSTRAINT [PK_BOS_History] PRIMARY KEY CLUSTERED ([szTransactionId], [szAccountId], [szCurrencyId])
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
    ON [PRIMARY]
    GO


    -- ----------------------------
    -- Primary Key structure for table BOS_Balance
    -- ----------------------------
    ALTER TABLE [dbo].[BOS_Balance] ADD CONSTRAINT [PK_BOS_Balance] PRIMARY KEY CLUSTERED ([szAccountId], [szCurrencyId])
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
    ON [PRIMARY]
    GO


    -- ----------------------------
    -- Primary Key structure for table BOS_Counter
    -- ----------------------------
    ALTER TABLE [dbo].[BOS_Counter] ADD CONSTRAINT [PK_BOS_Counter] PRIMARY KEY CLUSTERED ([szCounterId])
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
    ON [PRIMARY]
    GO
    ```

2. Build or Run project on visual studio / vscode :
    ## command build
    ``` bash
    dotnet build
    ```
    ## command run
    ``` bash
        dotnet run
    ```
3.  Swagger or Postman :
    ## import postman json documentation file on structure path on Docs Bosnet.postman_collection.json and BosnetEnv.postman_collection.json
    `` bash
    Bosnet/
    ├── bin
    ├── Controllers
    │   └── TransactionController.cs 
    ├── Docs/
    │   ├── Bosnet.postman_collection.json 
    │   └── BosnetEnv.postman_collection.json
    ├── Models
    │   ├── SetorRequest.cs 
    │   ├── TarikRequest.cs 
    │   ├── TransactionHistory.cs 
    │   └── TransferRequest.cs 
    ├── obj
    ├── Properties
    │   └── launchSettings.json 
    ├── Services
    │   └── TransactionService.cs 
    ├── SQL
    │   └── BOS_Struct_Data.sql
    ├── Utilities
    │   └── DatabaseHelper.cs   
    └── appsettings.json
    └── Bosnet.csproj   
    └── Bosnet.csproj.user
    └── Program.cs
    └── README.MD
    ```
    ## run swagger
    ``` bash

    http://localhost:3000/swagger/index.html

    ```