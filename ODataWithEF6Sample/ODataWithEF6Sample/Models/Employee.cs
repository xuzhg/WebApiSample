using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataWithEF6Sample.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AddressId { get; set; }

        public Address Address { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}