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
                    bulkCopy.DestinationTableName = "tb_shareclass_ts_test";

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

                command.CommandText = $"select distinct sc_id, sc_isinCode, sc_currency from tb_historyShareClass";
                //command.CommandText = $"select * from fn_timeseries_shareclass ('{sqlDate}', '{isin}'";

                //select* from fn_timeseries_shareclass('20190101', 'LU0828733419')

                var reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        shareClassList.Add(item: new ShareClassDto
                        {
                            Id = int.Parse(reader["sc_id"].ToString()),
                            Isin = reader["sc_isinCode"].ToString(),
                            Currency = reader["sc_currency"].ToString(),
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