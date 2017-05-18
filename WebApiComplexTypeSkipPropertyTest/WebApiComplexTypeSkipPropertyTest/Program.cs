using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Formatter;
using System.Web.OData.Formatter.Deserialization;

namespace WebApiComplexTypeSkipPropertyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IEdmModel model = GetEdmModel();

            AddSkipPropertyAnnotation(model, typeof(Address));

            Console.WriteLine("---------Disable skip properties--------------\n");

            HttpClient client = GetClient(model, false);
            QueryMetadata(client);
            QueryCustomers(client);

            Console.WriteLine("---------Enable skip properties--------------\n");
            client = GetClient(model, true);
            QueryMetadata(client);
            QueryCustomers(client);
        }

        private static HttpClient GetClient(IEdmModel edmModel, bool enableSkip)
        {
            var config = new HttpConfiguration();

            if (enableSkip)
            {
                config.Formatters.InsertRange(0, ODataMediaTypeFormatters.Create(new MySerializerProvider(), new DefaultODataDeserializerProvider()));
            }

            config.MapODataServiceRoute("odata", "odata", edmModel);
            return new HttpClient(new HttpServer(config));
        }

        private static async void QueryMetadata(HttpClient client)
        {
            string req = "http://localhost/odata/$metadata";
            HttpResponseMessage resp = await client.GetAsync(req);
            Console.WriteLine(await resp.Content.ReadAsStringAsync());
        }

        private static async void QueryCustomers(HttpClient client)
        {
            string req = "http://localhost/odata/Customers";
            HttpResponseMessage resp = await client.GetAsync(req);
            Console.WriteLine(await resp.Content.ReadAsStringAsync());
        }

        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            return builder.GetEdmModel();
        }

        private static IEdmModel AddSkipPropertyAnnotation(IEdmModel model, Type clrType)
        {
            IEdmType edmType = GetEdmType(model, clrType);
            var properties = clrType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            var skipProperties = new List<string>();
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<SkipPropertyAttribute>();
                if (attribute == null)
                {
                    continue;
                }

                skipProperties.Add(property.Name);

                //IEdmProperty edmProperty = FindEdmPropertyByName(edmType, property.Name);
                //model.SetAnnotationValue<SkipPropertyAnnotation>(edmProperty, new ClrTypeAnnotation(clrType));
            }

            if (skipProperties.Any())
            {
                model.SetAnnotationValue(edmType, new SkipPropertyAnnotation(skipProperties));
            }

            return model;
        }

        private static IEdmType GetEdmType(IEdmModel model, Type clrType)
        {
            var edmTypes = model.SchemaElements.OfType<IEdmStructuredType>();
            foreach (var edmType in edmTypes)
            {
                ClrTypeAnnotation annotation = model.GetAnnotationValue<ClrTypeAnnotation>(edmType);
                if (annotation != null)
                {
                    if (annotation.ClrType == clrType)
                    {
                        return edmType;
                    }
                }
            }

            return null;
        }

        public static IEdmProperty FindEdmPropertyByName(IEdmType edmType, string name)
        {
            IEdmStructuredType structureType;
            if (edmType is IEdmEntityType)
            {
                structureType = (IEdmEntityType)edmType;
            }
            else if (edmType is IEdmComplexType)
            {
                structureType = (IEdmComplexType)edmType;
            }
            else
            {
                return null;
            }

            return structureType.FindProperty(name);
        }
    }

    public class CustomersController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            Customer c = new Customer
            {
                Id = 1,
                Location = new Address
                {
                    Street = "156th AVE NE",
                    City = "Redmond",
                    Country = "US"
                }
            };

            return Ok(new[] { c });
        }
    }


    public class Customer
    {
        public int Id { get; set; }
        public Address Location { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }

        [SkipProperty]
        public string Country { get; set; }
    }
}
