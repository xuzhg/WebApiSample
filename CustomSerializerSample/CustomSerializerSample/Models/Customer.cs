using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomSerializerSample.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<Order> Orders { get; set; }
    }
}