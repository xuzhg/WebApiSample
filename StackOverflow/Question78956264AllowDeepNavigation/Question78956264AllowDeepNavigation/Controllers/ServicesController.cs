using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Question78956264AllowDeepNavigation.Controllers
{
    // Use the OData convention rules to build the endpoint
    public class ServicesController : ODataController
    {
        [HttpGet]
        [ODataRouteComponent("convention")] // this line is used to make sure only 'convention' component is allowed in convention routing 
        public IActionResult GetServiceArticle(int keyServiceId, int keyServiceArticleId)
        {
            return Ok($"You are calling the convention endpoint using ServiceId={keyServiceId}, ServiceArticleId={keyServiceArticleId}.");
        }
    }
}
