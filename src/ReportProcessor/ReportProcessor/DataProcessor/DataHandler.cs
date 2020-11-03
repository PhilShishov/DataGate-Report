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

            bool didPassSecurity = true;

            try
            {
                //var creationDate = File.GetCreationTime(csv_file_path);

                var creationDate = new DateTime(2020, 07, 23);
                // Retrieve shareclass sql table by date of today to perform security checks
                var actualShareClassList = SqlService.GetShareClassList(creationDate, logger);

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

                            dto.ProviderId = provider.Id;
                            dto.ProviderName = provider.Title;
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

                            // Check result from security before creating new entity
                            didPassSecurity = Controller.SecurityCheck(actualShareClassList, dto, logger);

                            if (!didPassSecurity)
                            {
                                records = null;
                                continue;
                            }

                            // Different time series type creates new entry in DB
                            foreach (var type in types)
                            {
                                var record = new TimeSerie
                                {
                                    date_ts = dto.DateReport,
                                    id_ts = type.Id,
                                    value_ts = type.Value,
                                    currency_ts = dto.CurrencyShare,
                                    provider_ts = dto.ProviderId,
                                    id_shareclass = dto.Id,
                                    file_name = Path.GetFileName(csv_file_path),
                                };
                                records.Add(record);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                didPassSecurity = true;
                logger.Error(ex.Message);                
            }

            if (!didPassSecurity)
            {
                return null;
            }

            return records;
        }
    }
}
