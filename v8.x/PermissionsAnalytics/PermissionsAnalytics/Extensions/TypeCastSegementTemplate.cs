using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace PermissionsAnalytics.Extensions
{
    public class TypeCastSegementTemplate : ODataSegmentTemplate
    {
        public override IEnumerable<string> GetTemplates(ODataRouteOptions options)
        {
            yield return "{entityType}";
        }

        public override bool TryTranslate(ODataTemplateTranslateContext context)
        {
            if (!context.RouteValues.TryGetValue("entityType", out object entityType))
            {
                return false;
            }

            string entityTypeName = entityType as string;

            IEdmEntityType edmEntityType = context.Model.FindType(entityTypeName) as IEdmEntityType;
            if (edmEntityType == null)
            {
                return false;
            }

            // Check the previous one
            NavigationPropertySegment navigationSegment = context.Segments.LastOrDefault() as NavigationPropertySegment;
            if (navigationSegment == null)
            {
                return false;
            }

            IEdmType actualType;
            IEdmTypeReference expectedType = navigationSegment.NavigationProperty.Type;
            bool isNullable = expectedType.IsNullable;
            if (expectedType.IsCollection())
            {
                actualType = new EdmCollectionType(new EdmEntityTypeReference(edmEntityType, isNullable));
            }
            else
            {
                actualType =edmEntityType;
            }

            TypeSegment typeSegment = new TypeSegment(actualType, expectedType.Definition, navigationSegment.NavigationSource);
            context.Segments.Add(typeSegment);
            return true;
        }
    }
}
