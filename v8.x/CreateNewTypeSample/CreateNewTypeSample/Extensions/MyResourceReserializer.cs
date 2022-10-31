using CreateNewTypeSample.Models;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.Globalization;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace CreateNewTypeSample.Extensions
{
    public class MyResourceReserializer : ODataResourceSerializer
    {
        public MyResourceReserializer(IODataSerializerProvider serializerProvider) 
            : base(serializerProvider)
        {
        }

        public override ODataProperty CreateStructuralProperty(
            IEdmStructuralProperty structuralProperty, ResourceContext resourceContext)
        {
            if (structuralProperty.Name == "Dist" && structuralProperty.Type.IsTypeDefinition())
            {
                object propertyValue = resourceContext.GetPropertyValue(structuralProperty.Name);
                var distValue = propertyValue as Distance;
                return new ODataProperty
                {
                    Name = structuralProperty.Name,
                    Value = new ODataPrimitiveValue(distValue.ToString())
                };
            }

            return base.CreateStructuralProperty(structuralProperty, resourceContext);
        }
    }
}
