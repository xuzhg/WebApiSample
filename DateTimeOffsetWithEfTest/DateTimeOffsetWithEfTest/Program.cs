using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace DateTimeOffsetWithEfTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CustomerContext>());
            CreateDatabase();
            Console.WriteLine("Database Done!");

            var client = GetClient();

            //Query(client, "Customers");

            Query(client, "Customers?$filter=Birthday eq 2016-06-12T07:08:00Z"); // only return customer #3

            Query(client, "Customers?$filter=Birthday gt 2016-06-12T07:08:00Z"); // only return customer #4

            Query(client, "Customers?$filter=ExpirationDate lt 3009-06-23T10:11:12Z"); // only return customer #1, 3, 4

            Query(client, "Customers?$filter=ExpirationDate eq 3009-06-23T10:11:12Z"); // only return customer #2

            Console.ReadKey();
        }

        private static void Query(HttpClient client, string uri)
        {
            string requestUri = "http://localhost/odata/" + uri;
            Console.WriteLine("\n[New Request: GET ] " + requestUri);
            Console.Write("[Response: ] ");

            HttpResponseMessage response = client.GetAsync(requestUri).Result;
            Console.WriteLine(response.StatusCode);
            Console.WriteLine();
            if (response.Content != null)
            {
                if (response.Content.Headers.ContentType.MediaType.Contains("json"))
                {
                    Console.WriteLine(JObject.Parse(response.Content.ReadAsStringAsync().Result));
                }
                else
                {
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
            }
        }

        private static HttpClient GetClient()
        {
            var c = new HttpConfiguration();
            c.SetTimeZoneInfo(TimeZoneInfo.Utc);
            c.MapODataServiceRoute("odata", "odata", GetEdmModel());
            return new HttpClient(new HttpServer(c));
        }

        private static IEdmModel GetEdmModel()
        {
            var b = new ODataConventionModelBuilder();
            b.EntitySet<Customer>("Customers");
            return b.GetEdmModel();
        }

        private static void CreateDatabase()
        {
            CustomerContext db = new CustomerContext();

            if (db.Customers.Any())
            {
                return;
            }

            Customer customer = new Customer();
            customer.Id = 1;
            customer.Name = "Sam";
            customer.Birthday = new DateTime(1978, 2, 15, 7, 8, 9, DateTimeKind.Utc);
            customer.ExpirationDate = new DateTimeOffset(2978, 2, 15, 10, 11, 12, TimeSpan.Zero);
            db.Customers.Add(customer);

            customer = new Customer();
            customer.Id = 2;
            customer.Name = "Kerry";
            customer.Birthday = new DateTime(2009, 12, 15, 7, 8, 9, DateTimeKind.Utc);
            customer.ExpirationDate = new DateTimeOffset(3009, 6, 23, 10, 11, 12, TimeSpan.Zero);
            db.Customers.Add(customer);

            customer = new Customer();
            customer.Id = 3;
            customer.Name = "Tony";
            customer.Birthday = new DateTime(2016, 6, 12, 7, 8, 9, DateTimeKind.Utc);
            customer.ExpirationDate = new DateTimeOffset(2116, 1, 2, 10, 11, 12, TimeSpan.Zero);
            db.Customers.Add(customer);

            customer = new Customer();
            customer.Id = 4;
            customer.Name = "Peter";
            customer.Birthday = new DateTime(2020, 12, 2, 7, 8, 9, DateTimeKind.Utc);
            customer.ExpirationDate = new DateTimeOffset(2120, 11, 22, 10, 11, 12, TimeSpan.Zero);
            db.Customers.Add(customer);

            db.SaveChanges();
        }
    }
}
