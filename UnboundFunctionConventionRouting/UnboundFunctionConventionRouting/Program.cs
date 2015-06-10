using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using UnboundFunctionConventionRouting.Models;

namespace UnboundFunctionConventionRouting
{
    class Program
    {/*
      * ""@odata.type"":""#UnboundNS.Address"",
      * */
        static void Main(string[] args)
        {
            HttpClient client = GetClient();

            Query(client, "$metadata");

            Query(client, "Customers");

            Query(client, "UnboundFunction(p1=8,p2='abc11',location=@p)?@p={\"City\":\"Shanghai\"}");

            Query(client, "RetrieveCustomersByFilter(name='John',lastName='Alex',customerType=UnboundNS.CustomerType'Vip')");

            Query(client, "RetrieveCustomersByFilter(name='John',lastName='Alex',customerType=UnboundNS.CustomerType'Vip')?$filter=Id lt 3");
        }

        private static async void Query(HttpClient client, string uri)
        {
            string resqUri = "http://localhost/odata/" + uri;
            Console.WriteLine(resqUri);
            HttpResponseMessage response;

            try
            {
                response = await client.GetAsync(resqUri);
                string content = "";
                if (response.Content != null)
                {
                    content = await response.Content.ReadAsStringAsync();
                }

                Console.WriteLine("Status Code: [" + response.StatusCode + "], Content: \n" + content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static HttpClient GetClient()
        {
            HttpConfiguration config = new HttpConfiguration();
            config.MapODataServiceRoute("odata", "odata", EdmModelBuilder.EdmModel);
            return new HttpClient(new HttpServer(config));
        }
    }
}
