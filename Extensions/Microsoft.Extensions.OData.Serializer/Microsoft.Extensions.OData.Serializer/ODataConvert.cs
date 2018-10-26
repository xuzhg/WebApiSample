using Microsoft.AspNet.OData.Builder;
using Microsoft.OData;
using Microsoft.OData.Edm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.OData.Serializer
{
    public static class ODataConvert
    {
        public static string ConvertToOData(object value)
        {
            // TODO:
            // 1. if primitive
            // 2. if enum
            // 3. if collection of (primitive or enum)

            // 4. 
            InMemoryMessage message = new InMemoryMessage();
            message.SetHeader("Content-Type", "application/json;odata.metadata=minimal");
            message.Stream = new MemoryStream();

            var settings = new ODataMessageWriterSettings
            {
                ODataUri = new ODataUri
                {
                    ServiceRoot = new Uri("http://localhost/"),
                },
            };
            var model = GetEdmModel(value.GetType());
            var writer = new ODataMessageWriter((IODataResponseMessage)message, settings, model);

            IEdmEntitySet entitySet = model.FindDeclaredEntitySet(value.GetType().Name + "s");
            ODataWriter odataWriter = writer.CreateODataResourceWriter(entitySet);
            Type valueType = value.GetType();
            bool isCollection = IsCollection(valueType, out Type elementType);

            if (value == null)
            {
                if (isCollection)
                {
                    odataWriter.WriteStart(new ODataResourceSet
                    {
                        TypeName = "Collection(" + elementType.FullName + ")"
                    });
                }
                else
                {
                    odataWriter.WriteStart(resource: null);
                }

                odataWriter.WriteEnd();
            }
            else
            {
                if (isCollection)
                {
                    WriteResourceSet(model, odataWriter, value);
                }
                else
                {
                    WriteResource(model, odataWriter, value);
                }
            }

            message.Stream.Position = 0;
            return (new StreamReader(message.Stream)).ReadToEnd();
        }

        private static void WriteNested(IEdmModel model, ODataWriter odataWriter, object value, PropertyInfo property)
        {
            ODataNestedResourceInfo nestedResourceInfo = new ODataNestedResourceInfo
            {
                Name = property.Name
            };

            Type propType = property.PropertyType;
            bool isCollection = IsCollection(propType, out Type elementType);
            nestedResourceInfo.IsCollection = isCollection;

            object propValue = property.GetValue(value);
            odataWriter.WriteStart(nestedResourceInfo);

            if (propValue == null)
            {
                if (isCollection)
                {
                    odataWriter.WriteStart(new ODataResourceSet
                    {
                        TypeName = "Collection(" + elementType.FullName + ")"
                    });
                }
                else
                {
                    odataWriter.WriteStart(resource: null);
                }

                odataWriter.WriteEnd();
            }
            else
            {
                if (isCollection)
                {
                    WriteResourceSet(model, odataWriter, propValue);
                }
                else
                {
                    WriteResource(model, odataWriter, propValue);
                }
            }

            odataWriter.WriteEnd();
        }

        public static void WriteResourceSet(IEdmModel model, ODataWriter odataWriter, object value)
        {
            Type valueType = value.GetType();
            IsCollection(valueType, out Type elementType);

            IEdmStructuredType structruedType = model.FindDeclaredType(valueType.FullName) as IEdmStructuredType;

            ODataResourceSet resourceSet = new ODataResourceSet
            {
                TypeName = "Collection(" + elementType.FullName + ")"
            };

            odataWriter.WriteStart(resourceSet);
            IEnumerable items = value as IEnumerable;

            foreach (object item in items)
            {
                if (item == null)
                {
                    odataWriter.WriteStart(resource: null);
                    odataWriter.WriteEnd();
                }
                else
                {
                    WriteResource(model, odataWriter, item);
                }
            }

            odataWriter.WriteEnd();
        }

        public static void WriteResource(IEdmModel model, ODataWriter odataWriter, object value)
        {
            Type myType = value.GetType();
            IEdmStructuredType structruedType = model.FindDeclaredType(myType.Namespace + "." + myType.Name) as IEdmStructuredType;

            var properties = new List<ODataProperty>();
            PropertyInfo[] props = myType.GetProperties();

            IList<PropertyInfo> nestedProperty = new List<PropertyInfo>();
            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(value);

                if (prop.PropertyType.IsValueType || prop.PropertyType == typeof(string))
                {
                    properties.Add(new ODataProperty { Name = prop.Name, Value = propValue });
                    continue;
                }

                if (propValue == null)
                {
                    Type nullableType = Nullable.GetUnderlyingType(prop.PropertyType);
                    if (nullableType != null)
                    {
                        properties.Add(new ODataProperty { Name = prop.Name, Value = new ODataNullValue() });
                        continue;
                    }
                }

                nestedProperty.Add(prop);
            }

            var entity = new ODataResource
            {
                TypeName = myType.FullName,
                Properties = properties,
            };

            odataWriter.WriteStart(entity);

            foreach (var nested in nestedProperty)
            {
                WriteNested(model, odataWriter, value, nested);
            }

            odataWriter.WriteEnd();
        }

        public static bool IsCollection(Type clrType, out Type elementType)
        {
            elementType = clrType;

            // see if this type should be ignored.
            if (clrType == typeof(string))
            {
                return false;
            }

            Type collectionInterface
                = clrType.GetInterfaces()
                    .Union(new[] { clrType })
                    .FirstOrDefault(
                        t => t.IsGenericType
                             && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (collectionInterface != null)
            {
                elementType = collectionInterface.GetGenericArguments().Single();
                return true;
            }

            return false;
        }
        private static IEdmModel GetEdmModel(Type type)
        {
            var builder = new ODataConventionModelBuilder();
            builder.AddEntitySet(type.Name + "s", builder.AddEntityType(type));
            return builder.GetEdmModel();
        }

        private static IEdmModel BuildModel(Type type)
        {
            EdmModel model = new EdmModel();
            BuildEntityType(model, type);
            return model;
        }

        private static void BuildEntityType(EdmModel model, Type type)
        {
            EdmEntityType entityType = new EdmEntityType(type.Namespace, type.Name);
            model.AddElement(entityType);

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.PropertyType.IsValueType)
                {
                    BuildProperty(entityType, property);
                }
                else
                {
                    BuildEntityType(model, property.PropertyType);
                }
            }
        }

        private static void BuildProperty(EdmEntityType entityType, PropertyInfo propertyInfo)
        {
            entityType.AddStructuralProperty(propertyInfo.Name, GetEdmPrimitiveKind(propertyInfo.PropertyType));

            // if ()
        }

        private static EdmPrimitiveTypeKind GetEdmPrimitiveKind(Type type)
        {
            if (type == typeof(int))
            {
                return EdmPrimitiveTypeKind.Int32;
            }
            else if (type == typeof(double))
            {
                return EdmPrimitiveTypeKind.Double;
            }
            else if (type == typeof(DateTimeOffset))
            {
                return EdmPrimitiveTypeKind.DateTimeOffset;
            }
            else
            {
                return EdmPrimitiveTypeKind.String;
            }
        }
    }
}
