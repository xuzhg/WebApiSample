using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExpressionTreeToString;
using System.Reflection;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.AspNetCore.OData.Query;

namespace ODataQueryLinqExpressionBuilder
{
    internal class Program
    {
        public static IList<Customer> _customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "Peter", Age= 36,
                Location = new Address { City = "Redmond", Street = "156TH AVE"},
                Properties = new Dictionary<string, object> { { "MyData", 5.4f} } },
            new Customer { Id = 2, Name = "Sam", Age= 34, Location = new Address { City = "Shanghai", Street = "Zixin Rd"},
                Properties = new Dictionary<string, object> { { "MyData", 5.1f} } },
            new Customer
            {
                Id = 3,
                Name = "John",
                Age = 12,
                Location = new Address { City = "Beijing", Street = "Yunan Rd" },
                Properties = new Dictionary<string, object> { { "MyData", 4.1f } }
            },
             new Customer
            {
                Id = 4,
                Name = "Kerry",
                Age = 24,
                Location = new Address { City = "Seattle", Street = "145TH NE ST" },
                Properties = new Dictionary<string, object> { { "MyData", 6.1f } }
            },
             new Customer { Id = 5, Name = "Alex", Age = 78, Location = new Address { City = "Sammamish", Street = "225TH AVE" },

                Properties = new Dictionary<string, object> { { "MyData", 7.1f } } }
        };


        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("\nPlay $filter using F/f, Play $select using S/s:");
            string cmd = Console.ReadLine();
            if (cmd == "S" || cmd == "s")
            {
                SelectDemo.Execute();
                return;
            }
            ////foreach (var item in _customers.Where(a => a.Name == "Sam"))
            ////{
            ////    Console.WriteLine(item.Name);
            ////}

            ////_customers.Where((a, b) =>
            ////{
            ////    if (a.Name == "Sam")
            ////    {
            ////        Console.WriteLine($"{a.Name} at location {b}");
            ////        return true;
            ////    }

            ////    return false;
            ////});

            // Build the model
            IEdmModel model = GetEdmModel();

            while (true)
            {
                Console.Clear();

                Console.WriteLine("Sample Data:");
                Print(ConsoleColor.Green, _customers, true);

                // Or use dynamic property as : MyData gt 5.0
                Console.WriteLine("\nPlease input $filter (For example: Location/City eq 'Shanghai' or Q/q for quit):");
                cmd = Console.ReadLine();
                if (cmd == "q" || cmd == "Q")
                {
                    break;
                }

                // Query parser
                // "Location/City eq 'Shanghai'"
                // Age lt 30 
                FilterClause filter;
                try
                {
                    filter = ParseFilter(model, cmd);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n[Error]: {ex.Message},\nPress any key to try more...");
                    Console.ReadKey();
                    continue;
                }

                // Linq Expression
                QueryBinderContext context = new QueryBinderContext(model, new ODataQuerySettings(), typeof(Customer));
                FilterBinder binder = new FilterBinder();
                Expression filterExp = binder.BindFilter(filter, context);

                //FilterBinder2 filterBinder = new FilterBinder2(/*model*/);
                //Expression filterExp = filterBinder.Bind<Customer>(filter);

                string filterStr = filterExp.ToString("C#");
                Console.WriteLine($"\n\t==> {filterStr}\n");

                IEnumerable<Customer> filtered = Invoke(_customers, filterExp);

                Console.WriteLine("Filter Result:");
                Print(ConsoleColor.Blue, filtered);

                Console.WriteLine("Press any key to try more");
                Console.ReadKey();
            }

            // Invoke
            Console.WriteLine("Nice Job!");
        }


        public static void Print(ConsoleColor color, IEnumerable<Customer> customers, bool head = false)
        {
            ConsoleColor backColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            Console.WriteLine();
            if (head)
            {
                Console.WriteLine($"  | ID |      Name|  Age  |     City     |     Street    |");
                Console.WriteLine($"  --------------------------------------------------------");
            }
            foreach (var customer in customers)
            {
                Console.WriteLine($"  |  {customer.Id} |{customer.Name,10}|  {customer.Age}  |{customer.Location.City,15}|{customer.Location.Street,15}|");
            }
            Console.WriteLine();
            Console.ForegroundColor = backColor;
        }

        private static IEdmModel GetEdmModel()
        {
            //var model = new EdmModel();
            //var address = new EdmComplexType("NS", "Address");
            //address.AddStructuralProperty("City", EdmPrimitiveTypeKind.String);
            //address.AddStructuralProperty("Street", EdmPrimitiveTypeKind.String);
            //model.AddElement(address);

            //var customer = new EdmEntityType("NS", "Customer");
            //customer.AddKeys(customer.AddStructuralProperty("Id", EdmPrimitiveTypeKind.Int32));
            //customer.AddStructuralProperty("Name", EdmPrimitiveTypeKind.String);
            //customer.AddStructuralProperty("Age", EdmPrimitiveTypeKind.Int32);
            //customer.AddStructuralProperty("Location", new EdmComplexTypeReference(address, true));

            //model.AddElement(customer);

            //var container = new EdmEntityContainer("NS", "Default");
            //container.AddEntitySet("Customers", customer);
            //model.AddElement(container);
            //return model;

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            builder.EntityType<Customer>().Property(c => c.Name).IsRequired();
            return builder.GetEdmModel();
        }

        private static FilterClause ParseFilter(IEdmModel model, string filter)
        {
            var customers = model.EntityContainer.FindEntitySet("Customers");
            var customer = customers.EntityType();

            var parser = new ODataQueryOptionParser(model, customer, customers, new Dictionary<string, string>
            {
                {"$filter", filter},
            });

            return parser.ParseFilter();
        }

        private static IEnumerable<T> Invoke<T>(IEnumerable<T> source, Expression expression)
        {
            MethodInfo whereMethod = GetEnumerableWhere();

            whereMethod = whereMethod.MakeGenericMethod(typeof(T));

            Func<T, bool> function = (expression as LambdaExpression).Compile() as Func<T, bool>;
            return whereMethod.Invoke(null, new object[] { source, function }) as IEnumerable<T>;
        }

        private static MethodInfo GetEnumerableWhere()
        {
            // public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
            // public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
            var whereMethods = typeof(Enumerable).GetMethods().Where(x => x.Name == "Where");

            foreach (var whereMethod in whereMethods)
            {
                ParameterInfo[] parameters = whereMethod.GetParameters();
                if (parameters[1].ParameterType.GetGenericArguments().Length == 2)
                {
                    return whereMethod;
                }
            }

            throw new NotSupportedException();
        }
    }

    public class Address
    {
        public string City { get; set; }

        public string Street { get; set; }

        public override string ToString()
        {
            string cityStr = $"City: {City}";
            string streetStr = $"Street: {Street}";
            return $"{{{cityStr},{streetStr}}}";
        }
    }

    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Age { get; set; }

        public Address Location { get; set; }

        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
    }
}
