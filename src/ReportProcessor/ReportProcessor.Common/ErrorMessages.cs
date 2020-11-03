namespace ReportProcessor.Common
{
    public static class ErrorMessages
    {
        public const string InvalidData = "Invalid data!";
        public const string InvalidIsin = "Error: {0} with {1} currency was not found!";
        public const string InvalidProvider = "Error: {0} with {1} provider was not found!";
        public const string MissingProvider = "Error: Please add company agreement type 8 for {0}";
        public const string InvalidExpectedDate = "Error: {0} at {1} date was not found!";
        public const string StopProgramException = "Stopped program because of exception";
    }
}
