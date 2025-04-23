using System.Collections.Generic;
using System.Drawing;
using System.Net;

namespace MinimalApiODataTest.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public Color FavoriteColor { get; set; }

        public virtual Address? HomeAddress { get; set; }

        public virtual IList<Order>? Orders { get; set; }
    }
}
