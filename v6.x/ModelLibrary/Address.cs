using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    public class Address
    {
        public string Street { get; set; }

        public City CityInfo { get; set; }

        public IList<City> Cities { get; set; } 
    }
}
