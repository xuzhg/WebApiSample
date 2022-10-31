using CreateNewTypeSample.Models;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.Globalization;
using System.Runtime.InteropServices;

namespace CreateNewTypeSample.Extensions
{
    public class MyPrimitiveReserializer : ODataPrimitiveSerializer
    {
        public override ODataPrimitiveValue CreateODataPrimitiveValue(
            object graph, IEdmPrimitiveTypeReference primitiveType, ODataSerializerContext writeContext)
        {
            if (graph == null)
            {
                return null;
            }

            if (primitiveType.FullName() == "Edm.Temperature")
            {
                Temperature temperature = graph as Temperature;
                ODataPrimitiveValue value = new ODataPrimitiveValue(temperature.ToString());

                // without this, it will write "Edm.String".
                value.TypeAnnotation = new ODataTypeAnnotation("Edm.Temperature");
                return value;
            }

            return base.CreateODataPrimitiveValue(graph, primitiveType, writeContext);
        }
    }
    
    // This is required to skip the validation.
    public class MyPayloadValueConverter : ODataPayloadValueConverter
    {
        public override object ConvertToPayloadValue(object value, IEdmTypeReference edmTypeReference)
        {
            return base.ConvertToPayloadValue(value, edmTypeReference);
        }

        public override object ConvertFromPayloadValue(object value, IEdmTypeReference edmTypeReference)
        {
            return base.ConvertFromPayloadValue(value, edmTypeReference);
        }
    }
}
