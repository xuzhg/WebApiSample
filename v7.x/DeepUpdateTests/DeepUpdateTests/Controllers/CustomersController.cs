using DeepUpdateTests.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OData.Edm;

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
                        Orders = Enumerable.Range(0, 2).Select(e => new Order { Title = "xyz" + e, Amount = 200 + e, List = new List<ListItem> () {new ListItem { isComplete = true } } }).ToList()
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

        [HttpGet]
        [ODataRoute("Customers({customerId})/orders")]
        public IActionResult GetOrdersForCustomer([FromODataUri] string customerId, ODataQueryOptions<Order> options)
        {
            if (options != null && options.RawValues != null && options.RawValues.DeltaToken == "abcd")
            {
                // in the > 8.x version you can do as follows
                var changeSetDeltaSet = new DeltaSet<Order>(new List<string> { "Id" });

                var changedOrder = new Delta<Order>();

                changeSetDeltaSet.Add(changedOrder);

                var subListSet = new DeltaSet<ListItem>(new List<string> { "Id" });
                subListSet.Add(new Delta<ListItem>
                {

                });
                changedOrder.TrySetPropertyValue("Id", 121);
                changedOrder.TrySetPropertyValue("List", subListSet);


                // changeSetDeltaSet.Add(subListSet);
                return Ok(changeSetDeltaSet);

                //IEdmModel model = Request.GetModel();
                //var orderType = model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "Order");

                //var changedOrders = new EdmChangedObjectCollection(orderType);

                //var changeOrder1Object = new EdmDeltaEntityObject(orderType);
                //changeOrder1Object.TrySetPropertyValue("Id", 1);
                //changedOrders.Add(changeOrder1Object);

                //var changedOrder2Object = new EdmDeltaDeletedEntityObject(orderType);
                //changedOrder2Object.Id = "http://tempuri.org/Orders(2)";
                //changedOrder2Object.TrySetPropertyValue("Id", 2);
                //changedOrders.Add(changedOrder2Object);

                //var changedOrderObject = new EdmDeltaEntityObject(orderType);
                //changedOrderObject.TrySetPropertyValue("Id", 1);
                //changedOrderObject.TrySetPropertyValue("Amount", 8);

                //// Have nested
                //var listItemType = model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "ListItem");
                //var changedLists = new EdmChangedObjectCollection(listItemType);
                //var changeList1Object = new EdmDeltaEntityObject(listItemType);
                //changeList1Object.TrySetPropertyValue("Id", 1);
                //changedLists.Add(changeList1Object);

                //changedOrderObject.TrySetPropertyValue("List", changedLists);

                //changedOrders.Add(changedOrderObject);

                //return Ok(changedOrders);
            }

            // handle read
            var orders = _customers.SelectMany(c => c.Orders);
            IQueryable<Order> queryOrders = orders.AsQueryable();
            options.ApplyTo(queryOrders);
            return Ok(queryOrders);
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
