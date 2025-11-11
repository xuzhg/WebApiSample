using DeepUpdateTests_9x.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DeepUpdateTests_9x.Controllers
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
                        Orders = Enumerable.Range(0, 2).Select(e => new Order { Title = "abc" + e, Amount = 100 + e,
                            Items = new List<ListItem> {
                                new ListItem { Id = 199, Description = $"afecListItem199" },
                                new ListItem { Id = 299, Description = $"5dfListItem299" },
                                new ListItem { Id = 399, Description = $"ddfListItem399" } }}
                                ).ToArray()
                    },
                    new Customer
                    {
                        Name = "Sam",
                  
                        Orders = Enumerable.Range(0, 2).Select(e => new Order { Title = "xyz" + e, Amount = 200 + e,
                            Items = new List<ListItem> {
                                new ListItem { Id = 699, Description = $"klmListItem699" },
                                new ListItem { Id = 799, Description = $"efgListItem799" },
                                new ListItem { Id = 999, Description = $"dddListItem999" } } }
                                ).ToArray()
                    },
                    new Customer
                    {
                        Name = "Peter",
                        Orders = Enumerable.Range(0, 2).Select(e => new Order { Title = "ijk" + e, Amount = 300 + e,
                            Items = new List<ListItem> {
                                new ListItem { Id = 1299, Description = $"efgListItem1299" },
                                new ListItem { Id = 599, Description = $"ijkListItem599" },
                                new ListItem { Id = 899, Description = $"xzyListItem899" } }}
                                ).ToArray()
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

          //  delta.TryGetNestedPropertyValue("Orders", out object orders);

            IList<string> changeItemsInOrders = new List<string>();
            DeltaSet<Order> deltaSetOrders = ordersValue as DeltaSet<Order>;
            if (deltaSetOrders != null)
            {
                foreach (var item in deltaSetOrders)
                {
                    // Process each order item
                    switch (item.Kind)
                    {
                        case DeltaItemKind.DeletedResource:
                            break;

                        case DeltaItemKind.Resource:
                            break;

                        case DeltaItemKind.DeltaLink:
                            break;

                        default:
                            break;
                    }

                    changeItemsInOrders.Add(item.Kind.ToString());
                }
            }

            return Ok(changeItemsInOrders);
        }

        [HttpGet]
        public IActionResult GetOrdersFromCustomer(int key, ODataQueryOptions<Order> queryOptions)
        {
            if (queryOptions.RawValues.DeltaToken != null && queryOptions.RawValues.DeltaToken == "abcd")
            {
                var changesets = new DeltaSet<Order>();

                // First Order
                var changedOrder = new Delta<Order>();
                changesets.Add(changedOrder);

                // List Items 1 in first Order
                var listItemDelta = new Delta<ListItem>();
                listItemDelta.TrySetPropertyValue("Description", "ChangedDescription");
                var subListSet = new DeltaSet<ListItem>();
                subListSet.Add(listItemDelta);

                changedOrder.TrySetPropertyValue("Id", 121);
                changedOrder.TrySetPropertyValue("Items", subListSet);


                // Second Order
                var order2 = new DeltaDeletedResource<Order>();
                order2.Id = new Uri("http://localhost/odata/orders(3)");
                order2.Reason = Microsoft.OData.DeltaDeletedEntryReason.Deleted;
                changesets.Add(order2);

                return Ok(changesets);
            }
            return Ok();
        }

        [EnableQuery]
        public IActionResult Post([FromBody] Customer customer)
        {
            return Created(customer);
        }
    }
}
