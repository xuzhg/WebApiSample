using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataWithEF6Sample.Models
{
    public class Address
    {
        public int Id { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }
    }
}