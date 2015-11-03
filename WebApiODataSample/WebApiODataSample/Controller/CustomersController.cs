using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using EdmModelLib;
using Microsoft.OData.Core.UriParser;
using Microsoft.OData.Edm;

namespace WebApiODataSample.Controller
{
    public class CustomersController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(DataSource.Customers);
        }

        [EnableQuery]
        public IHttpActionResult Get(int key)
        {
            Customer customer = DataSource.Customers.FirstOrDefault(c => c.Id == key);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        public IHttpActionResult Post(Customer customer)
        {
            int max = DataSource.Customers.Max(c => c.Id);
            customer.Id = max + 1;
            DataSource.Customers.Add(customer);
            return Ok(customer);
        }

        public IHttpActionResult Patch(int key, Delta<Customer> delta)
        {
            Customer customer = DataSource.Customers.FirstOrDefault(c => c.Id == key);
            if (customer == null)
            {
                return NotFound();
            }

            delta.Patch(customer);

            return Updated(customer);
        }

        [AcceptVerbs("POST", "PUT")]
        public IHttpActionResult CreateRef([FromODataUri] int key, string navigationProperty, [FromBody] Uri reference)
        {
            int relatedKey = GetKeyValue<int>(Request, reference);
            Customer customer = DataSource.Customers.First(x => x.Id == key);
            Order order = DataSource.Orders.First(o => o.Id == relatedKey);

            if (navigationProperty != "Orders" || customer == null || order == null)
            {
                return BadRequest();
            }

            if (customer.Orders == null)
            {
                customer.Orders = new List<Order>();
            }
            customer.Orders.Add(order);
            return StatusCode(HttpStatusCode.NoContent);
        }

        public static TKey GetKeyValue<TKey>(HttpRequestMessage request, Uri uri)
        {
            //get the odata path Ex: ~/entityset/key/$links/navigation
            var odataPath = CreateODataPath(request, uri);
            var keySegment = odataPath.Segments.OfType<KeyValuePathSegment>().FirstOrDefault();
            if (keySegment == null)
            {
                throw new InvalidOperationException("The link does not contain a key.");
            }

            var value = ODataUriUtils.ConvertFromUriLiteral(keySegment.Value, Microsoft.OData.Core.ODataVersion.V4);
            return (TKey)value;
        }

        public static ODataPath CreateODataPath(HttpRequestMessage request, Uri uri)
        {
            var newRequest = new HttpRequestMessage(HttpMethod.Get, uri);
            var route = request.GetRouteData().Route;

            var newRoute = new HttpRoute(
                route.RouteTemplate,
                new HttpRouteValueDictionary(route.Defaults),
                new HttpRouteValueDictionary(route.Constraints),
                new HttpRouteValueDictionary(route.DataTokens),
                route.Handler);
            var routeData = newRoute.GetRouteData(request.GetConfiguration().VirtualPathRoot, newRequest);
            if (routeData == null)
            {
                throw new InvalidOperationException("The link is not a valid odata link.");
            }

            return newRequest.ODataProperties().Path;
        }
    }
}
