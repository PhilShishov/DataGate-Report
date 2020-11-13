namespace ReportProcessor.Configuration
{
    using Microsoft.Extensions.Configuration;

    public static class BuilderConfiguration
    {
        public static IConfigurationRoot ConfigureBuilder()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //.AddJsonFile($"appsettings.Development.json", false, true)
                .AddJsonFile($"appsettings.Production.json", false, true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            return configuration;
        }
    }
}
