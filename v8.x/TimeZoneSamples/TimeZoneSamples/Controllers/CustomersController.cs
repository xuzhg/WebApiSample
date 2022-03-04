using Microsoft.AspNetCore.Mvc;
using TimeZoneSamples.Models;

namespace TimeZoneSamples.Controllers
{
    public class CustomersController : ControllerBase
    {

        private static IList<Customer> _customers = new List<Customer>
        {
            new Customer
            {
                Id =1,
                Name = "UTC",
                CreatedDate = new DateTime(1978, 11, 15, 10, 20, 30, DateTimeKind.Utc),
                DeliverDate = DateTimeOffset.Parse("2014-12-16T01:02:03+8:00")
            },
            new Customer
            {
                Id =1,
                Name = "Local",
                CreatedDate = new DateTime(2018, 12, 4, 1, 4, 6, DateTimeKind.Local),
                DeliverDate = DateTimeOffset.Parse("2018-01-16T11:02:03+7:00")
            },
            new Customer
            {
                Id =1,
                Name = "Unspecified",
                CreatedDate = new DateTime(2022, 1, 3, 6, 3, 22, DateTimeKind.Unspecified),
                DeliverDate = DateTimeOffset.Parse("2014-12-16T01:02:03-2:00")
            }
        };

        public IActionResult Get()
        {
            return Ok(_customers);
        }


        /* Send a HttpPut request to: http://localhost:5185/odata/customers(1) 
         * using the following request payload:
{
   "Name": "UTC",
  "CreatedDate": "2020-11-15T01:02:03-02:00",
  "DeliverDate": "2014-12-16T01:02:03+08:00"
}
         */
        // You will get
        /*
{
    "@odata.context": "http://localhost:5185/odata/$metadata#Customers/$entity",
    "Id": 1,
    "Name": "UTC",
    "RawCreatedDate": "2020-11-14T19:02:03.0000000",
    "CreatedDate": "2020-11-14T19:02:03-08:00",
    "DeliverDate": "2014-12-16T01:02:03+08:00"
}
         */

        [HttpPut]
        public IActionResult Put(int key, [FromBody]Customer customer)
        {
            var existing = _customers.FirstOrDefault(c => c.Id == key);
            if (existing == null)
            {
                return NotFound($"Can't find customer with key={key}");
            }

            existing.CreatedDate = customer.CreatedDate;
            existing.DeliverDate = customer.DeliverDate;

            return Ok(existing);
        }
    }
}
