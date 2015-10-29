using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ODataWithEF6Sample.Models
{
    public class ODataWithEf6SampleContext : DbContext
    {
        public IDbSet<Employee> Employees { get; set; }

        public IDbSet<Message> Messages { get; set; }

        public IDbSet<Address> Addresses { get; set; }
    }
}