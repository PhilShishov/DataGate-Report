namespace ReportProcessor
{
    using Microsoft.Extensions.Configuration;
    using NLog;
    using ReportProcessor.Common;
    using ReportProcessor.Configuration;
    using ReportProcessor.Core;
    using ReportProcessor.Core.Contracts;

    public class Startup
    {
        public static void Main()
        {
            Logger logger = LogManager.GetCurrentClassLogger();

            try
            {
                var config = BuilderConfiguration.ConfigureBuilder();
                ConnectionString = config.GetConnectionString(GlobalConstants.DataGateConnectionString);

                IEngine engine = new Engine();
                engine.Run(logger);
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }

        public static string ConnectionString { get; set; }
    }
}
