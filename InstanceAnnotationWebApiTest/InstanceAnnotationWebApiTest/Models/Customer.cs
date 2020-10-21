using Microsoft.AspNet.OData.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstanceAnnotationWebApiTest.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Address HomeAddress { get; set; }

        public IDictionary<string, object> Dynamics { get; set; }

        public IODataInstanceAnnotationContainer Container { get; set; }
    }
}
