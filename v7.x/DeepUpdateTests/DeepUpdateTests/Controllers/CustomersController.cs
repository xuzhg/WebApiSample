using DeepUpdateTests.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace DeepUpdateTests.Controllers
{
    public class CustomersController : ODataController
    {
        private readonly static IList<Customer> _customers;

        static CustomersController()
        {
            _customers = new List<Customer>
                {
                    new Customer
                    {
                        Name = "Jonier",
                      //  Order = new Order { Title = "104m" },
                        Orders = Enumerable.Range(0, 2).Select(e => new Order { Title = "abc" + e, Amount = 100 + e }).ToList()
                    },
                    new Customer
                    {
                        Name = "Sam",
                        //HomeAddress = new Address { City = "Bellevue", Street = "Main St NE"},
                        //FavoriteAddresses = new List<Address>
                        //{
                        //    new Address { City = "Red4ond", Street = "456 AVE NE"},
                        //    new Address { City = "Re4d", Street = "51 NE"},
                        //},
                        //Order = new Order { Title = "Zhang" },
                        Orders = Enumerable.Range(0, 2).Select(e => new Order { Title = "xyz" + e, Amount = 200 + e }).ToList()
                    },
                    new Customer
                    {
                        Name = "Peter",
                        //HomeAddress = new Address {  City = "Hollewye", Street = "Main St NE"},
                        //FavoriteAddresses = new List<Address>
                        //{
                        //    new Address { City = "R4mond", Street = "546 NE"},
                        //    new Address { City = "R4d", Street = "546 AVE"},
                        //},
                        //Order = new Order { Title = "Jichan" },
                        Orders = Enumerable.Range(0, 2).Select(e => new Order { Title = "ijk" + e, Amount = 300 + e }).ToList()
                    },
                };
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_customers);
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(_customers.FirstOrDefault(c => c.Id == key));
        }

        [EnableQuery]
        public IActionResult Patch(int key, Delta<Customer> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            delta.TryGetPropertyType("Orders", out Type ordersType);

            delta.TryGetPropertyValue("Orders", out object ordersValue);

            delta.TryGetNestedPropertyValue("Orders", out object orders);

            IList<string> changeItemsInOrders = new List<string>();
            DeltaSet<Order> deltaSetOrders = orders as DeltaSet<Order>;
            if (deltaSetOrders != null)
            {
                foreach (var item in deltaSetOrders)
                {
                    // Process each order item
                    switch (item.DeltaKind)
                    {
                        case EdmDeltaEntityKind.DeletedEntry:
                            break;

                        case EdmDeltaEntityKind.Entry:
                            break;

                        case EdmDeltaEntityKind.LinkEntry:
                            break;

                        default:
                            break;
                    }

                    changeItemsInOrders.Add(item.DeltaKind.ToString());
                }
            }

            return Ok(changeItemsInOrders);

            // return Ok(_customers.FirstOrDefault(c => c.Id == key));
        }

        /// <summary>
        /// If testing in IISExpress with the POST request to: http://localhost:2087/test/my/a/Customers
        /// Content-Type : application/json
        /// {
        ///    "Name": "Jonier","
        /// }
        /// 
        /// Check the reponse header, you can see 
        /// "Location" : "http://localhost:2087/test/my/a/Customers(0)"
        /// </summary>
        [EnableQuery]
        public IActionResult Post([FromBody] Customer customer)
        {
            return Created(customer);
        }
    }
}
