using DeepUpdateTests.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OData.Edm;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace DeepUpdateTests.Controllers
{
    public class HistoryController : ODataController
    {
        // In-memory static list for demonstration. Replace with persistent storage as needed.
        private static readonly IList<History> _history;
        private static readonly IEdmModel _edmModel = EdmModelBuilder.GetEdmModel();
        private static readonly IEdmEntityType _customerEdmType = _edmModel.SchemaElements
    .OfType<IEdmEntityType>()
    .First(e => e.Name == nameof(Customer));

        static HistoryController()
        {
            var delta = GetDeltaObject();

            _history = new List<History>
            {
                new History
                {
                    Id = 1,
                    CustomerDelta = DeltaObjectSerializer.AsSerializedDeltaObject(delta)
                }
            };
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_history);
        }

        private static EdmDeltaEntityObject GetDeltaObject()
        {
            IEdmModel model = EdmModelBuilder.GetEdmModel();
            var customerType = model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "Customer");
            
            var changedCustomerObject = new EdmDeltaEntityObject(customerType);
            changedCustomerObject.TrySetPropertyValue("Id", 1);
            changedCustomerObject.TrySetPropertyValue("Name", "customer old name");

            var orderType = model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "Order");

            var changedOrders = new EdmChangedObjectCollection(orderType);

            var changeOrder1Object = new EdmDeltaEntityObject(orderType);
            changeOrder1Object.TrySetPropertyValue("Id", 1);
            changedOrders.Add(changeOrder1Object);

            var changedOrder2Object = new EdmDeltaDeletedEntityObject(orderType);
            changedOrder2Object.Id = "http://tempuri.org/Orders(2)";
            changedOrder2Object.TrySetPropertyValue("Id", 2);
            changedOrders.Add(changedOrder2Object);

            var changedOrderObject = new EdmDeltaEntityObject(orderType);
            changedOrderObject.TrySetPropertyValue("Id", 1);
            changedOrderObject.TrySetPropertyValue("Amount", 8);

            // Have nested
            var listItemType = model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "ListItem");
            var changedLists = new EdmChangedObjectCollection(listItemType);
            var changeList1Object = new EdmDeltaEntityObject(listItemType);
            changeList1Object.TrySetPropertyValue("Id", 1);
            changedLists.Add(changeList1Object);

            changedOrderObject.TrySetPropertyValue("Items", changedLists);

            changedOrders.Add(changedOrderObject);
            changedCustomerObject.TrySetPropertyValue("Orders", changedOrders);
            return changedCustomerObject;
        }
    }
}