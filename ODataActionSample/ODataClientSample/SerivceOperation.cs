using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OData.Core;
using ODataClientSample.Extra;
using ODataClientSample.ODataActionSample;

namespace ODataClientSample
{
    public class SerivceOperation
    {
        private static Uri _serviceRoot = new Uri("http://localhost:33082/odata/");

        public static void TestStackOverflow_31393018()
        {
            // Customer #1
            Customer newCustomer = new Customer();
            newCustomer.Id = 19;
            newCustomer.Name = "Customer #19";
            newCustomer.Properties = new Dictionary<string, object>
            {
                {"IntProp", 9},
                {"DateTimeOffsetProp", new DateTimeOffset(2015, 7, 16, 1, 2, 3, 4, TimeSpan.Zero)},
                {"blah", "ha"}
            };

            AddCustomer(newCustomer);

            // Customer #2
            Customer newCustomer1 = new Customer();
            newCustomer1.Id = 20;
            newCustomer1.Name = "Customer #20";
            newCustomer1.Properties = new Dictionary<string, object>
            {
                {"IntProp", 10},
                {"dir", "north"}
            };

            AddCustomer(newCustomer1);

            // Update Customer #2
            // newCustomer1.Properties["dir"] = "south";

            // It seems there are errors to update. 
            UpdateCustomer(newCustomer1);
        }

        public static void AddCustomer(Customer customer)
        {
            Container container = new Container(_serviceRoot);
            container.Customers.ToList();

            try
            {
                AddCustomer(container, customer); // Register an action

                container.AddToCustomers(customer);

                container.SaveChanges(); // will call the registered actions
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void UpdateCustomer(Customer customer)
        {
            Container container = new Container(_serviceRoot);
            var customers = container.Customers.ToList();

            try
            {
                var cu = customers.First(c => c.Id == customer.Id);
                cu.Properties = new Dictionary<string, object>();
                cu.Properties["dir"] = "south";

                AddCustomer(container, cu); // Register an action

                container.UpdateObject(cu);

                container.SaveChanges(); // will call the registered actions
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void AddCustomer(Container container, Customer customer)
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
    }
}
