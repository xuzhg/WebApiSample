using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Library;
using Microsoft.OData.Edm.Library.Values;
using Xunit;

namespace Microsoft.AspNet.OData.Test
{
    public class NamesapcePrefixTest
    {
        [Fact]
        public void AddNamespaceForEmptyEdmModel()
        {
            EdmModel model = new EdmModel();
            EdmEntityContainer container = new EdmEntityContainer("NS", "Container");
            model.SetAnnotationValue(container, "http://microsoft.com", "Foo",
                new EdmBooleanConstant(EdmCoreModel.Instance.GetBoolean(false), true));
            model.AddElement(container);

            string metadata = GetMetadataDocument(model);

            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<edmx:Edmx Version=""4.0"" xmlns:edmx=""http://docs.oasis-open.org/odata/ns/edmx"">
  <edmx:DataServices>
    <Schema Namespace=""NS"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityContainer Name=""Container"" p4:Foo=""true"" xmlns:p4=""http://microsoft.com"" />
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>";

            Assert.Equal(xml, metadata);

            // Why the auto-generated prefix is "p4"?
            // Sam's note:
            // By Default, XmlWriter has 3 three default Namespaces:
            // 1. xmlns= http://www.w3.org/XML/1998/namespace
            // 2. xml = http://www.w3.org/2000/xmlns/
            // 3. "" = ""  // empty

            // In my example, ODL will add 1 new namespace and replace 1:
            // 1. xmlns:edmx=http://docs.oasis-open.org/odata/ns/edmx
            // 2. xmlns=http://docs.oasis-open.org/odata/ns/edm  (replace)

            // I also add two new namespaces:
            // 1. "NS"
            // 2. "http://microsoft.com"

            // So, total number of namespaces is 6. 
            // XmlWriter use < "p" + (number - 2) > to auto generate the namespace prefix.
            // therefore, the first prefix is "p4".
        }

        [Fact]
        public void AddNamespaceForEmptyEdmModel_WithMultiplePrefix()
        {
            string namespaceName = "http://microsoft.com";
            string otherNamespaceName = "http://foo.bar.com";

            EdmModel model = new EdmModel();
            EdmEntityContainer container = new EdmEntityContainer("NS", "Container");
            model.SetAnnotationValue(container, namespaceName, "Foo",
                new EdmBooleanConstant(EdmCoreModel.Instance.GetBoolean(false), true));

            model.SetAnnotationValue(container, otherNamespaceName, "PI",
                new EdmFloatingConstant(EdmCoreModel.Instance.GetDouble(false), 3.14));

            model.AddElement(container);

            string metadata = GetMetadataDocument(model);

            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<edmx:Edmx Version=""4.0"" xmlns:edmx=""http://docs.oasis-open.org/odata/ns/edmx"">
  <edmx:DataServices>
    <Schema Namespace=""NS"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityContainer Name=""Container"" p4:Foo=""true"" p5:PI=""3.14"" xmlns:p5=""http://foo.bar.com"" xmlns:p4=""http://microsoft.com"" />
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>";
            Assert.Equal(xml, metadata);

            // Why the auto-generated prefix is "p4" & "p5"?
        }

        [Fact]
        public void AddNamespaceForEmptyEdmModel_WithCustomPrefix()
        {
            string namespaceName = "http://microsoft.com";
            string otherNamespaceName = "http://foo.bar.com";

            EdmModel model = new EdmModel();
            EdmEntityContainer container = new EdmEntityContainer("NS", "Container");
            model.SetAnnotationValue(container, namespaceName, "Foo",
                new EdmBooleanConstant(EdmCoreModel.Instance.GetBoolean(false), true));

            model.SetAnnotationValue(container, otherNamespaceName, "PI",
                new EdmFloatingConstant(EdmCoreModel.Instance.GetDouble(false), 3.14));

            model.AddElement(container);
            model.SetNamespacePrefixMappings(new[]
            {
                new KeyValuePair<string, string>("myns", namespaceName)
            });

            string metadata = GetMetadataDocument(model);

            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<edmx:Edmx Version=""4.0"" xmlns:edmx=""http://docs.oasis-open.org/odata/ns/edmx"">
  <edmx:DataServices>
    <Schema Namespace=""NS"" xmlns:myns=""http://microsoft.com"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityContainer Name=""Container"" myns:Foo=""true"" p6:PI=""3.14"" xmlns:p6=""http://foo.bar.com"" />
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>";

            Assert.Equal(xml, metadata);

            // Why the auto-generated prefix is "p6"?
        }

        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [Fact]
        public void AddNamespaceForConventionModel_ForProperty()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntityType<Customer>();
            IEdmModel model = builder.GetEdmModel();

            const string namespaceName = "http://my.org/schema";
            var type = "Microsoft.AspNet.OData.Test.Customer";
            const string localName = "MyCustomAttribute";

            // this registers a "myns" namespace on the model
            model.SetNamespacePrefixMappings(new[] { new KeyValuePair<string, string>("myns", namespaceName),
                new KeyValuePair<string, string>("kk", "http://foo.bar.com"),
            });

            // set a simple string as the value of the "MyCustomAttribute" annotation on the "RevisionDate" property
            var stringType = EdmCoreModel.Instance.GetString(true);
            var value = new EdmStringConstant(stringType, "!MyString!");
            model.SetAnnotationValue(((IEdmEntityType)model.FindType(type)).FindProperty("Name"),
                                    namespaceName, localName, value);

            model.SetAnnotationValue(((IEdmEntityType)model.FindType(type)).FindProperty("Id"),
                                    "http://foo.bar.com", localName, value);

            string metadata = GetMetadataDocument(model);

            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<edmx:Edmx Version=""4.0"" xmlns:edmx=""http://docs.oasis-open.org/odata/ns/edmx"">
  <edmx:DataServices>
    <Schema Namespace=""Microsoft.AspNet.OData.Test"" xmlns:myns=""http://my.org/schema"" xmlns:kk=""http://foo.bar.com"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityType Name=""Customer"">
        <Key>
          <PropertyRef Name=""Id"" />
        </Key>
        <Property Name=""Id"" Type=""Edm.Int32"" Nullable=""false"" kk:MyCustomAttribute=""!MyString!"" />
        <Property Name=""Name"" Type=""Edm.String"" myns:MyCustomAttribute=""!MyString!"" />
      </EntityType>
    </Schema>
    <Schema Namespace=""Default"" xmlns:myns=""http://my.org/schema"" xmlns:kk=""http://foo.bar.com"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityContainer Name=""Container"" />
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>";

            Assert.Equal(xml, metadata);
        }

        private static string GetMetadataDocument(IEdmModel model)
        {
            var configuration = new[] { typeof(MetadataController) }.GetHttpConfiguration();
            configuration.MapODataServiceRoute("odata", "odata", model);
            HttpServer server = new HttpServer(configuration);
            var client = new HttpClient(server);

            var response = client.GetAsync("http://localhost/odata/$metadata").Result;

            string payload = response.Content.ReadAsStringAsync().Result;
            //Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            return payload;
        }
    }
}
