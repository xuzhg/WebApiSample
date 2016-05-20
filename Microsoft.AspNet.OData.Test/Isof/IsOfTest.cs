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
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.AspNet.OData.Test.Isof
{
    public class IsOfTest
    {
        [Fact]
        public void Isof_SubType_Metadata()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<A>("As");
            IEdmModel model = builder.GetEdmModel();

            var configuration = new[] { typeof(MetadataController) }.GetHttpConfiguration();
            configuration.MapODataServiceRoute("odata", "odata", model);
            HttpServer server = new HttpServer(configuration);
            var client = new HttpClient(server);

            var response = client.GetAsync("http://localhost/odata/$metadata").Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result); 
        }

        [Fact]
        public void Isof_SubType_Test()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<A>("As");
            IEdmModel model = builder.GetEdmModel();

            var configuration = new[] { typeof(AsController) }.GetHttpConfiguration();
            configuration.MapODataServiceRoute("odata", "odata", model);
            HttpServer server = new HttpServer(configuration);
            var client = new HttpClient(server);

            // Query As only the Dp is a "E" type.
            string payload = @"{
  ""@odata.context"":""http://localhost/odata/$metadata#As"",""value"":[
    {
      ""@odata.type"":""#Microsoft.AspNet.OData.Test.Isof.B"",""Id"":1,""Dp"":{
        ""@odata.type"":""#Microsoft.AspNet.OData.Test.Isof.E"",""Prop"":""E Prop"",""Ep"":""E kind""
      },""Bp"":7
    }
  ]
}";
            var response = client.GetAsync("http://localhost/odata/As?$filter=isof(Dp,'Microsoft.AspNet.OData.Test.Isof.E')").Result;
            Assert.Equal(payload, response.Content.ReadAsStringAsync().Result);

            // Query As only the Dp is a "F" type.
            payload = @"{
  ""@odata.context"":""http://localhost/odata/$metadata#As"",""value"":[
    {
      ""@odata.type"":""#Microsoft.AspNet.OData.Test.Isof.B"",""Id"":2,""Dp"":{
        ""@odata.type"":""#Microsoft.AspNet.OData.Test.Isof.F"",""Prop"":""FFFF Prop"",""Fp"":""FFFF kind""
      },""Bp"":8
    }
  ]
}";
            response = client.GetAsync("http://localhost/odata/As?$filter=isof(Dp,'Microsoft.AspNet.OData.Test.Isof.F')").Result;
            Assert.Equal(payload, response.Content.ReadAsStringAsync().Result);
        }

        public class AsController : ODataController
        {
            private static IList<A> _as;

            static AsController()
            {
                _as = new List<A>();

                A a = new B
                {
                    Id = 1,
                    Bp = 7,
                    Dp = new E
                    {
                        Ep = "E kind",
                        Prop = "E Prop"
                    }
                };
                _as.Add(a);

                a = new B
                {
                    Id = 2,
                    Bp = 8,
                    Dp = new F
                    {
                        Fp = "FFFF kind",
                        Prop = "FFFF Prop"
                    }
                };
                _as.Add(a);
            }

            [EnableQuery]
            public IHttpActionResult Get()
            {
                return Ok(_as);
            }
        }

        public abstract class A
        {
            public int Id { get; set; }
            public D Dp { get; set; }
        }

        public class B : A
        {
            public int Bp { get; set; }
        }

        public abstract class D
        {
            public string Prop { get; set; }
        }

        public class E : D
        {
            public string Ep { get; set; }
        }

        public class F : D
        {
            public string Fp { get; set; }
        }
    }
}
