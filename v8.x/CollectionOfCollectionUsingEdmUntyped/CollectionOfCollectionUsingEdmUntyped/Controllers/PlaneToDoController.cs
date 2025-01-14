using CollectionOfCollectionUsingEdmUntyped.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using System.Drawing;

namespace CollectionOfCollectionUsingEdmUntyped.Controllers
{
    public class PlaneToDoController : Controller
    {
        [HttpGet("odata/Planes({id})")]
        [ODataAttributeRouting]
        [EnableQuery]
        public IActionResult GetPlanes(Guid id)
        {
            return Ok(
                new PlaneDto
                {
                    Id = id,
                    Normal = [-0.99999999974639, 8.22496291971287E-06, 2.09659328342425E-05],
                    Point = [-22.4877607468324, 0.000184960998336979, 0.000471476881530173],
                    Points = [-22.4822136189295, -13.6948240459868, 269.9512],
                    Contours = new int[][]
                    {
                        new int[] { 1, 2, 3 },
                        new int[] { 2, 3, 4 }
                    }
                });
        }
    }
}
