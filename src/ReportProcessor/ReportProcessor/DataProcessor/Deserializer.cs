
namespace ReportProcessor.DataProcessor
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    using ReportProcessor.Data.Models;
    using ReportProcessor.DataProcessor.ImportDto;
    public class Deserializer
    {
        public static Provider ImportHeaders(string xmlString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportProviderDto[]),
                                            new XmlRootAttribute("Providers"));

            var providersDto = (ImportProviderDto[])xmlSerializer.Deserialize(new StringReader(xmlString));

            //var sb = new StringBuilder();
            var provider = new Provider();

            foreach (var dto in providersDto)
            {
                provider.Id = dto.Provider;

                foreach (var headerDto in dto.Headers)
                {
                    provider.Headers.Add(new Header
                    {
                        Name = headerDto.Name.Trim(),
                        Id_TS = headerDto.Id_TS,
                    });
                }
                //sb.AppendLine($"Successfully imported projection {movie.Title} on {projection.DateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}!");
            }

            //string result = sb.ToString().TrimEnd();

            return provider;
        }
    }
}
