using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicWebApiSample.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Address HomeAddress { get; set; }

        public Color FavoriateColor { get; set; }

        public IList<Order> Orders { get; set; }
    }
}
