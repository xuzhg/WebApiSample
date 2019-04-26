using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderbyTest.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class SpecialOrder : Order
    {
        public int Price { get; set; }
    }
}
