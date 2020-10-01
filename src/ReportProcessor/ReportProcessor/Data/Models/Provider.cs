namespace ReportProcessor.Data.Models
{

    using System.Collections.Generic;

    public class Provider
    {
        public Provider()
        {
            this.Headers = new HashSet<Header>();
        }
        public int Id { get; set; }

        public ICollection<Header> Headers { get; set; }
    }
}
