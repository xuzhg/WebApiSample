using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;

namespace NestedFilterSample
{
    class Program
    {
        private const string BaseUri = "http://localhost/odata/";

        static void Main(string[] args)
        { 
            HttpClient client = GetClient();

            Query(client, "Groups?$expand=Queries");

            Query(client, "Groups?$select=Id,Name&$filter=IsHidden eq false&IsShared ne false&$expand=Queries($select=Id,Name,IsPinned)");

            Query(client, "Groups?$select=Id,Name&$filter=IsHidden eq false&IsShared ne false&$expand=Queries($select=Id,Name,IsPinned;$filter=IsPinned eq true)");

            Query(client, "Groups?$select=Id,Name&$filter=IsHidden eq false&IsShared ne false&$expand=Queries($select=Id,Name,IsPinned;$filter=IsPinned ne true)");

            // $count
            Query(client, "Groups?$expand=Queries&$count=true");

            Query(client, "Groups?$expand=Queries($count=true)"); // not work before 5.8
        }

        private static void Query(HttpClient client, string uri)
        {
            string requestUri = BaseUri + uri;
            Console.WriteLine("\n[Query]: "+requestUri);
            HttpResponseMessage response = client.GetAsync(requestUri).Result;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("\n==> Failed.");
            }
            else
            {
                Console.WriteLine("\n==> Succeed.");
            }

            if (response.Content != null)
            {
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }

        private static HttpClient GetClient()
        {
            var config = new HttpConfiguration();
            config.MapODataServiceRoute("odata", "odata", GetEdmModel());
            return new HttpClient(new HttpServer(config));
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Group>("Groups");
            builder.EntitySet<Query>("Queries");
            return builder.GetEdmModel();
        }
    }
}
