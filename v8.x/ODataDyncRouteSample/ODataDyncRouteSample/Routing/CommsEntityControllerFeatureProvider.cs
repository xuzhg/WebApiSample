using System.Reflection;

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

using ODataDyncRouteSample.Controllers;
using ODataDyncRouteSample.Models;

namespace ODataDyncRouteSample.Routing
{
    public class CommsEntityControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var controllerType = typeof(CommsEntityController<>)
                .MakeGenericType(typeof(AzureUpdate))
                .GetTypeInfo();

            feature.Controllers.Add(controllerType);

            controllerType = typeof(CommsEntityController<>)
                .MakeGenericType(typeof(M365Roadmap))
                .GetTypeInfo();

            feature.Controllers.Add(controllerType);

            feature.Controllers.Add(typeof(SingleEntityController).GetTypeInfo());
        }
    }
}

