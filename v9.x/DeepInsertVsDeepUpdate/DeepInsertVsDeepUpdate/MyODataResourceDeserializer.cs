using Microsoft.AspNetCore.OData.Formatter.Deserialization;
using Microsoft.AspNetCore.OData.Formatter.Wrapper;
using Microsoft.OData.Edm;
using System.Reflection;
using Microsoft.AspNetCore.OData.Edm;
using Microsoft.AspNetCore.OData.Deltas;

namespace DeepInsertVsDeepUpdate
{
    public class MyODataResourceDeserializer : ODataResourceDeserializer
    {
        public MyODataResourceDeserializer(IODataDeserializerProvider deserializerProvider) : base(deserializerProvider)
        {
        }

        public override void ApplyNestedProperty(object resource, ODataNestedResourceInfoWrapper resourceInfoWrapper,
         IEdmStructuredTypeReference structuredType, ODataDeserializerContext readContext)
        {
            IDelta deltaResource = resource as IDelta;
            if (deltaResource != null)
            {
                base.ApplyNestedProperty(resource, resourceInfoWrapper, structuredType, readContext);
                return;
            }

            bool hasNavBinding = false;
            string odataId = null;

            // 1) OData v4.0, it's "Orders@odata.bind", and we get "ODataEntityReferenceLinkWrapper"(s) for that.
            // 2) OData v4.01, it's {"odata.id" ...}, and we get "ODataResource"(s) for that.
            if (resourceInfoWrapper.NestedItems.OfType<ODataEntityReferenceLinkWrapper>().Any())
            {
                // It's from 'nav@odata.bind"
                hasNavBinding = true;
                odataId = resourceInfoWrapper.NestedItems.OfType<ODataEntityReferenceLinkWrapper>().First().EntityReferenceLink.Url.OriginalString;
            }
            else
            {
                ODataResourceWrapper resourceWrapper = resourceInfoWrapper.NestedItems.OfType<ODataResourceWrapper>().FirstOrDefault();
                if (resourceWrapper != null && resourceWrapper.Resource.Id != null)
                {
                    // It's from 'nav': { "odata.id": ... }
                    hasNavBinding = true;
                    odataId = resourceWrapper.Resource.Id.OriginalString;
                }
            }

            if (hasNavBinding)
            {
                // You can do a lot of things here
                // 1) If you can change your C# model class, you can add a property to label it?
                // 2) You can log it into HttReqest.Items for later use?
                //  readContext.Request.HttpContext.Items[resourceInfoWrapper.NestedResourceInfo.Name] = true;
                // ...
                //if (resource is IDelta delta)
                //{
                //    delta.Se
                //}

                PropertyInfo containerPropertyInfo = readContext.Model.GetTypeMapper().GetClrType(readContext.Model, structuredType).GetProperties().FirstOrDefault(x => x.PropertyType == typeof(ODataDeepUpdateMetadataCollection));
                if (containerPropertyInfo != null)
                {
                    ODataDeepUpdateMetadataCollection metadataCollection = containerPropertyInfo.GetValue(resource) as ODataDeepUpdateMetadataCollection;
                    if (metadataCollection == null)
                    {
                        metadataCollection = new ODataDeepUpdateMetadataCollection();
                        containerPropertyInfo.SetValue(resource, metadataCollection);
                    }
                    ODataDeepInsertMetadata metadata = new ODataDeepInsertMetadata
                    {
                        PropertyName = resourceInfoWrapper.NestedResourceInfo.Name,
                        ODataId = odataId
                    };
                    metadataCollection.Add(metadata);
                }

            }

            base.ApplyNestedProperty(resource, resourceInfoWrapper, structuredType, readContext);
        }
    }
}
