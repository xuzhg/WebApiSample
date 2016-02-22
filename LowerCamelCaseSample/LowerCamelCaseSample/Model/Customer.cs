using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LowerCamelCaseSample
{
    public class Customer
    {
        public int Id { get; set; }

        public ServiceStatus Status { get; set; }

        public Color Color { get; set; }

        public Order Order { get; set; }
    }
}
