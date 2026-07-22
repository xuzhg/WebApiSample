using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ODataService.Models;

namespace ODataService.Controllers;

public class CustomersController : ODataController
{
    private static readonly List<Customer> Customers = DataSource.GetCustomers();

    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(Customers);
    }

    [EnableQuery]
    public IActionResult Get(int key)
    {
        Customer customer = Customers.FirstOrDefault(c => c.Id == key);
        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }
}

internal static class DataSource
{
    public static List<Customer> GetCustomers()
    {
        return new List<Customer>
        {
            new Customer
            {
                Id = 1,
                Name = "Alice",
                City = "Redmond",
                Category = CustomerCategory.Premium,
                //RegisteredAt = new DateTime(2021, 3, 15, 9, 30, 0, DateTimeKind.Local),
                //LastOrderDate = new DateTime(2024, 11, 2, 14, 5, 0, DateTimeKind.Local),
                RegisteredAt = new DateTime(2021, 3, 15, 9, 30, 0),
                LastOrderDate = new DateTime(2024, 11, 2, 14, 5, 0),
                Orders = new List<Order>
                {
                    new Order { Id = 1, Product = "Keyboard", Amount = 49.99m },
                    new Order { Id = 2, Product = "Mouse", Amount = 19.99m },
                }
            },
            new Customer
            {
                Id = 2,
                Name = "Bob",
                City = "Seattle",
                Category = CustomerCategory.Standard,
                RegisteredAt = new DateTime(2022, 7, 1, 8, 0, 0, DateTimeKind.Local),
                LastOrderDate = new DateTime(2024, 9, 20, 10, 45, 0, DateTimeKind.Local),
                Orders = new List<Order>
                {
                    new Order { Id = 3, Product = "Monitor", Amount = 199.99m },
                }
            },
            new Customer
            {
                Id = 3,
                Name = "Charlie",
                City = "Redmond",
                Category = CustomerCategory.Vip,
                RegisteredAt = new DateTime(2023, 1, 10, 12, 0, 0, DateTimeKind.Local),
                LastOrderDate = null,
                Orders = new List<Order>()
            },
        };
    }
}
