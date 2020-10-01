namespace ReportProcessor.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Provider")]
    public class ImportProviderDto
    {
        [XmlAttribute("id")]
        public int Provider { get; set; }

        [XmlArray("Headers")]
        public ImportHeaderDto[] Headers { get; set; }
    }

    [XmlType("Header")]
    public class ImportHeaderDto
    {
        [XmlElement("Name")]
        [Required]
        public string Name { get; set; }

        [XmlElement("Id_TS")]
        public int Id_TS { get; set; }

    }
}
