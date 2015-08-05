using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using SelfHostServer.Models;

namespace SelfHostServer.Controllers
{
    public class OrdersController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(DataSource.Orders);
        }

        public IHttpActionResult Get(int key)
        {
            Order order = DataSource.Orders.FirstOrDefault(e => e.OrderId == key);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        public IHttpActionResult Post(Order order)
        {
            int orderId = DataSource.Orders.Max(e => e.OrderId);
            order.OrderId = orderId + 1;
            DataSource.Orders.Add(order);
            return Created(order);
        }
    }
}
