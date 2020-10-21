using System.Collections.Generic;
using System.Linq;
using InstanceAnnotationWebApiTest.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OData;

namespace InstanceAnnotationWebApiTest.Controllers
{
    public class CustomersController : ControllerBase
    {
        private static readonly Customer[] _customers;

        static CustomersController()
        {
            ODataInstanceAnnotationContainer container = new ODataInstanceAnnotationContainer();

            container.AddResourceAnnotation("NS.ResourceAddress", new Address
            {
                Street = "156th AVE",
                City = "Redmond"
            });

            container.AddResourceAnnotation("NS.ResourceName", "Sam");
            container.AddPropertyAnnotation("Name", "NS.DisplayName", "==UI==");

            _customers = new Customer[]
            {
                new Customer
                {
                    Id = 1,
                    Name = "Freezing",
                    Dynamics = new Dictionary<string, object>
                    {
                        { "StringDynamicProperty", "abc" }
                    },
                    Container = container
                },
                new Customer
                {
                    Id = 2,
                    Name = "Hot",
                    Dynamics = new Dictionary<string, object>
                    {
                        { "IntDynamicProperty", 123 }
                    },
                    Container = new ODataInstanceAnnotationContainer()
                }
            };
        }

        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return _customers;
        }

        [HttpGet]
        public Customer Get(int key)
        {
            return _customers.FirstOrDefault(k => k.Id == key);
        }
    }
}
