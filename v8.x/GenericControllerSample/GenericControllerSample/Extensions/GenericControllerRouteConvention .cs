using GenericControllerSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.OData.ModelBuilder;
using System.IO;
using System.Reflection;

namespace GenericControllerSample.Extensions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class GenericControllerRouteConvention : Attribute, IControllerModelConvention
    {
        private readonly string _prefix = "api/odata";

        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.IsGenericType)
            {
                Type genericType = controller.ControllerType.GenericTypeArguments[0];

                var name = genericType.Name;

                controller.ControllerName = name + "Controller";

                SelectorModel selectorModel = new SelectorModel
                {
                    AttributeRouteModel = new AttributeRouteModel(new RouteAttribute($"{_prefix}/{name}")),
                };

                controller.Selectors.Add(selectorModel);

                var model = ODataBuilder.GetEdmModel();
                ODataPathTemplate path = new ODataPathTemplate(new GenericODataTemplate(name));

                ODataRoutingMetadata odataMetadata = new ODataRoutingMetadata(_prefix, model, path);
                selectorModel.EndpointMetadata.Add(odataMetadata);
            }
        }
    }
}
