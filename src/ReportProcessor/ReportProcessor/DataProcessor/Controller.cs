namespace ReportProcessor.DataProcessor
{
    using System;

    using ReportProcessor.Dtos;

    public static class Controller
    {
        public static bool SecurityCheck(ShareClassDto shareClass)
        {
            bool hasError = false;

            // First security check: ISIN and currency in report are existing and the same as in internal DB
            if (shareClass == null)
            {
                Console.WriteLine($"Error: {shareClass.Isin} with {shareClass.Currency} currency was not found!");
                hasError = true;
            }

            return hasError;
        }
    }
}
