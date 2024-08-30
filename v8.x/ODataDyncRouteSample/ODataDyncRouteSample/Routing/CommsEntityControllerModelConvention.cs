using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.OData.Edm;
using ODataDyncRouteSample.Controllers;
using ODataDyncRouteSample.Models;

namespace ODataDyncRouteSample.Routing
{
    public class CommsEntityControllerModelConvention : IControllerModelConvention
    {
        private IEdmModel _model;
        public CommsEntityControllerModelConvention(IEdmModel model)
        {
            _model = model;
        }

        public void Apply(ControllerModel controller)
        {
            if (!controller.ControllerType.IsGenericType || controller.ControllerType.GetGenericTypeDefinition() != typeof(CommsEntityController<>))
            {
                return;
            }

            Type entityType = controller.ControllerType.GenericTypeArguments[0];

            if (entityType == typeof(AzureUpdate))
            {
                SelectorModel selector = new SelectorModel
                {
                    AttributeRouteModel = new AttributeRouteModel
                    {
                        Template = "api/v2/Azure"
                    },
                };

                IEdmEntitySet azureEntitySet = _model.FindDeclaredEntitySet("Azure");
                ODataPathTemplate template = new ODataPathTemplate(new EntitySetSegmentTemplate(azureEntitySet));
                selector.EndpointMetadata.Add(new ODataRoutingMetadata("api/v2", _model, template));

                controller.Selectors.Add(selector);
            }
            else if (entityType == typeof(M365Roadmap))
            {
                SelectorModel selector = new SelectorModel
                {
                    AttributeRouteModel = new AttributeRouteModel
                    {
                        Template = "api/v2/M365"
                    }
                };

                IEdmEntitySet azureEntitySet = _model.FindDeclaredEntitySet("M365");
                ODataPathTemplate template = new ODataPathTemplate(new EntitySetSegmentTemplate(azureEntitySet));
                selector.EndpointMetadata.Add(new ODataRoutingMetadata("api/v2", _model, template));

                controller.Selectors.Add(selector);
            }
        }
    }
}

