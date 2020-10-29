namespace ReportProcessor.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using CsvHelper;
    using NLog;
    using ReportProcessor.Common;
    using ReportProcessor.Data.Models;
    using ReportProcessor.Data.Services;
    using ReportProcessor.Dtos;

    public class DataHandler
    {
        // XML Headers index
        private const int IndexNavDate = 0;
        private const int IndexCurrency = 1;
        private const int IndexIsin = 2;
        private const int CountHeadersToSkip = 3;

        public static List<TimeSerie> ProcessData(Provider provider, string csv_file_path, Logger logger)
        {
            var records = new List<TimeSerie>();

            try
            {
                // Retrieve shareclass sql table by date of today to perform security checks
                var shareClassList = SqlService.GetShareClassList(DateTime.Today, logger);

                using (var streamReader = new StreamReader(csv_file_path))
                {
                    using (CsvReader reader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                    {
                        reader.Configuration.Delimiter = GlobalConstants.RequiredDelimiter;
                        reader.Read();
                        reader.ReadHeader();

                        while (reader.Read())
                        {
                            var headers = provider.Headers.ToArray();

                            //Expected result coming from official file - isin, currencyShare, dateReport
                            ExpectedResultDto dto = new ExpectedResultDto();

                            var providerId = provider.Id;
                            var dateField = reader.GetField(headers[IndexNavDate].Name);
                            dto.DateReport = new DateTime(Convert.ToInt32(dateField.Substring(0, 4)), // Year
                                    Convert.ToInt32(dateField.Substring(4, 2)), // Month
                                    Convert.ToInt32(dateField.Substring(6, 2)));// Day
                            dto.CurrencyShare = reader.GetField(headers[IndexCurrency].Name);
                            dto.Isin = reader.GetField(headers[IndexIsin].Name);

                            // Map time series types between xml and source 
                            var types = new List<TimeSerieType>();

                            for (int i = CountHeadersToSkip; i < headers.Length; i++)
                            {
                                var type = new TimeSerieType
                                {
                                    Id = headers[i].Id_TS,
                                    Value = reader.GetField<decimal>(headers[i].Name),
                                };

                                types.Add(type);
                            }

                            // First security check: ISIN and currency in report are existing and the same as in internal DB
                            // Second security check: Nav date matching expected date from internal DB
                            var currentShareClass = shareClassList
                                .FirstOrDefault(
                                sc => sc.Isin == dto.Isin && 
                                sc.CurrencyShare == dto.CurrencyShare && 
                                sc.ExpectedNavDate == dto.DateReport);

                            // Check result from security before creating new entity
                            bool didPassSecurity = Controller.SecurityCheck(currentShareClass, dto, logger);

                            if (!didPassSecurity)
                            {
                                records = null;
                                break;
                            }

                            int id_sc = currentShareClass.IdShareClass;

                            // Different time series type creates new entry in DB
                            foreach (var type in types)
                            {
                                var record = new TimeSerie
                                {
                                    date_ts = dto.DateReport,
                                    id_ts = type.Id,
                                    value_ts = type.Value,
                                    currency_ts = dto.CurrencyShare,
                                    provider_ts = providerId,
                                    id_shareclass = id_sc,
                                };
                                records.Add(record);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return null;
            }
            return records;
        }
    }
}
