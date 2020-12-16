using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntitySetWithSpaceRouting
{
    public class MyEntitySetRoutingConvention : IODataRoutingConvention
    {
        public virtual IEnumerable<ControllerActionDescriptor> SelectAction(RouteContext routeContext)
        {
            Microsoft.AspNet.OData.Routing.ODataPath odataPath = routeContext.HttpContext.ODataFeature().Path;
            if (odataPath == null)
            {
                return null;
            }

            string controller = null;
            string action = null;
            if (odataPath.PathTemplate == "~/entityset" &&
                routeContext.HttpContext.Request.Method.Equals("get", StringComparison.OrdinalIgnoreCase))
            {
                EntitySetSegment entitySetSegment = (EntitySetSegment)odataPath.Segments[0];
                if (entitySetSegment.EntitySet.Name == "Order Details")
                {
                    controller = "OrderDetails";
                    action = "Get";
                }
            }

            if (controller != null && action != null)
            {
                IActionDescriptorCollectionProvider actionCollectionProvider =
                    routeContext.HttpContext.RequestServices.GetRequiredService<IActionDescriptorCollectionProvider>();

                return actionCollectionProvider
                    .ActionDescriptors.Items.OfType<ControllerActionDescriptor>()
                    .Where(c => c.ControllerName == controller && c.ActionName == action);

            }

            return null;
        }
    }

    public class MyEntitySetRoutingConvention1 : EntitySetRoutingConvention
    {
        public override string SelectAction(RouteContext routeContext, SelectControllerResult controllerResult, IEnumerable<ControllerActionDescriptor> actionDescriptors)
        {
            return base.SelectAction(routeContext, controllerResult, actionDescriptors);
        }
    }
}
