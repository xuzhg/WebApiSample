using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OData.Core;
using Microsoft.OData.Core.UriParser;
using ODataClientSample.Extra;
using ODataClientSample.ODataActionSample;

namespace ODataClientSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\t************Query customers before insert.************\t");
            QueryCustomers();

            PostCustomer();

            Console.WriteLine("\t************Query customers After insert.************\t");
            QueryCustomers();

            Console.ReadKey();
        }

        private static void PostCustomer()
        {
            Container container = new Container(new Uri("http://localhost:33082/odata/"));
            var customers = container.Customers.ToList();

            Address address = new Address();
            address.City = "Beijing";
            address.Street = "ChangAn Rd";
            address.Postcode = 1001100;

            int count = customers.Count;
            Customer newCustomer = new Customer();
            newCustomer.Id = count + 1;
            newCustomer.Name = "New Customer";
            newCustomer.Email = "happy@abc.com";
            newCustomer.Age = 37;
            newCustomer.Birthday = new DateTimeOffset(2015, 7, 15, 1, 2, 3, 4, TimeSpan.Zero);
            newCustomer.Address = address;

            newCustomer.Properties = new Dictionary<string, object>
            {
                {"IntProp", 9},
                {"DateTimeOffsetProp", new DateTimeOffset(2015, 7, 16, 1, 2, 3, 4, TimeSpan.Zero)}
            };

            container.Configurations.RequestPipeline.OnEntryStarting(args =>
            {
                foreach (var property in newCustomer.Properties)
                {
                    args.Entry.AddProperties(new ODataProperty
                    {
                        Name = property.Key,
                        Value = property.Value // for enum, complex type, should to create ODataEnumValue and ODataComplexValue.
                    });
                }
            });

            Console.WriteLine("!!! Start to insert a customer.");

            container.AddToCustomers(newCustomer);
            container.SaveChanges();

            Console.WriteLine("!!! Done! Insert a customer finished.");
        }

        private static void QueryCustomers()
        {
            Container container = new Container(new Uri("http://localhost:33082/odata/"));
            var customers = container.Customers.ToList();
            Console.WriteLine("Customer Count: [" + customers.Count + "]");

            foreach (Customer customer in customers)
            {
                Console.WriteLine("Customer #" + customer.Id);

                Console.WriteLine("\tName:" + customer.Name);
                Console.WriteLine("\tSalary:" + customer.Salary);
                Console.WriteLine("\tEmail:" + customer.Email);
                Console.WriteLine("\tAge:" + customer.Age);
                

                if (customer.Address != null)
                {
                    Console.Write("\tAddress: " + customer.Address.City);
                    if (customer.Address.Street != null)
                    {
                        Console.Write("," + customer.Address.Street);
                    }

                    if (customer.Address.Postcode != null)
                    {
                        Console.Write("," + customer.Address.Postcode);
                    }

                    Console.WriteLine();
                }

                if (customer.Birthday != null)
                {
                    Console.WriteLine("\tBirthday:" + customer.Birthday);
                }
            }
        }
    }

    public static class EntensionMethods
    {
        public static void AddProperties(this ODataEntry entry, params ODataProperty[] properties)
        {
            var odataProps = new List<ODataProperty>(entry.Properties);

            odataProps.AddRange(properties);

            entry.Properties = odataProps;
        }
    }
}
