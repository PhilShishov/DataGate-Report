namespace ReportProcessor.Dtos
{
    using System;

    public class ExpectedResultDto
    {
        public DateTime DateReport { get; set; }

        public string Isin { get; set; }

        public string CurrencyShare { get; set; }

        public int Id { get; set; }

        public int ProviderId { get; set; }

        public string ProviderName { get; set; }
    }
}
