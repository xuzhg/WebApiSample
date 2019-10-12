using System;
using System.Collections.Generic;
using System.Text;

namespace NS
{
    public class Address
    {
        public string Street { get; set; }

        public string Region { get; set; }

        public IList<string> Emails { get; set; }

        public City RelatedCity { get; set; }

        public IList<City> Cities { get; set; }

    }

    public class CnAddress : Address
    {
        public string PostCode { get; set; }

        public City CnCity { get; set; }
    }

    public class UsAddress : Address
    {
        public string ZipCode { get; set; }

        public IList<City> UsCities { get; set; }
    }

}
