using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;

namespace DeepUpdateTests.Models
{
    public static class EdmModelBuilder
    {
        private static IEdmModel _edmModel;

        public static IEdmModel GetEdmModel()
        {
            if (_edmModel == null)
            {
                var builder = new ODataConventionModelBuilder();
                builder.EntitySet<Customer>("Customers");
                builder.EntitySet<Order>("Orders");
                builder.EntityType<ListItem>();

                _edmModel = builder.GetEdmModel();
            }

            return _edmModel;
        }
    }

    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual Order[] Orders { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }

        public int Amount { get; set; }

        public string Title { get; set; }

        [Contained]
        public IList<ListItem> Items { get; set; }
    }

    public class ListItem
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }
}
