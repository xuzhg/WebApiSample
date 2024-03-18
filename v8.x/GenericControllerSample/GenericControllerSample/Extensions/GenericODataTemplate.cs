
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace GenericControllerSample.Extensions
{
    // You can make it as generic type.
    public class GenericODataTemplate : ODataSegmentTemplate
    {
        public GenericODataTemplate(string typeName)
        {
            TemplateName = typeName;
        }

        public string TemplateName { get; }

        public override IEnumerable<string> GetTemplates(ODataRouteOptions options)
        {
            yield return $"/{TemplateName}";
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
                return true;
            }

            return false;
        }
    }

    public class GenericNavigationPropertyLinkTemplate : ODataSegmentTemplate
    {
        private IEdmEntitySet edmEntitySet;

        public GenericNavigationPropertyLinkTemplate()
        {
        }


        public GenericNavigationPropertyLinkTemplate(IEdmEntitySet entitySet)
        {
            edmEntitySet = entitySet;
        }

        public override IEnumerable<string> GetTemplates(ODataRouteOptions options)
        {
            if (edmEntitySet != null)
            {
                yield return $"{edmEntitySet.Name}({{key}})/{{navigationProperty}}/$ref";
                yield return $"{edmEntitySet.Name}/{{key}}/{{navigationProperty}}/$ref";
            }
            else
            {
                yield return "/{entityset}({key})/{navigationProperty}/$ref";
                yield return "/{entityset}/{key}/{navigationProperty}/$ref";
            }
        }

        public override bool TryTranslate(ODataTemplateTranslateContext context)
        {
            IEdmModel model = context.Model;
            EntitySetSegment entitySetSegment;

            IEdmEntitySet entitySet = edmEntitySet;
            if (edmEntitySet != null)
            {
                entitySetSegment = new EntitySetSegment(edmEntitySet);
            }
            else
            {
                context.RouteValues.TryGetValue("entityset", out var entitysetObj);
                string entitySetName = entitysetObj as string;

                entitySet = model.EntityContainer.EntitySets().FirstOrDefault(e => e.Name.Equals(entitySetName, StringComparison.OrdinalIgnoreCase));
                // TODO: add error handle
                
                entitySetSegment = new EntitySetSegment(entitySet);
            }

            context.RouteValues.TryGetValue("key", out var key);

            int keyValue = int.Parse(key.ToString());

            KeySegment keySegment = new KeySegment(new[] { new KeyValuePair<string, object>("Id", keyValue) }, entitySet.EntityType(), edmEntitySet);

            context.RouteValues.TryGetValue("navigationProperty", out var navigationProperty);

            var edmNavProperty = entitySet.EntityType().NavigationProperties().FirstOrDefault(c => c.Name.Equals(navigationProperty.ToString(), StringComparison.OrdinalIgnoreCase));

            var targetNav = entitySet.FindNavigationTarget(edmNavProperty);

            NavigationPropertyLinkSegment navSegment = new NavigationPropertyLinkSegment(edmNavProperty, targetNav);

            context.Segments.Add(entitySetSegment);

            context.Segments.Add(keySegment);

            context.Segments.Add(navSegment);

            return true;

        }

    }
}
