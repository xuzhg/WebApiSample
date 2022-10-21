using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData;
using UpdateNestedNavigationPropertySample.Models;

namespace UpdateNestedNavigationPropertySample.Controllers
{
    [ApiController]
    [Route("odata/v{version}")]
    public class ClassesController : ODataController
    {
        private readonly ILogger<ClassesController> _logger;

        public ClassesController(ILogger<ClassesController> logger)
        {
            _logger = logger;
        }

        [HttpPatch("classes/{classId}/assignmentSettings")]
        public IActionResult PatchGradingCategories(int classId, Delta<EducationSettings> delta)
        {
            var changedProperties = delta.GetChangedPropertyNames();
            // verify there's two properties

            delta.TryGetPropertyValue("GradingCategories", out object gradingCategories);

            return Ok(gradingCategories as IList<EducationGradingCategory>);
        }
    }
}