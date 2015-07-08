using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace ODataAttributeRoutingTest
{
    public  class MyController : ODataController
    {
        [ODataRoute("Restaurants({pRestaurantId})/ResDishes/$ref")]
        [HttpDelete]
        public IHttpActionResult RemoveDish([FromODataUri] int pRestaurantId)
        {
            return Ok("OK");
        }

        [ODataRoute("Restaurants({pRestaurantId})/ResDishes({pDishId})/$ref")]
        [HttpDelete]
        public IHttpActionResult RemoveDishWithId([FromODataUri] int pRestaurantId, [FromODataUri]int pDishId)
        {
            string a = "Restaurants(" + pRestaurantId + ")/ResDishes(" + pDishId + ")";
            return Ok(a);
        }
    }
}
