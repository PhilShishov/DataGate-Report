namespace ReportProcessor.DataProcessor
{
    using NLog;
    using ReportProcessor.Common;
    using ReportProcessor.Dtos;
    using System;

    public static class Controller
    {
        public static bool SecurityCheck(ShareClassDto shareClass, string isin, string currency, DateTime date, Logger logger)
        {
            bool hasPassedCheck = true;

          
            if (shareClass == null)
            {
                logger.Error(string.Format(ErrorMessages.InvalidIsin, isin, currency, date));
                hasPassedCheck = false;
            }

            // Third security check: Aggregated sf Aum and share sum of AuM have same result
            //if (shareClass == null)
            //{
            //    logger.Error(string.Format(ErrorMessages.InvalidIsin, isin, currency));
            //    hasPassedCheck = false;
            //}

            return hasPassedCheck;
        }
    }
}
