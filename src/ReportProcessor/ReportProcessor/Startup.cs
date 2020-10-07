namespace ReportProcessor
{
    using System;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog;
    using NLog.Extensions.Logging;
    using ReportProcessor.Common;
    using ReportProcessor.Core;
    using ReportProcessor.Core.Contracts;

    public class Startup
    {
        public static void Main()
        {
            Logger logger = LogManager.GetCurrentClassLogger();

            try
            {
                var config = ConfigureBuilder();
                var servicesProvider = BuildDi(config);

                using (servicesProvider as IDisposable)
                {
                    var runner = servicesProvider.GetRequiredService<Runner>();
                    runner.DoAction("Action1");

                    IEngine engine = new Engine();
                    engine.Run();
                }
            }
            catch (Exception ex)
            {
                // NLog: catch any exception and log it.
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }

        public static string ConnectionString { get; set; }

        private static IConfigurationRoot ConfigureBuilder()
        {
            var environmentName = Environment.GetEnvironmentVariable(GlobalConstants.EnvironmentVariable);

            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            ConnectionString = configuration.GetConnectionString(GlobalConstants.DataGateConnectionString);

            return configuration;
        }

        private static IServiceProvider BuildDi(IConfiguration config)
        {
            return new ServiceCollection()
               .AddTransient<Runner>() // Runner is the custom class
               .AddLogging(loggingBuilder =>
               {
                   // configure Logging with NLog
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(config);
               })
               .BuildServiceProvider();
        }
    }
}
