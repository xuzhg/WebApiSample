using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using Microsoft.OData.Edm;
using Newtonsoft.Json.Linq;

namespace DerivedEntityTypeApp
{
    public class DynamicPropertyClass
    {
        public DynamicPropertyClass()
        {
            HttpClient client = GetClient();

            //Query(client, "$metadata");

            Query(client, "Users");

            Patch(client, "Users(2)");

            Query(client, "Users(2)");
        }

        private static void Query(HttpClient client, string uri)
        {
            string requestUri = "http://localhost/us/" + uri;
            Console.WriteLine(requestUri);

            HttpResponseMessage response = client.GetAsync(requestUri).Result;
            Console.WriteLine(response.StatusCode);

            if (response.Content != null)
            {
                if (response.Content.Headers.ContentType.MediaType.Contains("xml"))
                {
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Console.WriteLine(JObject.Parse(response.Content.ReadAsStringAsync().Result));
                }
            }
        }

        private static void Patch(HttpClient client, string uri)
        {
            string requestUri = "http://localhost/us/" + uri;
            Console.WriteLine(requestUri);

            const string content = @"{'Name':'Peter','Data':{'IntValue':101,'DoubleValue':19.9}}";
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = new StringContent(content)
            };
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            HttpResponseMessage response = client.SendAsync(request).Result;

            Console.WriteLine(response.StatusCode);

            if (response.Content != null)
            {
                if (response.Content.Headers.ContentType.MediaType.Contains("xml"))
                {
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Console.WriteLine(JObject.Parse(response.Content.ReadAsStringAsync().Result));
                }
            }
        }

        private static HttpClient GetClient()
        {
            var config = new HttpConfiguration();
            config.MapODataServiceRoute("us", "us", GetEdmModel());
            return new HttpClient(new HttpServer(config));
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<User>("Users");
            return builder.GetEdmModel();
        }
    }

    public class UsersController : ODataController
    {
        private static readonly IList<User> Users;
        static UsersController()
        {
            Users = new List<User>();
            User user = new User
            {
                Id = 1,
                Name = "Tony",
                Data = new Data { IntValue = 8 }
            };
            Users.Add(user);

            user = new User
            {
                Id = 2,
                Name = "Sam",
                Data = new Data
                {
                    IntValue = 99,
                    DynamicProperties = new Dictionary<string, object>
                    {
                        { "StringProperty", "StringValue1" },
                        { "LongValue", 987L }
                    }
                }
            };
            Users.Add(user);
        }

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(Users);
        }

        [EnableQuery]
        public IHttpActionResult Patch(int key, Delta<User> patch)
        {
            User user = Users.FirstOrDefault(c => c.Id == key);
            if (user == null)
            {
                return NotFound();
            }

            patch.Patch(user);
            return Updated(user);
        }
    }

    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Data Data { get; set; }
    }

    public class Data
    {
        public int IntValue { get; set; }

        public IDictionary<string, object> DynamicProperties { get; set; }
    }
}

// output
#if false

http://localhost/us/Users
OK
{
  "@odata.context": "http://localhost/us/$metadata#Users",
  "value": [
    {
      "Id": 1,
      "Name": "Tony",
      "Data": {
        "IntValue": 8
      }
    },
    {
      "Id": 2,
      "Name": "Sam",
      "Data": {
        "IntValue": 99,
        "StringProperty": "StringValue1",
        "LongValue": 987
      }
    }
  ]
}
http://localhost/us/Users(2)
NoContent
http://localhost/us/Users(2)
OK
{
  "@odata.context": "http://localhost/us/$metadata#Users",
  "value": [
    {
      "Id": 1,
      "Name": "Tony",
      "Data": {
        "IntValue": 8
      }
    },
    {
      "Id": 2,
      "Name": "Peter",
      "Data": {
        "IntValue": 101,
        "DoubleValue": 19.9
      }
    }
  ]
}

#endif