using AspNetClassicOData.Models;
using Microsoft.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using System.Web.WebSockets;

namespace AspNetClassicOData.Controllers
{
    public class CustomersController : ODataController
    {
        private static IList<Customer> customers = new List<Customer>
        {
            new Customer
            {
                Id = 1, Name = "Sam", Age = 14, Nums = new List<int> { 2, 5, 6 },
                Emails = new List<string> { "abc@abc.com", "efg@efg.com"},
                HomeAdress = new Address { City = "samCity", Street = "samStreet" },
                NoHiddenAddress = new Address { City = "nohidden-samCity", Street = "nohidden-samStreet" },
                MailAddresses = new List<Address>
                {
                    new Address { City = "samMailCity1", Street = "samMailStreet1" },
                    new Address { City = "samMailCity2", Street = "samMailStreet2" }
                },
                SingleOrder = new Order { Id = 2, Title = "samOrder" },
                Orders = new List<Order>
                {
                    new Order { Id = 3, Title = "samOrders1" },
                    new Order { Id = 4, Title = "samOrders2" }
                }
            },
             new Customer
            {
                Id = 2, Name = "Kerry", Age = 4, Nums = new List<int> { 3, 7, 9 },
                Emails = new List<string> { "111@abc.com", "xyz@xyz.com"},
                HomeAdress = new Address { City = "kerryCity", Street = "kerryStreet" },
                NoHiddenAddress = new Address { City = "nohidden-kerryCity", Street = "nohidden-kerryStreet" },
                MailAddresses = new List<Address>
                {
                    new Address { City = "kerryMailCity1", Street = "kerryMailStreet1" },
                    new Address { City = "kerryMailCity2", Street = "kerryMailStreet2" }
                },
                SingleOrder = new Order { Id = 7, Title = "kerryOrder" },
                Orders = new List<Order>
                {
                    new Order { Id = 8, Title = "kerryOrders1" },
                    new Order { Id = 39, Title = "kerryOrders2" }
                }
            }
        };
        public IHttpActionResult Get()
        {
            return Ok(customers);
        }

        public IHttpActionResult Get(int key)
        {
            return Ok(customers[key]);
        }
    }
}
