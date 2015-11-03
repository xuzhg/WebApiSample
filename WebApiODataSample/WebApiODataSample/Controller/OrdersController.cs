using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using EdmModelLib;

namespace WebApiODataSample.Controller
{
    public class OrdersController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(DataSource.Orders);
        }

        [EnableQuery]
        public IHttpActionResult Get(int key)
        {
            return Ok(DataSource.Orders.FirstOrDefault(o => o.Id == key));
        }

        public IHttpActionResult Post(Order order)
        {
            int max = DataSource.Orders.Max(o => o.Id);
            order.Id = max + 1;
            DataSource.Orders.Add(order);
            return Ok(order);
        }
    }
}
