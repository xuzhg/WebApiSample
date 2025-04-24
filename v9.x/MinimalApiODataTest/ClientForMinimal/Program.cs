// See https://aka.ms/new-console-template for more information


using Default;
using MinimalApiODataTest.Models;

Console.WriteLine("Please run the mini application first, then press any key to go...");
Console.ReadKey();
Console.WriteLine("\nList all the customers data:");

Container container = new Container(new Uri("http://localhost:5202/odata"));

IEnumerable<Customer> customers = await container.Customers.ExecuteAsync();

foreach (var customer in customers)
{
Console.WriteLine(customer);
}

namespace MinimalApiODataTest.Models
{
    public partial class Customer
    {
        public override string ToString()
        {
            return $"  {Id} -- '{Name}' likes '{FavoriteColor}', he/she lives '{HomeAddress}'";
        }
    }

    public partial class Address
    {
        public override string ToString()
        {
            return $"[ {Street} at {City} ]";
        }
    }
}