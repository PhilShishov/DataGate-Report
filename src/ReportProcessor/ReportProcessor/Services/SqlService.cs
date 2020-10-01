namespace ReportProcessor.Services
{
    using ReportProcessor.Common;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;

    public class SqlService
    {
        public static bool IsDataInsertedDB(DataTable csvData)
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

        public static int GetId(string isin, DateTime date)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = Startup.ConnectionString;
                connection.Open();

                SqlCommand command = connection.CreateCommand();

                var sqlDate = date.ToString(GlobalConstants.RequiredSqlDateTimeFormat, CultureInfo.InvariantCulture);

                command.CommandText = $"select sc_id from tb_historyShareClass where sc_isinCode = '{isin}'";
                //command.CommandText = $"select * from fn_timeseries_shareclass ('{sqlDate}', '{isin}'";

                //select* from fn_timeseries_shareclass('20190101', 'LU0828733419')

                return (int)command.ExecuteScalar();
            }
        }
    }
}