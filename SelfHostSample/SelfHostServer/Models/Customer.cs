using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostServer.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public Color FavoriteColor { get; set; }

        public Address Location { get; set; }

        public Category Category { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
