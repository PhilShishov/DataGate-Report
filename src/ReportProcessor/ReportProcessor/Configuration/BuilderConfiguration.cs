namespace ReportProcessor.Configuration
{
    using System;

    using Microsoft.Extensions.Configuration;
    using ReportProcessor.Common;

    public static class BuilderConfiguration
    {
        public static IConfigurationRoot ConfigureBuilder()
        {
            var environmentName = Environment.GetEnvironmentVariable(GlobalConstants.EnvironmentVariable);

            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            return configuration;
        }
    }
}
