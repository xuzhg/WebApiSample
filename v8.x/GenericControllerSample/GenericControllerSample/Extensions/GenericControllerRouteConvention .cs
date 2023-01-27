using GenericControllerSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Routing.Template;

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
                var model = ODataBuilder.GetEdmModel();

                Type genericType = controller.ControllerType.GenericTypeArguments[0];

                var name = genericType.Name;

                controller.ControllerName = name + "Controller";

                //SelectorModel selectorModel = new SelectorModel
                //{
                //    AttributeRouteModel = new AttributeRouteModel(new RouteAttribute($"{_prefix}/{name}")),
                //};

                foreach (var action in controller.Actions)
                {
                    bool withKey = true;
                    var parameter = action.Parameters.FirstOrDefault(p => p.Name == "key");
                    if (parameter != null)
                    {
                        IRouteTemplateProvider routeProvider = null;
                        if (action.ActionName == "Get")
                        {
                            routeProvider = new HttpGetAttribute($"{_prefix}/{name}/{{key}}"); // only add the key as segment.
                        }
                        else if (action.ActionName == "Patch")
                        {
                            routeProvider = new HttpPatchAttribute($"{_prefix}/{name}/{{key}}"); // only add the key as segment.
                        }
                        else if (action.ActionName == "Delete")
                        {
                            routeProvider = new HttpDeleteAttribute($"{_prefix}/{name}/{{key}}"); // only add the key as segment.
                        }
                        else
                        {
                            routeProvider = new RouteAttribute($"{_prefix}/{name}"); // without key
                            withKey = false;
                        }

                        // since we have all [HttpVerb] on each action, it's safe to find a selectorModel with  AttributeRouteModel == null.
                        SelectorModel actionSelectorModel = action.Selectors.FirstOrDefault(s => s.AttributeRouteModel == null);
                        if (actionSelectorModel != null)
                        {
                            actionSelectorModel.AttributeRouteModel = new AttributeRouteModel(routeProvider);

                            ODataPathTemplate path;
                            if (withKey)
                            {
                                path = new ODataPathTemplate(new GenericODataTemplateWithKey(name));
                            }
                            else
                            {
                                path = new ODataPathTemplate(new GenericODataTemplate(name));
                            }

                            ODataRoutingMetadata odataMetadata = new ODataRoutingMetadata(_prefix, model, path);
                            actionSelectorModel.EndpointMetadata.Add(odataMetadata);
                        }
                    }
                }
            }
        }
    }
}
