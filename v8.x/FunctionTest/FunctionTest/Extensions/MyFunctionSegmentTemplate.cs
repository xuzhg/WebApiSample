using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace FunctionTest.Extensions
{
    public class MyFunctionSegmentTemplate : ODataSegmentTemplate
    {
        private string _parameters;
        public MyFunctionSegmentTemplate(IEdmFunction function)
        {
            Function = function;
            _parameters = string.Join(",", function.Parameters.Where(p => p.Type.IsPrimitive()).Select(p => $"{p.Name}={{{p.Name}}}"));
        }

        public IEdmFunction Function { get; }

        public override IEnumerable<string> GetTemplates(ODataRouteOptions options)
        {
            // only create an template as "/MyFunction"
            if (string.IsNullOrEmpty(_parameters))
            {
                yield return $"/{Function.Name}";
            }
            else
            {
                yield return $"/{Function.Name}({_parameters})";
            }
        }

        public override bool TryTranslate(ODataTemplateTranslateContext context)
        {
            var primitiveParameters = Function.Parameters.Where(p => p.Type.IsPrimitive());
            IList<OperationSegmentParameter> parameters = new List<OperationSegmentParameter>();
            foreach (var p in  primitiveParameters)
            {
                context.RouteValues.TryGetValue(p.Name, out object value);
                string parameterValue = value as string;

                object newValue = ODataUriUtils.ConvertFromUriLiteral(parameterValue, ODataVersion.V4, context.Model, p.Type);

                context.UpdatedValues[p.Name] = newValue;
                parameters.Add(new OperationSegmentParameter(p.Name, newValue));
            }

            var bindingType = Function.Parameters.First().Type.AsCollection().ElementType().Definition;
            var entityset = context.Model.EntityContainer.EntitySets().FirstOrDefault(a => a.EntityType() == bindingType);
            context.Segments.Add(new EntitySetSegment(entityset));
            context.Segments.Add(new OperationSegment(Function, parameters, null));
            return true;
        }
    }
}
