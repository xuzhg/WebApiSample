using System;
using System.Collections.Generic;
using System.Text;

namespace NS
{
    public class Customer
    {
        public int Id { get; set; }

        public Address HomeLocation { get; set; }

        public IList<Address> Locations { get; set; }

        public Order PrivateOrder { get; set; }

        public IList<Order> Orders { get; set; }
    }

    public class VipCustomer : Customer
    {
        public string VipName { get; set; }

        public Address VipAddress { get; set; }

        public Order VipOrder { get; set; }

    }
}
