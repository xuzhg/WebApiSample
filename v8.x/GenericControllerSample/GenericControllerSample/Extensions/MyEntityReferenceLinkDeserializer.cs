using GenericControllerSample.Controllers;
using GenericControllerSample.Models;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OData.Formatter.Deserialization;
using Microsoft.OData;
using System.Reflection;

namespace GenericControllerSample.Extensions
{
    public class MyEntityReferenceLinkDeserializer : ODataEntityReferenceLinkDeserializer
    {
        public override async Task<object> ReadAsync(ODataMessageReader messageReader, Type type, ODataDeserializerContext readContext)
        {
            // Be noted:
            // When a request body contains 'Id' as below
            /* POST http://localhost:5182/api/odata/Customers/1/annotation/$ref
            {
                "@odata.id":"$2"
            }

            */
            // The default deserializer by default replaces/resolve the content-id using the real Url.
            // This customized deserializer just returns the content-id, omitting the replacement.

            ODataEntityReferenceLink entityReferenceLink = await messageReader.ReadEntityReferenceLinkAsync().ConfigureAwait(false);

            return entityReferenceLink.Url;
        }
    }
}
