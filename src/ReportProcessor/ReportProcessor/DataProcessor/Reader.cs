namespace ReportProcessor.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using CsvHelper;
    using ReportProcessor.Common;
    using ReportProcessor.Data.Models;
    using ReportProcessor.Data.Services;

    public class Reader
    {
        // XML Headers index
        private const int IndexDate = 0;
        private const int IndexCurrency = 1;
        private const int IndexIsin = 2;
        private const int CountHeadersToSkip = 3;

        public static List<TimeSerie> ProcessData(Provider provider, string csv_file_path)
        {
            var records = new List<TimeSerie>();

            // Retrieve shareclass sql table by date of today to perform security checks
            var shareClassList = SqlService.GetShareClassList(DateTime.Today);

            try
            {
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

                            var providerId = provider.Id;
                            var dateField = reader.GetField(headers[IndexDate].Name);
                            var dateReport = new DateTime(Convert.ToInt32(dateField.Substring(0, 4)), // Year
                                    Convert.ToInt32(dateField.Substring(4, 2)), // Month
                                    Convert.ToInt32(dateField.Substring(6, 2)));// Day
                            var currency = reader.GetField(headers[IndexCurrency].Name);
                            var isin = reader.GetField(headers[IndexIsin].Name);

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

                            var currentShareClass = shareClassList.FirstOrDefault(sc => sc.Isin == isin && sc.Currency == currency);

                            bool didPassSecurity = Controller.SecurityCheck(currentShareClass, isin, currency);

                            if (!didPassSecurity)
                            {
                                records = null;
                                break;
                            }

                            int id_sc = currentShareClass.Id;

                            // Different time series type creates new entry in DB
                            foreach (var type in types)
                            {
                                var record = new TimeSerie
                                {
                                    date_ts = dateReport,
                                    id_ts = type.Id,
                                    value_ts = type.Value,
                                    currency_ts = currency,
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
                Console.WriteLine(ex.Message);
                return null;
            }
            return records;
        }
    }
}
