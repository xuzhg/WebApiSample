
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace GenericControllerSample.Extensions
{
    // You can make it as generic type.
    public class GenericODataTemplateWithKey : ODataSegmentTemplate
    {
        public GenericODataTemplateWithKey(string typeName)
        {
            TemplateName = typeName;
        }

        public string TemplateName { get; }

        public override IEnumerable<string> GetTemplates(ODataRouteOptions options)
        {
            yield return $"/{TemplateName}/{{key}}";
        }

        public override bool TryTranslate(ODataTemplateTranslateContext context)
        {
            // Simply use the type name to match the entity set.
            var entitySet = context.Model.EntityContainer.EntitySets()
                .FirstOrDefault(e => string.Equals(e.EntityType().Name, TemplateName, StringComparison.OrdinalIgnoreCase));

            if (entitySet != null)
            {
                EntitySetSegment segment = new EntitySetSegment(entitySet);
                context.Segments.Add(segment);

                context.RouteValues.TryGetValue("key", out object value);
                var entityset = segment.EntitySet;
                var key = segment.EntitySet.EntityType().DeclaredKey.First();
                KeySegment keySegment = new KeySegment(new Dictionary<string, object> { { key.Name, value } }, entityset.EntityType(), entityset);
                context.Segments.Add(keySegment);

                return true;
            }

            return false;
        }
    }
}
