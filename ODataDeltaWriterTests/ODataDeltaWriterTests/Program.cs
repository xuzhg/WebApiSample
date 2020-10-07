
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Newtonsoft.Json.Linq;

namespace ConsoleApp1
{
    class Program
    {
        const string BaseUri = "http://www.example.com/";

        static void Main(string[] args)
        {
            Test(ODataVersion.V4, true);

            Test(ODataVersion.V4, false);

            Test(ODataVersion.V401, true);

            Test(ODataVersion.V401, false);

            Console.WriteLine("Hello World!");
        }

        private static void Test(ODataVersion version, bool response)
        {
            Console.WriteLine($"\nIn ==> " + (response ? "response" : "request") + $"            OData Version {version}\n");
            var writerSettings = new ODataMessageWriterSettings();

            IEdmModel model = GetEdmModel(out IEdmEntitySet customers, out IEdmEntityType customer);

            ODataDeltaResourceSet feed = GetResourceSet(response);

            string payload = Write(model, version, response, writerSettings, omWriter =>
            {
                var writer = omWriter.CreateODataDeltaResourceSetWriter(customers, customer);
                writer.WriteStart(feed);

                WriteResource(writer);

                WriteDeletedResource(writer);

                writer.WriteEnd();

            }, out string contentType);

            Console.WriteLine(JObject.Parse(payload));
        }

        private static void WriteResource(ODataWriter omWriter)
        {
            ODataResource customer = new ODataResource
            {
                Id = new Uri("Customers(1)", UriKind.Relative),
                Properties = new List<ODataProperty>
                {
                    new ODataProperty { Name = "CustomerId", Value = 1 },
                    new ODataProperty { Name = "Name", Value = "Sam Xu" },
                },
                TypeName = "NS.Customer"
            };

            omWriter.WriteStart(customer);
            WriteNestedResource(omWriter);
            omWriter.WriteEnd();
        }

        private static void WriteNestedResource(ODataWriter omWriter)
        {
            ODataResource address = new ODataResource
            {
                Properties = new List<ODataProperty>
                {
                    new ODataProperty { Name = "Street", Value = "154TH AVE NE" },
                    new ODataProperty { Name = "City", Value = "Redmond" },
                },
                TypeName = "NS.Address"
            };

            ODataNestedResourceInfo nested = new ODataNestedResourceInfo
            {
                Name = "HomeAddress",
                IsCollection = false
            };

            omWriter.WriteStart(nested);
            omWriter.WriteStart(address);
            omWriter.WriteEnd();
            omWriter.WriteEnd();
        }

        private static void WriteDeletedResource(ODataWriter omWriter)
        {
            ODataDeletedResource changedCustomer = new ODataDeletedResource(new Uri("Customers(7)", UriKind.Relative), DeltaDeletedEntryReason.Changed)
            {
                TypeName = "NS.Customer",
                Properties = new[]
                {
                    new ODataProperty {Name = "CustomerId", Value = 7 },
                    new ODataProperty {Name = "Name", Value = "Peter"}
                },
            };

            omWriter.WriteStart(changedCustomer);
            omWriter.WriteEnd();

            ODataDeletedResource deleteCustomer = new ODataDeletedResource(new Uri("Customers(19)", UriKind.Relative), DeltaDeletedEntryReason.Deleted);
            omWriter.WriteStart(deleteCustomer);
            omWriter.WriteEnd();
        }

        private static ODataDeltaResourceSet GetResourceSet(bool response)
        {
            ODataDeltaResourceSet feed;
            if (response)
            {
                feed = new ODataDeltaResourceSet
                {
                    Count = 5,
                    DeltaLink = new Uri("Customers?$expand=Orders&$deltatoken=8015", UriKind.Relative)
                };
            }
            else
            {
                feed = new ODataDeltaResourceSet();
            }

            feed.SetSerializationInfo(new ODataDeltaResourceSetSerializationInfo
            {
                EntitySetName = "Customers",
                EntityTypeName = "NS.Customer",
                ExpectedTypeName = "NS.Customer"
            });

            return feed;
        }

        private static string Write(IEdmModel edmModel, ODataVersion version, bool response, ODataMessageWriterSettings writerSettings, Action<ODataMessageWriter> writerAction,
            out string contentType)
        {
            var message = new InMemoryMessage() { Stream = new MemoryStream() };

            writerSettings.EnableMessageStreamDisposal = false;
            writerSettings.BaseUri = new Uri(BaseUri);
            writerSettings.ODataUri.ServiceRoot = new Uri(BaseUri);
            writerSettings.SetContentType(ODataFormat.Json);
            writerSettings.Version = version;

            if (response)
            {
                using (var msgWriter = new ODataMessageWriter((IODataResponseMessage)message, writerSettings, edmModel))
                {
                    writerAction(msgWriter);
                }
            }
            else
            {
                using (var msgWriter = new ODataMessageWriter((IODataRequestMessage)message, writerSettings, edmModel))
                {
                    writerAction(msgWriter);
                }
            }

            string payload;
            message.Stream.Seek(0, SeekOrigin.Begin);
            using (StreamReader reader = new StreamReader(message.Stream))
            {
                contentType = message.GetHeader("Content-Type");
                payload = reader.ReadToEnd();
                return payload;
            }
        }

        private static IEdmModel GetEdmModel(out IEdmEntitySet customers, out IEdmEntityType customer)
        {
            var model = new EdmModel();

            var defaultContainer = new EdmEntityContainer("NS", "Default");
            model.AddElement(defaultContainer);

            EdmEntityType customerType = new EdmEntityType("NS", "Customer");
            customer = customerType;
            var customerIdProperty = new EdmStructuralProperty(customerType, "CustomerId", EdmCoreModel.Instance.GetInt32(false));
            customerType.AddProperty(customerIdProperty);
            customerType.AddKeys(new IEdmStructuralProperty[] { customerIdProperty });
            customerType.AddProperty(new EdmStructuralProperty(customerType, "Name", EdmCoreModel.Instance.GetString(false)));
            model.AddElement(customerType);

            EdmComplexType addressType = new EdmComplexType("NS", "Address");
            addressType.AddProperty(new EdmStructuralProperty(addressType, "Street", EdmCoreModel.Instance.GetString(false)));
            addressType.AddProperty(new EdmStructuralProperty(addressType, "City", EdmCoreModel.Instance.GetString(false)));
            model.AddElement(addressType);

            customerType.AddProperty(new EdmStructuralProperty(customerType, "HomeAddress", new EdmComplexTypeReference(addressType, true)));
            customers = new EdmEntitySet(defaultContainer, "Customers", customerType);
            defaultContainer.AddElement(customers);

            return model;
        }
    }
}
