using System;

namespace ReportProcessor.Dtos
{
    public class ShareClassDto
    {
        public int IdSubFund { get; set; }

        public string Isin { get; set; }

        public string CurrencyShare { get; set; }

        public string CurrencySubFund { get; set; }

        public int IdShareClass { get; set; }

        public DateTime ExpectedNavDate { get; set; }
    }
}
