using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    public class Customer
    {
        public int Id { get; set; }

        public Address Location { get; set; }

        public ICollection<Address> Locations { get; set; }
    }
}
