using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace ODataActionSample
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<Order> Orders { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }

        public double Price { get; set; }
    }

}