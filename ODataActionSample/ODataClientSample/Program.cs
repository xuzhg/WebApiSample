using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OData.Client;
using Microsoft.OData.Core;
using Microsoft.OData.Core.UriParser;
using ODataClientSample.Extra;
using ODataClientSample.ODataActionSample;

namespace ODataClientSample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Query();
            
            SerivceOperation.TestStackOverflow_31393018();

            Console.WriteLine("\t************Query customers after insert.************\t");
            Query();
            // Test_SO_31393018();

            /*
            Console.WriteLine("\t************Query customers before insert.************\t");
            QueryCustomers();

            PostCustomer();

            Console.WriteLine("\t************Query customers After insert.************\t");
            QueryCustomers();*/

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
                        Value = property.Value
                        // for enum, complex type, should to create ODataEnumValue and ODataComplexValue.
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

        private static void Query()
        {
            string req = "http://localhost:33082/odata/Customers";
            WebRequest request = WebRequest.Create(req);
            WebResponse response = request.GetResponse();
            Stream receiveStream = response.GetResponseStream();

            if (receiveStream != null)
            {
                StreamReader sr = new StreamReader(receiveStream);
                string result = sr.ReadToEnd();
                Console.WriteLine(result);
            }
        }

        private static IDictionary<string, object> _dic;
        private static void Test_SO_31393018()
        {
            Container container = new Container(new Uri("http://localhost:33082/odata/"));
            //container.Customers.ToList();
            Customer newCustomer = new Customer();
            newCustomer.Id = 19;
            newCustomer.Properties = new Dictionary<string, object>
            {
                {"IntProp", 9},
                {"DateTimeOffsetProp", new DateTimeOffset(2015, 7, 16, 1, 2, 3, 4, TimeSpan.Zero)},
                {"blah", "ha"}
            };
            //_dic = newCustomer.Properties;
            try
            {
                //addCustomer(container, _dic);
                addCustomer(container, newCustomer);
                container.AddToCustomers(newCustomer);
                container.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Customer newCustomer1 = new Customer();
            newCustomer1.Id = 20;
            newCustomer1.Properties = new Dictionary<string, object>
            {
                {"IntProp", 10},
                {"dir", "north"}
            };

            //_dic = newCustomer1.Properties;
            addCustomer(container, newCustomer1);
            container.AddToCustomers(newCustomer1);
            container.SaveChanges();

            newCustomer1.Properties["dir"] = "south";
            container.UpdateObject(newCustomer1);
            container.SaveChanges();
            Console.ReadKey();
        }

        private static void addCustomer(Container container, Customer customer)
        {
            container.Configurations.RequestPipeline.OnEntryStarting(args =>
            {
                foreach (var property in customer.Properties)
                {
                    args.Entry.AddProperties(new ODataProperty
                    {
                        Name = property.Key,
                        Value = property.Value // for enum, complex type, should to create ODataEnumValue and ODataComplexValue.
                    });
                }
            });
        }

        private static void addCustomer(Container container, IDictionary<string, object> properties)
        {
            container.Configurations.RequestPipeline.OnEntryStarting(args =>
            {
                foreach (var property in properties)
                {
                    if (args.Entry.Properties.Any(e => e.Name == property.Key))
                    {
                        continue;
                    }

                    args.Entry.AddProperties(new ODataProperty
                    {
                        Name = property.Key,
                        Value = property.Value // for enum, complex type, should to create ODataEnumValue and ODataComplexValue.
                    });
                }
            });
        }
    }

    public static class ExtensionMethods
    {
        public static void AddProperties(this ODataEntry entry, params ODataProperty[] properties)
        {
            var odataProps = new List<ODataProperty>(entry.Properties);

            odataProps.AddRange(properties);

            entry.Properties = odataProps;
        }
    }
}
