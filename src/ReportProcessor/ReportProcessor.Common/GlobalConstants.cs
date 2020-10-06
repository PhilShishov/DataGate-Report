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
        public const string FolderToWatch = @"D:\Reports\{0}\downloads\";
        public const string FolderOnSuccess = @"D:\Reports\{0}\historic\";
        public const string FolderOnError = @"D:\Reports\{0}\error\";
        public const string FolderHeaders = @"Datasets\";
        public static string[] ReportFolders = new string[3] { "EDR-SFTP01", "NT", "CACEIS" };
        public const string XmlRoot = "Providers";
        public const string RequiredDelimiter = ";";

        // SQL Server functions, tables and columns

        public const string TableDestination = "tb_shareclass_ts_test";
        public const string FunctionShareClass = "select distinct sc_id, sc_isinCode, sc_currency from tb_historyShareClass";
        public const string ColumnId = "sc_id";
        public const string ColumnIsin = "sc_isinCode";
        public const string ColumnCurrency = "sc_currency";
    }
}
