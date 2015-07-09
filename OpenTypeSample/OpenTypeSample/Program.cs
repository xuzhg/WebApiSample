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
using Microsoft.OData.Edm.Csdl;

namespace OpenTypeSample
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = GetClient();

            bool isContinue = false;
            do
            {
                isContinue = false;
                Console.WriteLine("Please input what you want to do:");
                Console.WriteLine("\t[1]. Query metadata");
                Console.WriteLine("\t[2]. Query entity set");
                Console.WriteLine("\t[3]. Query dynamic property using convention routing");
                Console.WriteLine("\t[4]. Query dynamic property using attribute routing");
                Console.WriteLine();

                string key = Console.ReadLine();
                switch (key)
                {
                    case "1":
                        QueryMetadata(client);
                        isContinue = true;
                        break;

                    case "2":
                        QueryBooks(client);
                        isContinue = true;
                        break;

                    case "3":
                        QueryDynamics(client);
                        isContinue = true;
                        break;

                    case "4":
                        QueryDynamics2(client);
                        isContinue = true;
                        break;
                    default:
                        isContinue = false;
                        break;
                }


            } while (isContinue);


            
        }

        private static async void QueryMetadata(HttpClient client)
        {
            string req = "http://localhost/odata/$metadata";

            HttpResponseMessage resp = await client.GetAsync(req);

            Console.WriteLine(await  resp.Content.ReadAsStringAsync());
        }

        private static async void QueryBooks(HttpClient client)
        {
            string req = "http://localhost/odata/Books";

            HttpResponseMessage resp = await client.GetAsync(req);

            Console.WriteLine(await resp.Content.ReadAsStringAsync());
        }

        private static async void QueryDynamics(HttpClient client)
        {
            string req = "http://localhost/odata/Books('978-0-7356-7942-9')/Authors";

            HttpResponseMessage resp = await client.GetAsync(req);

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine(await resp.Content.ReadAsStringAsync());
            }
            else
            {
                Console.WriteLine(resp.StatusCode);
            }
        }

        private static async void QueryDynamics2(HttpClient client)
        {
            string req = "http://localhost/odata/Books('978-0-7356-7942-9')/Press/Address";

            HttpResponseMessage resp = await client.GetAsync(req);

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine(await resp.Content.ReadAsStringAsync());
            }
            else
            {
                Console.WriteLine(resp.StatusCode);
            }
        }

        private static HttpClient GetClient()
        {
            var configuration = new HttpConfiguration();

            configuration.MapODataServiceRoute("odata", "odata", GetEdmModel());

            return new HttpClient(new HttpServer(configuration));
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();

            builder.EntitySet<Book>("Books");
            builder.EntityType<Book>().HasKey(c => c.ISBN);
            builder.EntityType<Customer>();
            return builder.GetEdmModel();
        }
    }
}
