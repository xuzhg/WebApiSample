using Microsoft.OData;
using Microsoft.OData.Edm;
using ODataReadWrite.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ODataReadWrite.Write
{
    class ODataWriteTest
    {
        public static void TestWriting()
        {
            ODataMessageWriterSettings settings = new ODataMessageWriterSettings
            {
                ODataUri = new ODataUri { ServiceRoot = new Uri("http://localhost") },
                ShouldIncludeAnnotation = s => true
            };

            MemoryStream outputStream = new MemoryStream();
            IODataResponseMessage message = new ODataMessageWrapper(outputStream);

            IEdmModel edmModel = EdmModelBuilder.GetEdmModel();
            IEdmEntityType customerType = edmModel.FindDeclaredType("NS.Customer") as IEdmEntityType;
            IEdmEntitySet customers = edmModel.EntityContainer.FindEntitySet("Customers");

            using(var messageWriter = new ODataMessageWriter(message, settings, edmModel))
            {
                ODataWriter writer = messageWriter.CreateODataResourceWriter(customers, customerType);

                ODataResource resource = new ODataResource
                {
                    TypeName = "NS.Customer",
                    Properties = [
                        new ODataProperty { Name = "Id", Value = 102 },
                        new ODataProperty { Name = "Name", Value = "Bob" }
                        // ,
                        //new ODataProperty { Name = "Emails", Value = new ODataCollectionValue
                        //        {
                        //            TypeName = "Collection(Edm.String)",
                        //            Items = [ "sam@abc.com", "petter@abc.com"]
                        //        }
                        //    }
                        ]
                };

                ODataPropertyInfo propertyInfo = new ODataPropertyInfo();
                propertyInfo.Name = "Emails";
                // propertyInfo.InstanceAnnotations.Add(new ODataInstanceAnnotation("odata.count", new ODataPrimitiveValue(4)));
                propertyInfo.InstanceAnnotations.Add(new ODataInstanceAnnotation("my.count", new ODataPrimitiveValue(4)));

                writer.WriteStart(resource);
                    writer.WriteStart(propertyInfo);
                    writer.WriteEnd();
                writer.WriteEnd();

                outputStream.Seek(0, SeekOrigin.Begin);
                string output = new StreamReader(outputStream).ReadToEnd();
                Console.WriteLine(output);
            }
        }
        
    }
}
