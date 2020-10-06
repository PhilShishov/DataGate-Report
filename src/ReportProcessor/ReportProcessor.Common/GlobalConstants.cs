namespace ReportProcessor.Common
{
    public static class GlobalConstants
    {
        public const string FolderToWatch = @"D:\Reports\{0}\downloads\";
        public const string FolderOnSuccess = @"D:\Reports\{0}\historic\";
        public const string FolderOnError = @"D:\Reports\{0}\error\";
        public static string[] ReportFolders = new string[3] { "EDR-SFTP01", "NT", "CACEIS" };

        public const string PermissionSetFullTrust = "FullTrust";

        // Connection Strings
        public const string DataGateConnectionString = "DataGate_vFinaleConnection";

        // File formats
        public const string ExcelFileExtension = ".xlsx";
        public const string CSVFileExtension = ".csv";

        // Dates
        public const string RequiredSqlDateTimeFormat = "yyyyMMdd";
    }
}
