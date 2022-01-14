using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.OData;
using System.Web.OData.Formatter.Serialization;

namespace AspNetClassicOData.Extensions
{
    public class ShowHiddenResourceSerializer : ODataResourceSerializer
    {
        public ShowHiddenResourceSerializer(ODataSerializerProvider provider)
            : base(provider)
        { }

        public override SelectExpandNode CreateSelectExpandNode(ResourceContext resourceContext)
        {
            SelectExpandNode selectExpandNode = base.CreateSelectExpandNode(resourceContext);

            // It's hacky, you should find other ways to make sure the hidden/show logic is for the correct type
            if (!resourceContext.StructuredType.FullTypeName().EndsWith(".Customer"))
            {
                return selectExpandNode;
            }

            var isPriviledged = resourceContext.Request.Headers.Contains("Priviledged");
            if (!isPriviledged)
            {
                return selectExpandNode;
            }

            // primitive, enum or collection of them

            IList<IEdmStructuralProperty> removedProperties = new List<IEdmStructuralProperty>();
            foreach (var property in selectExpandNode.SelectedStructuralProperties)
            {
                if (HasShowHiddenAnnotation(resourceContext.EdmModel, property))
                {
                    removedProperties.Add(property);
                }
            }

            foreach (var property in removedProperties)
            {
                selectExpandNode.SelectedStructuralProperties.Remove(property);
            }

            // complex or collection of complex
            removedProperties.Clear();
            foreach (var property in selectExpandNode.SelectedComplexProperties)
            {
                if (HasShowHiddenAnnotation(resourceContext.EdmModel, property))
                {
                    removedProperties.Add(property);
                }
            }

            foreach (var property in removedProperties)
            {
                selectExpandNode.SelectedComplexProperties.Remove(property);
            }

            // navigation or collection of navigation
            //IList<IEdmNavigationProperty> removedNavs = new List<IEdmNavigationProperty>();
            //foreach (var navProperty in selectExpandNode.ExpandedNavigationProperties)
            //{
            //   // ....
            //}

            //foreach (var property in removedNavs)
            //{
            //    selectExpandNode.ExpandedNavigationProperties.Remove(property);
            //}

            return selectExpandNode;
        }

        //public override ODataProperty CreateStructuralProperty(IEdmStructuralProperty structuralProperty, ResourceContext resourceContext)
        //{
        //    if (HasShowHiddenAnnotation(resourceContext.EdmModel, structuralProperty))
        //    {
        //        var isPriviledged = resourceContext.Request.Headers.Contains("Priviledged");
        //        if (isPriviledged)
        //        {
        //            return null; // skip it.
        //        }
        //    }

        //    return base.CreateStructuralProperty(structuralProperty, resourceContext);
        //}

        private bool HasShowHiddenAnnotation(IEdmModel model, IEdmProperty property)
        {
            IEdmTerm term = model.FindTerm("NS.IsShowHidden");
            IEdmVocabularyAnnotation ann = model.FindVocabularyAnnotations(property).FirstOrDefault();
            if (ann == null)
            {
                return false;
            }

            var boolConstant = ann.Value as EdmBooleanConstant;
            if (boolConstant == null)
            {
                return false;
            }

            return boolConstant.Value;
        }
    }
}