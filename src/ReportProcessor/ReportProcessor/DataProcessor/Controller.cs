﻿namespace ReportProcessor.DataProcessor
{
    using NLog;
    using ReportProcessor.Common;
    using ReportProcessor.Dtos;

    public static class Controller
    {
        public static bool SecurityCheck(ShareClassDto shareClass, string isin, string currency, Logger logger)
        {
            bool hasPassedCheck = true;

            // First security check: ISIN and currency in report are existing and the same as in internal DB
            if (shareClass == null)
            {
                logger.Error(string.Format(ErrorMessages.InvalidIsin, isin, currency));
                hasPassedCheck = false;
            }

            return hasPassedCheck;
        }
    }
}
