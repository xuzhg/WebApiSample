using FunctionTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.OData.Edm;
using System.Data.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FunctionTest.Extensions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ODataFunctionAttribute : Attribute, IActionModelConvention
    {
        public ODataFunctionAttribute(string prefix)
        {
            Prefix = prefix ?? string.Empty;
        }

        public string Prefix { get; }

        public void Apply(ActionModel action)
        {
            var hasAttributeRouteModels = action.Selectors
                    .Any(selector => selector.AttributeRouteModel != null);
            if (hasAttributeRouteModels)
            {
                return;
            }

            IEdmModel model = EdmModelBuilder.GetEdmModel();
            var function = model.SchemaElements.OfType<IEdmFunction>().FirstOrDefault(f => f.Name == action.ActionName);

            ODataPathTemplate path = new ODataPathTemplate(new MyFunctionSegmentTemplate(function));
            action.AddSelector("get", Prefix, model, path);
        }
    }
}
