using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsNS
{
    public class Address
    {
        public string City { get; set; }

        public string Street { get; set; }
    }

    public class CnAddress : Address
    {
        public string PostCode { get; set; }
    }

    public class UsAddress : Address
    {
        public string ZipCode { get; set; }
    }
}
