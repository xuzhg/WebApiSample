using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Question78956264AllowDeepNavigation.Controllers
{
    // Use the OData attribute rules to build the endpoint
    // in attribute routing, the controller name and action name does NOT matter.
    // Only the route template on the action matters.
    public class Services2Controller : ODataController
    {
        [HttpGet("attribute/Services({serviceId})/ServiceArticle({serviceArticleId})")]
        public IActionResult GetServiceArticle(int serviceId, int serviceArticleId)
        {
            return Ok($"You are calling the attribute endpoint using ServiceId={serviceId}, ServiceArticleId={serviceArticleId}." +
                $"By attribute, only the template in [HttpGet] is in consideration");
        }
    }
}
