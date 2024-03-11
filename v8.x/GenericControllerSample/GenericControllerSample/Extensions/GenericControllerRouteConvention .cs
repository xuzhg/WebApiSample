using GenericControllerSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.OData.Edm;
using System.IO;

namespace GenericControllerSample.Extensions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class GenericControllerRouteConvention : Attribute, IControllerModelConvention
    {
        private readonly string _prefix = "api/odata";

        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerName.StartsWith("CustomersController"))
            {
                // skip the 'CustomerController<Customer>' for basic CRUD.
                return;
            }

            if (controller.ControllerType.IsGenericType)
            {
                var model = ODataBuilder.GetEdmModel();

                Type genericType = controller.ControllerType.GenericTypeArguments[0];

                var name = genericType.Name;

                controller.ControllerName = name + "Controller";

                foreach (var action in controller.Actions)
                {
                    string httpMethods = "Post";
                    bool withKey = action.Parameters.Any(p => p.Name == "key");

                    if (action.ActionName == "Get")
                    {
                        httpMethods = "get";
                    }
                    else if (action.ActionName == "Post")
                    {
                        httpMethods = "Post";
                    }
                    else if (action.ActionName == "Patch")
                    {
                        httpMethods = "Patch";
                    }
                    else if (action.ActionName == "Delete")
                    {
                        httpMethods = "Delete";
                    }
                    else if (action.ActionName == "CreateLink")
                    {
                        HandleCreateLink(action, _prefix, model);
                        continue;
                    }
                    else
                    {
                        continue;
                    }

                    ODataPathTemplate path;
                    if (withKey)
                    {
                        path = new ODataPathTemplate(new GenericODataTemplateWithKey(name));
                    }
                    else
                    {
                        path = new ODataPathTemplate(new GenericODataTemplate(name));
                    }

                    action.AddSelector(httpMethods, _prefix, model, path);
                }
            }
        }

        private void HandleCreateLink(ActionModel actionModel, string prefix, IEdmModel model)
        {
            var entitySetClientRequest = model.EntityContainer.FindEntitySet("Customers");

            // Without provide the entityset, it creates the more generic endpoints.
            var path = new ODataPathTemplate(new GenericNavigationPropertyLinkTemplate());

            // With the entityset, it creates the more concrete endpoints.
          //  var path = new ODataPathTemplate(new GenericNavigationPropertyLinkTemplate(entitySetClientRequest));
            actionModel.AddSelector("Post,Put", prefix, model, path);
        }
    }
}

