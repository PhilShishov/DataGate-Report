namespace ReportProcessor.DataProcessor
{
    using NLog;
    using ReportProcessor.Common;
    using ReportProcessor.Dtos;

    public static class Controller
    {
        public static bool SecurityCheck(ShareClassDto shareClass, ExpectedResultDto dto, Logger logger)
        {
            bool hasPassedCheck = true;

          
            if (shareClass == null)
            {
                logger.Error(string.Format(ErrorMessages.InvalidIsin, dto.Isin, dto.CurrencyShare, dto.DateReport));;
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
