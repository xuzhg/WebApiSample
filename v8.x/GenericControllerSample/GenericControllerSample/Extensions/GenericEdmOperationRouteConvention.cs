using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.OData.Edm;
using System;
using static System.Collections.Specialized.BitVector32;
using System.Reflection;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Routing;

namespace GenericControllerSample.Extensions
{
    public class GenericEdmOperationRouteConvention : IActionModelConvention
    {
        private readonly string _prefix = "api/odata";
        private IDictionary<IEdmType, IEdmEntitySet> _navigationSources;

        public GenericEdmOperationRouteConvention(IEdmModel model)
        {
            Model = model;

            _navigationSources = new Dictionary<IEdmType, IEdmEntitySet>();
            foreach (var s in model.EntityContainer.Elements.OfType<IEdmEntitySet>())
            {
                _navigationSources[s.EntityType()] = s;
            }
        }

        public IEdmModel Model { get; }

        public void Apply(ActionModel actionModel)
        {
            if (!actionModel.Controller.ControllerType.IsGenericType ||
                !actionModel.Controller.ControllerName.StartsWith("EngagementEntitySetController"))
            {
                return;
            }

            if (actionModel.Selectors
                   .Any(selector => selector.AttributeRouteModel != null))
            {
                return;
            }

            string actionName = actionModel.ActionName;
            if (actionName.EndsWith("Async"))
            {
                actionName = actionName.Substring(0, actionName.Length - 5);
            }

            IEdmOperation operation = FindOperation(actionModel, actionName);
            if (operation == null)
            {
                return;
            }

            // You can add more code to verify "[HttpGet]" for function, and "[HttpPost]" for action.
            // And the parameter should be same.
            ODataPathTemplate path = BuildTemplate(operation, out bool isFunction);
            if (path == null)
            {
                return;
            }

            string httpMethods = "post";
            if (isFunction)
            {
                httpMethods = "get";
            }

            ODataRouteOptions options = new ODataRouteOptions
            {
                EnableKeyInParenthesis = false,
                EnableQualifiedOperationCall = false,
            };

            actionModel.Selectors.Clear();
            actionModel.AddSelector(httpMethods, _prefix, Model, path, options);
        }

        private IEdmOperation FindOperation(ActionModel actionModel, string name)
        {
            // Here, for simplicity, I assume there's no function/action overloads.
            // If want to support function/action overloads, we need to more codes to find the best matched operation

            return Model.SchemaElements.OfType<IEdmOperation>().FirstOrDefault(c => c.Name == name);
        }

        private ODataPathTemplate BuildTemplate(IEdmOperation operation, out bool isFunction)
        {
            isFunction = true;
            if (!operation.IsBound)
            {
                return null;
            }

            bool bindingToCollection = false;
            var bindingParameter = operation.Parameters.First();
            IEdmType bindingType = bindingParameter.Type.Definition;
            if (bindingParameter.Type.IsCollection())
            {
                bindingToCollection = true;
                bindingType = bindingParameter.Type.AsCollection().ElementType().Definition;
            }

            if (!_navigationSources.TryGetValue(bindingType, out IEdmEntitySet ns))
            {
                return null;
            }

            ODataPathTemplate path = new ODataPathTemplate();
            EntitySetSegmentTemplate entitySet = new EntitySetSegmentTemplate(ns);
            path.Add(entitySet);

            IEdmEntityType entityType = ns.EntityType();
            if (!bindingToCollection)
            {
                KeySegmentTemplate key = new KeySegmentTemplate(GetKeys(entityType), entityType, ns);
                path.Add(key);
            }

            if (operation is IEdmFunction edmFunction)
            {
                FunctionSegmentTemplate func = new FunctionSegmentTemplate(edmFunction, null);
                path.Add(func);
                isFunction = true;
            }
            else
            {
                IEdmAction edmAction = (IEdmAction)operation;
                ActionSegmentTemplate act = new ActionSegmentTemplate(edmAction, null);
                path.Add(act);
                isFunction = false;
            }

            return path;
        }

        private static IDictionary<string, string> GetKeys(IEdmEntityType entityType)
        {
            IDictionary<string, string> keys = new Dictionary<string, string>();
            foreach (var key in entityType.Key())
            {
                keys[key.Name] = $"{{{key.Name}}}";
            }

            return keys;
        }
    }
}
