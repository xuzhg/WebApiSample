using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.OData.Edm;

namespace PermissionsAnalytics.Extensions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GenericEntityTypeActionConvention : Attribute, IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            foreach (var selector in action.Selectors)
            {
                var odataMetadata = selector.EndpointMetadata.FirstOrDefault(a => a is IODataRoutingMetadata) as IODataRoutingMetadata;
                if (odataMetadata == null)
                {
                    continue;
                }

                ODataPathTemplate newTemplate;
                if (!TryBuildPathTemplate(odataMetadata.Model, odataMetadata.Template, out newTemplate))
                {
                    continue;
                }

                selector.EndpointMetadata.Remove(odataMetadata);

                ODataRoutingMetadata newMetadata = new ODataRoutingMetadata(odataMetadata.Prefix, odataMetadata.Model, newTemplate)
                {
                    IsConventional = odataMetadata.IsConventional
                };

                selector.EndpointMetadata.Add(newMetadata);
            }
        }

        private static bool TryBuildPathTemplate(IEdmModel model, ODataPathTemplate pathTemplate, out ODataPathTemplate newTemplate)
        {
            newTemplate = null;
            ODataSegmentTemplate lastSegment = pathTemplate.LastOrDefault();
            if (lastSegment == null && lastSegment is not KeySegmentTemplate)
            {
                return false;
            }

            KeySegmentTemplate lastKeySegment = (KeySegmentTemplate)lastSegment;

            // Let's only re-build the key parameter as "entityType"
            if (!lastKeySegment.KeyMappings.Values.Any(c => string.Equals(c, "entityType", StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            IList<ODataSegmentTemplate> segments = new List<ODataSegmentTemplate>();
            foreach (var segment in pathTemplate)
            {
                if (segment != lastKeySegment)
                {
                    segments.Add(segment);
                }
            }

            segments.Add(new TypeCastSegementTemplate());

            newTemplate = new ODataPathTemplate(segments);
            return true;
        }
    }
}
