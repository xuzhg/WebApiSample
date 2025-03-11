using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace issue1432DollarIt.Controllers;

public class CustomersController : ODataController
{
    private static IList<Customer> Customers = new List<Customer>
    {
        new Customer
        {
            Id = 1,
            Name = "Sam",
            //MailAddresses = new List<Address>
            //{
            //    new Address { City = "Remond", Street = "145TH" },
            //    new Address { City = "Remond", Street = "125TH" },
            //    new Address { City = "Remond", Street = "115TH" }
            //},
            Emails = ["abc.org", "efg.com", "xyg.com"],
            Address = new Address { City = "Remond", Street = "120TH AVE" },
            Orders = new List<Order>
            {
                new Order { Id = 11, Price = 8, ShipTo = new Address { City = "Remond", Street = "120TH AVE" } },
                new Order { Id = 12, Price = 43, ShipTo = new Address { City = "Issaqu", Street = "145TH AVE" } },
                new Order { Id = 13, Price = 18, ShipTo = new Address { City = "Bellevue", Street = "10TH AVE" } },
            }
        },
        new Customer
        {
            Id = 2,
            Name = "Xu",
            //MailAddresses = new List<Address>
            //{
            //    new Address { City = "Issaqu", Street = "15TH" },
            //    new Address { City = "Issaqu", Street = "25TH" },
            //    new Address { City = "Issaqu", Street = "35TH" }
            //},
            Emails = ["123.org", "456.com", "789.com"],
            Address = new Address { City = "Bellevue", Street = "10TH AVE" },
            Orders = new List<Order>
            {
                new Order { Id = 21, Price = 86, ShipTo = new Address { City = "Remond", Street = "120TH AVE" } },
                new Order { Id = 22, Price = 13, ShipTo = new Address { City = "Bellevue", Street = "10TH AVE" } },
                new Order { Id = 23, Price = 11, ShipTo = new Address { City = "Issaqu", Street = "145TH AVE" } },
            }
        }
    };

    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(Customers);
    }


    [EnableQuery]
    public IActionResult Get(int key)
    {
        Customer c = Customers.FirstOrDefault(a => a.Id == key);
        if (c == null)
        {
            return NotFound();
        }

        return Ok(c);
    }

    [EnableQuery]
    public IActionResult GetEmails(int key)
    {
        Customer c = Customers.FirstOrDefault(a => a.Id == key);
        if (c == null)
        {
            return NotFound();
        }

        return Ok(c.Emails);
    }
}
