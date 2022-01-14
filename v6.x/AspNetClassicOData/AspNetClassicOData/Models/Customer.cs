using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetClassicOData.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [ShowHidden]
        public int Age { get; set; }

        [ShowHidden]
        public List<int> Nums { get; set; }

        public List<string> Emails { get; set; } // no hidden

        [ShowHidden]
        public Address HomeAdress { get; set; }

        public Address NoHiddenAddress { get; set; }

        [ShowHidden]
        public IList<Address> MailAddresses { get; set; }

        [ShowHidden]
        public Order SingleOrder { get; set; }

        [ShowHidden]
        public IList<Order> Orders { get; set; }

    }

    public class Address
    {
        public string City { get; set; }

        public string Street { get; set; }

    }

    public class Order
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}