namespace ReportProcessor.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;

    using NLog;
    using ReportProcessor.Common;
    using ReportProcessor.Dtos;

    public class SqlService
    {
        public static bool UploadData(DataTable csvData, Logger logger)
        {
            bool isInserted = true;
            using (var dbConnection = new SqlConnection(Startup.ConnectionString))
            {
                dbConnection.Open();
                using (var bulkCopy = new SqlBulkCopy(dbConnection))
                {
                    bulkCopy.EnableStreaming = true;
                    bulkCopy.DestinationTableName = GlobalConstants.TableDestination;

                    foreach (var column in csvData.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ToString(), column.ToString());
                    }

                    try
                    {
                        bulkCopy.WriteToServer(csvData);
                    }
                    catch (Exception ex)
                    {
                        isInserted = false;
                        logger.Error(ex.GetType().ToString());
                        logger.Error(ex.Message);
                    }
                }
            }
            return isInserted;
        }

        public static IEnumerable<ShareClassDto> GetShareClassList(DateTime date, Logger logger)
        {
            var shareClassList = new List<ShareClassDto>();
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Startup.ConnectionString;
                connection.Open();

                SqlCommand command = connection.CreateCommand();

                var sqlDate = date.ToString(GlobalConstants.RequiredSqlDateTimeFormat, CultureInfo.InvariantCulture);

                command.CommandText = string.Format(GlobalConstants.FunctionShareClass, sqlDate);
                //command.CommandText = $"select * from dbo.fn_get_active_shares_expected_nav_date ('{sqlDate}')";

                var reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        shareClassList.Add(item: new ShareClassDto
                        {
                            IdSubFund = int.Parse(reader[GlobalConstants.ColumnIdSubFund].ToString()),
                            CurrencySubFund = reader[GlobalConstants.ColumnCurrencySubFund].ToString(),
                            IdShareClass = int.Parse(reader[GlobalConstants.ColumnIdShare].ToString()),
                            Isin = reader[GlobalConstants.ColumnIsin].ToString(),
                            CurrencyShare = reader[GlobalConstants.ColumnCurrencyShare].ToString(),
                            ExpectedNavDate = DateTime.Parse(reader[GlobalConstants.ColumnExpectedNavDate].ToString()),
                        });
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.GetType().ToString());
                    logger.Error(ex.Message);
                }
            }

            return shareClassList;
        }
    }
}