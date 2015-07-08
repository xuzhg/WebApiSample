using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleODataClientTest
{
    public class People
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<string> Emails { get; set; }

        public IList<Location> AddressInfo { get; set; }

        public PersonGender Gender { get; set; }
    }

    public class Location
    {
        public string Address { get; set; }

        public City City { get; set; }
    }

    public class City
    {
        public string CountryRegion { get; set; }

        public string Name { get; set; }

        public string Region { get; set; }
    }

    public enum PersonGender
    {
        Female,
        Male
    }
}
