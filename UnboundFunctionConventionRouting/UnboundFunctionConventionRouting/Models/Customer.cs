using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnboundFunctionConventionRouting.Models;

namespace UnboundNS
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public CustomerType CustomerType { get; set; }
    }
}
