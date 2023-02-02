﻿using GenericControllerSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Routing.Template;
using System.IO;

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

                foreach (var action in controller.Actions)
                {
                    string httpMethods = "Post";
                    bool withKey = action.Parameters.Any(p => p.Name == "key");

                    if (action.ActionName == "Get")
                    {
                        httpMethods = "get";
                    }
                    else if (action.ActionName == "Patch")
                    {
                        httpMethods = "Patch";
                    }
                    else if (action.ActionName == "Delete")
                    {
                        httpMethods = "Delete";
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
    }
}
