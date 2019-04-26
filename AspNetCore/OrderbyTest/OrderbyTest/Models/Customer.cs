using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderbyTest.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<Address> Locations { get; set; }

        public IList<Order> Orders { get; set; }
    }

    public class SpecialCustomer : Customer
    {
        public string Token { get; set; }
    }
}
