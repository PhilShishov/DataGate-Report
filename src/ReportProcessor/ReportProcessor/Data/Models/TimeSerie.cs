namespace ReportProcessor.Data.Models
{
    using System;

    public class TimeSerie
    {
        // Model template to map the one in database

        public DateTime date_ts { get; set; }

        public int id_ts { get; set; }

        public decimal value_ts { get; set; }

        public string currency_ts { get; set; }

        public int provider_ts  { get; set; }

        public int id_shareclass { get; set; }
    }
}
