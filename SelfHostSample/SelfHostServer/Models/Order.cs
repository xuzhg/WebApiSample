using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostServer.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public string OrderName { get; set; }

        public IDictionary<string, object> Properties { get; set; }
    }
}
