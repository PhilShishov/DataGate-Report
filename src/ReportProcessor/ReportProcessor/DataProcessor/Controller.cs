namespace ReportProcessor.DataProcessor
{
    using System.Collections.Generic;
    using System.Linq;

    using NLog;
    using ReportProcessor.Common;
    using ReportProcessor.Dtos;

    public static class Controller
    {
        public static bool SecurityCheck(IEnumerable<ShareClassDto> actualShareClassList, ExpectedResultDto dto, Logger logger)
        {
            bool hasPassedSecurity = true;

            // First security check: ISIN and currency in report are existing and the same as in internal DB
            var shareClass = actualShareClassList
                .FirstOrDefault(
                sc => sc.Isin == dto.Isin &&
                sc.CurrencyShare == dto.CurrencyShare);

            if (shareClass == null)
            {
                logger.Error(string.Format(ErrorMessages.InvalidIsin, dto.Isin, dto.CurrencyShare));;
                hasPassedSecurity = false;
            }

            // Missing internal provider check
            if (shareClass.Provider == null)
            {
                logger.Error(string.Format(ErrorMessages.MissingProvider, dto.Isin)); ;
                hasPassedSecurity = false;
            }

            // Second security check: Provider matching
            if (shareClass.Provider != dto.ProviderName)
            {
                logger.Error(string.Format(ErrorMessages.InvalidProvider, dto.Isin, dto.ProviderName));
                hasPassedSecurity = false;
            }

            // Third security check: Nav date matching expected date from internal DB
            if (shareClass.ExpectedNavDate != dto.DateReport)
            {
                logger.Error(string.Format(ErrorMessages.InvalidExpectedDate, dto.Isin, dto.DateReport));
                hasPassedSecurity = false;
            }

            // Fourth security check: Aggregated sf Aum and share sum of AuM have same result
            //if (shareClass == null)
            //{
            //    logger.Error(string.Format(ErrorMessages.InvalidAuM, isin, currency));
            //    hasPassedSecurity = false;
            //}

            // Set shareclass Id
            dto.Id = shareClass.IdShareClass;

            return hasPassedSecurity;
        }
    }
}
