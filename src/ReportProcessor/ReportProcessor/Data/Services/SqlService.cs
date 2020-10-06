namespace ReportProcessor.Data.Services
{
    using ReportProcessor.Common;
    using ReportProcessor.Dtos;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;

    public class SqlService
    {
        public static bool UploadData(DataTable csvData)
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
                        Console.WriteLine(ex.GetType().ToString());
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return isInserted;
        }

        public static IEnumerable<ShareClassDto> GetShareClassList(DateTime date)
        {
            var shareClassList = new List<ShareClassDto>();
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Startup.ConnectionString;
                connection.Open();

                SqlCommand command = connection.CreateCommand();

                var sqlDate = date.ToString(GlobalConstants.RequiredSqlDateTimeFormat, CultureInfo.InvariantCulture);

                command.CommandText = GlobalConstants.FunctionShareClass;

                //command.CommandText = $"select * from fn_timeseries_shareclass ('{sqlDate}'";
                //select* from fn_timeseries_shareclass('20190101')

                var reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        shareClassList.Add(item: new ShareClassDto
                        {
                            Id = int.Parse(reader[GlobalConstants.ColumnId].ToString()),
                            Isin = reader[GlobalConstants.ColumnIsin].ToString(),
                            Currency = reader[GlobalConstants.ColumnCurrency].ToString(),
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetType().ToString());
                    Console.WriteLine(ex.Message);
                }
            }

            return shareClassList;
        }
    }
}