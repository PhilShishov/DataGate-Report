namespace ReportProcessor.Common
{
    public static class GlobalConstants
    {
        // Application
        public const string EnvironmentVariable = "ENVIRONMENT";
        public const string NetCoreApp = "netcoreapp";
        public const string PermissionSetFullTrust = "FullTrust";

        // Connection Strings
        public const string DataGateConnectionString = "DataGate_vFinaleConnection";

        // File formats
        public const string ExcelFileExtension = ".xlsx";
        public const string CSVFileExtension = ".csv";

        // Dates
        public const string RequiredSqlDateTimeFormat = "yyyyMMdd";

        // Business
        // Linux server ubuntu paths
        //public const string FolderToWatch = @"/home/ReportsTest/{0}/downloads/";
        //public const string FolderOnSuccess = @"/home/ReportsTest/{0}/historic/";
        //public const string FolderOnError = @"/home/ReportsTest/{0}/error/";

        // Windows server ubuntu paths
        public const string FolderToWatch = @"C:/Reports/{0}/downloads/";
        public const string FolderOnSuccess = @"C:/Reports/{0}/historic/";
        public const string FolderOnError = @"C:/Reports/{0}/error/";

        public const string FolderHeaders = @"Datasets/";

        // Report folder
        public static string[] ReportFolders = new string[3] { "EDR-SFTP01", "NT", "CACEIS" };
        public const string XmlRoot = "Providers";
        public const string RequiredDelimiter = ";";
        public const string HeadersFileName = "headers-{0}.xml";

        // SQL Server functions, tables and columns
        public const string TableDestination = "tb_shareclass_ts_test";
        public const string FunctionShareClass = "select * from dbo.fn_get_active_shares_expected_nav_date ('{0}')";
        public const string ColumnIdSubFund = "sf_id";
        public const string ColumnCurrencySubFund = "sf_currency";
        public const string ColumnIdShare = "sc_id";
        public const string ColumnIsin = "sc_isin";
        public const string ColumnCurrencyShare = "sc_ccy";
        public const string ColumnExpectedNavDate = "ExpectedNavDate";
    }
}
