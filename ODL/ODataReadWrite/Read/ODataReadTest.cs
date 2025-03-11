using Microsoft.OData;
using Microsoft.OData.Edm;
using ODataReadWrite.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ODataReadWrite.Read
{
    class ODataReadTest
    {
        public static void TestReadResource()
        {
            string requestData = @"{
  ""@odata.type"": ""#NS.Customer"",
  ""DisplayName@Microsoft.DirectoryServices.authority"": ""Cloud"",
  ""DisplayName"": ""Group Display Name"",
  ""AssignedLabels@Microsoft.DirectoryServices.authority"": ""Cloud"",
  ""AssignedLabels"": [
    {
      ""City"": ""City1"",
      ""Street"": ""Street1""
    }
  ]
}";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json;odata.metadata=minima;odata.streaming=true");
            headers.Add("Accept", "application/json");

            ODataMessageReaderSettings readerSettings = new ODataMessageReaderSettings
            {
                BaseUri = new Uri("http://localhost"),
                ShouldIncludeAnnotation = s => true
            };

            //ODataResourceWrapper topLevelResource = null;

            ODataItemWrapper topLevel = null, currentItem = null;
            IEdmModel model = EdmModelBuilder.GetEdmModel();
            IEdmEntitySet customers = model.EntityContainer.FindEntitySet("Customers");

            //  ODataResource resource = null;
            using (MemoryStream ms = new MemoryStream(Encoding.GetEncoding("iso-8859-1").GetBytes(requestData)))
            {
                ODataMessageWrapper requestMessage = new ODataMessageWrapper(ms);

                ODataMessageReader messageReader = new ODataMessageReader((IODataRequestMessage)requestMessage, readerSettings, model);

                Stack<ODataItemWrapper> resStack = new Stack<ODataItemWrapper>();
                ODataReader odataReader = messageReader.CreateODataResourceReader(customers, customers.EntityType);

                while (odataReader.Read())
                {
                    Console.WriteLine(odataReader.State);

                    switch (odataReader.State)
                    {
                        case ODataReaderState.ResourceStart:
                            ODataResourceWrapper newResource = new ODataResourceWrapper();
                            newResource.Resource = odataReader.Item as ODataResource;

                            resStack.TryPeek(out currentItem);
                            if (currentItem == null)
                            {
                                // TopLevel, do nothing
                                Debug.Assert(topLevel == null);
                                topLevel = newResource;
                            }
                            else
                            {
                                currentItem.Append(newResource);
                            }

                            resStack.Push(newResource);
                            break;

                        case ODataReaderState.ResourceSetStart:
                            ODataResourceSet resourceSet = (ODataResourceSet)odataReader.Item;
                            ODataResourceSetWrapper setWrapper = new ODataResourceSetWrapper
                            {
                                ResourceSet = resourceSet
                            };

                            resStack.TryPeek(out currentItem);
                            if (currentItem == null)
                            {
                                // TopLevel, do nothing
                                Debug.Assert(topLevel == null);
                                topLevel = setWrapper;
                            }
                            else
                            {
                                currentItem.Append(setWrapper);
                            }

                            resStack.Push(setWrapper);
                            break;

                        case ODataReaderState.NestedResourceInfoStart:
                            resStack.TryPeek(out currentItem);
                            Debug.Assert(currentItem != null);

                            ODataItem item = odataReader.Item;
                            ODataNestedResourceInfoWrapper infoWrapper = new ODataNestedResourceInfoWrapper
                            {
                                NestedInfo = item as ODataNestedResourceInfo
                            };

                            currentItem.Append(infoWrapper);
                            resStack.Push(infoWrapper);
                            break;

                        case ODataReaderState.ResourceEnd:
                            //currWrapper = resStack.Peek();
                            //currWrapper.Resource = (ODataResource)odataReader.Item;
                            resStack.Pop();
                            break;

                        case ODataReaderState.ResourceSetEnd:

                            resStack.Pop();
                            break;

                        case ODataReaderState.NestedResourceInfoEnd:

                            item = odataReader.Item;

                            resStack.Pop();
                            break;
                    }
                }
            }

            if (topLevel == null)
            {
                return;
            }

            ODataResourceWrapper topLevelResource = topLevel as ODataResourceWrapper;
            if (topLevelResource != null)
            {
                foreach (var a in topLevelResource.Resource.Properties.OfType<ODataProperty>())
                {
                    Console.WriteLine(a.Name + ": " + a.Value);
                }

                foreach (var a in topLevelResource.NestedResourceInfos)
                {
                    Console.WriteLine(a.NestedInfo.Name);
                    ODataResourceSetWrapper setWrapper = a.NestedWrapper as ODataResourceSetWrapper;
                    ODataResourceWrapper resourceWrapper = a.NestedWrapper as ODataResourceWrapper;
                    if (resourceWrapper != null)
                    {
                        foreach (var prop in resourceWrapper.Resource.Properties.OfType<ODataProperty>())
                        {
                            Console.WriteLine(prop.Name + ": " + prop.Value);
                        }
                    }
                }
            }
        }
    }
}
