namespace ReportProcessor
{
    using System;
    using Microsoft.Extensions.Configuration;
    using ReportProcessor.Common;
    using ReportProcessor.Core;
    using ReportProcessor.Core.Contracts;
    using ReportProcessor.Data;

    public class Startup
    {
        public static void Main()
        {
            ConfigureBuilder();

            IEngine watcher = new Engine();
            watcher.Run();
        }

        public static string ConnectionString { get; set; }

        private static void ConfigureBuilder()
        {
            var environmentName = Environment.GetEnvironmentVariable("ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            ConnectionString = configuration.GetConnectionString(GlobalConstants.DataGateConnectionString);
        }
    }
}
