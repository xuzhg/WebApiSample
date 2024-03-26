using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.OData;
using System.Reflection;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.AspNetCore.OData.Query.Container;
using Microsoft.OData.Edm;
using System.Linq.Expressions;
using Microsoft.AspNetCore.OData.Edm;
using Microsoft.OData.ModelBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System.Collections.Generic;

namespace OData.Annotations.Example
{
    public class CustomODataResourceSerializer(IODataSerializerProvider provider) : ODataResourceSerializer(provider)
    {
        public override ODataResource CreateResource(SelectExpandNode selectExpandNode, ResourceContext resourceContext)
        {
            var resource = base.CreateResource(selectExpandNode, resourceContext);
            if (resource != null)
            {
                // var resourceContextPropDictionary = GetPropertiesDictionary(resourceContext.ResourceInstance);
                Type clrType = MySelectExpandBinder.GetClrType(resourceContext.EdmModel, resourceContext.StructuredType);
                PropertyInfo[] properties = clrType.GetProperties();

                foreach (var prop in resource.Properties)
                {
                    string extraPropertyName = $"{prop.Name}Name";
                    var propertyInfo = properties.FirstOrDefault(c => c.Name.Equals(extraPropertyName, StringComparison.OrdinalIgnoreCase));
                    if (propertyInfo == null)
                    {
                        continue;
                    }

                    object value = resourceContext.GetPropertyValue(extraPropertyName);
                    if (value != null)
                    {
                        prop.InstanceAnnotations.Add(new ODataInstanceAnnotation("lookup.name", new ODataPrimitiveValue(value)));
                    }
                    else
                    {
                        prop.InstanceAnnotations.Add(new ODataInstanceAnnotation("lookup.name", new ODataNullValue()));
                    }

                    //var propNameToLower = prop.Name.ToLower();

                    //if (resourceContextPropDictionary.TryGetValue($"{propNameToLower}name", out object lookupNamePropValue))
                    //{
                    //    prop.InstanceAnnotations.Add(new ODataInstanceAnnotation("lookup.name", new ODataPrimitiveValue(lookupNamePropValue)));
                    //}
                }
            }
            return resource;
        }

        private Dictionary<string, object> GetPropertiesDictionary(object obj)
        {
            Dictionary<string, object> propertiesDictionary = new Dictionary<string, object>();
            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                object value = property.GetValue(obj);
                propertiesDictionary.Add(property.Name.ToLower(), value);
            }

            return propertiesDictionary;
        }
    }

    public class MySelectExpandBinder : SelectExpandBinder
    {
        public MySelectExpandBinder(IFilterBinder filterBinder, IOrderByBinder orderByBinder)
            : base(filterBinder, orderByBinder)
        {
        }

        protected override void BindOrderByProperties(QueryBinderContext context, Expression source,
            IEdmStructuredType structuredType,
            IList<NamedPropertyExpression> includedProperties, bool isSelectedAll)
        {

            // This place is not good place to add the extra property, but it seems this is the only place that we can add
            if (structuredType.FullTypeName() == "OData.Annotations.Example.Controllers.Account")
            {
                Type clrType = GetClrType(context.Model, structuredType);

                PropertyInfo[] properties = clrType.GetProperties();

                IList<NamedPropertyExpression> extraProperties = new List<NamedPropertyExpression>();

                foreach (var included in includedProperties)
                {
                    string includedProperty = ((ConstantExpression)included.Name).Value.ToString().ToLower() + "name";
                    var propertyInfo = properties.FirstOrDefault(c => c.Name.Equals(includedProperty, StringComparison.OrdinalIgnoreCase));
                    if (propertyInfo == null)
                    {
                        continue;
                    }

                    Expression extraPropertyName = Expression.Constant(propertyInfo.Name);
                    Expression extraPropertyValue = Expression.Property(source, propertyInfo);
                    extraProperties.Add(new NamedPropertyExpression(extraPropertyName, extraPropertyValue)
                    {
                        //AutoSelected = true
                    });
                }

                foreach (var e in extraProperties)
                {
                    includedProperties.Add(e);
                }
            }
            //

            base.BindOrderByProperties(context, source, structuredType, includedProperties, isSelectedAll);
        }

        public static Type GetClrType(IEdmModel edmModel, IEdmStructuredType type)
        {
            ClrTypeAnnotation annotation = edmModel.GetAnnotationValue<ClrTypeAnnotation>(type);
            if (annotation != null)
            {
                return annotation.ClrType;
            }

            throw new InvalidOperationException($"Cannot find the CLR type for {type.FullTypeName()}");
        }
    }
}
