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

            // Security check 1: ISIN and currency in report are existing and the same as in internal DB
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

            // Security check 2: Provider matching
            if (shareClass.Provider != dto.ProviderName)
            {
                logger.Error(string.Format(ErrorMessages.InvalidProvider, dto.Isin, dto.ProviderName));
                hasPassedSecurity = false;
            }

            // Security check 3: Nav date matching expected date from internal DB
            if (shareClass.ExpectedNavDate != dto.DateReport)
            {
                logger.Error(string.Format(ErrorMessages.InvalidExpectedDate, dto.Isin, dto.DateReport));
                hasPassedSecurity = false;
            }

            // Security check 4: Aggregated sf Aum and share sum of AuM have same result
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
