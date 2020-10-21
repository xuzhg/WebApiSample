using Microsoft.AspNet.OData.Formatter.Serialization;
using Microsoft.OData;
using Microsoft.OData.Edm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstanceAnnotationWebApiTest
{
    public class MyResourceSetSerializer : ODataResourceSetSerializer
    {
        public MyResourceSetSerializer(ODataSerializerProvider serializerProvider)
            : base(serializerProvider)
        {

        }

        public override ODataResourceSet CreateResourceSet(IEnumerable resourceSetInstance,
            IEdmCollectionTypeReference resourceSetType, ODataSerializerContext writeContext)
        {
            ODataResourceSet resourceSet = base.CreateResourceSet(resourceSetInstance, resourceSetType, writeContext);

            // Add a primitive annotation.
            ODataInstanceAnnotation annotation = new ODataInstanceAnnotation("NS.selectedcount", new ODataPrimitiveValue(50));

            resourceSet.InstanceAnnotations.Add(annotation);

            // Add a complex annotation.
            ODataResourceValue resourceValue = new ODataResourceValue
            {
                TypeName = "InstanceAnnotationWebApiTest.Models.Address",
                Properties = new ODataProperty[]
                {
                    new ODataProperty { Name = "Street", Value = "228th NE" },
                    new ODataProperty { Name = "City", Value = "Sammamish" }
                }
            };

            annotation = new ODataInstanceAnnotation("NS.AddressAnnotation", resourceValue);
            resourceSet.InstanceAnnotations.Add(annotation);

            return resourceSet;
        }
    }
}
