using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using Microsoft.OData.Edm;
using SelfHostServer.Models;

namespace SelfHostServer.Controllers
{
    public class CustomersController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(DataSource.Customers);
        }

        public IHttpActionResult Get(int key)
        {
            Customer customer = DataSource.Customers.FirstOrDefault(e => e.CustomerId == key);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [EnableQuery]
        public IHttpActionResult Post(Customer customer)
        {
            int max = DataSource.Customers.Max(e => e.CustomerId);
            customer.CustomerId = max + 1;
            DataSource.Customers.Add(customer);
            return Created(customer);
        }

        public IHttpActionResult PutToCategory(int key, [FromBody]Category category)
        {
            Customer customer = DataSource.Customers.FirstOrDefault(e => e.CustomerId == key);
            if (customer == null)
            {
                return NotFound();
            }

            customer.Category = category;
            return Ok(category);
        }

        [AcceptVerbs("POST", "PUT")]
        public IHttpActionResult CreateRefToCategory(int key, string navigationProperty, [FromBody] Uri reference)
        {
            Customer customer = DataSource.Customers.FirstOrDefault(e => e.CustomerId == key);
            if (customer == null)
            {
                return NotFound();
            }

            if (navigationProperty != "Category")
            {
                return BadRequest();
            }

            ODataRoute route = Request.GetConfiguration().Routes.First(e => e is ODataRoute) as ODataRoute;
            // just for test:
            string serviceRoot = "http://localhost:12345/odata/";
            string odataPath = reference.AbsoluteUri.Substring(serviceRoot.Length);

            IEdmModel model = Request.ODataProperties().Model;
            ODataPath path = Request.ODataProperties().PathHandler.Parse(model, serviceRoot, odataPath);
            if (path.PathTemplate == "~/entityset/key")
            {
                KeyValuePathSegment newKey = path.Segments.Last() as KeyValuePathSegment;
                int categoryId = int.Parse(newKey.Value);

                Category category = DataSource.Categories.First(e => e.CategoryId == categoryId);
                if (category == null)
                {
                    category = new Category
                    {
                        CategoryId = categoryId
                    };
                    DataSource.Categories.Add(category);
                }

                customer.Category = category;
                return Ok(category);
            }

            return BadRequest();
        }

        [HttpGet]
        [EnableQuery]
        public SingleResult<Customer> RefreshCustomer([FromODataUri] int key)
        {
            Customer customer = DataSource.Customers.FirstOrDefault(e => e.CustomerId == key);
            if (customer == null)
            {
                return new SingleResult<Customer>(Enumerable.Empty<Customer>().AsQueryable());
            }

            return new SingleResult<Customer>(new[] {customer}.AsQueryable());
        }

        [HttpGet]
        [EnableQuery]
        public IHttpActionResult RefreshCustomer2([FromODataUri] int key)
        {
            Customer customer = DataSource.Customers.FirstOrDefault(e => e.CustomerId == key);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }
    }
}
