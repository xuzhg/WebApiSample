using System.Globalization;

namespace TimeZoneSamples.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string RawCreatedDate
        {
            get
            {
                CultureInfo enUS = new CultureInfo("en-US");
                return CreatedDate.ToString("o", enUS);
            }
            set
            {
                // do nothing here
            }
        }

        public DateTime CreatedDate { get; set; }

        public DateTimeOffset DeliverDate { get; set; }
    }
}
