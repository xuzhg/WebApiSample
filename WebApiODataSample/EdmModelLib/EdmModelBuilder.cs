using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.OData.Builder;
using Microsoft.OData.Edm;

namespace EdmModelLib
{
    public class EdmModelBuilder
    {
        private static IEdmModel _edmModel { get; set; }

        public static IEdmModel EdmModel
        {
            get
            {
                if (_edmModel == null)
                {
                    _edmModel = GetEdmModel();
                }

                return _edmModel;
            }
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<Order>("Orders");
            return builder.GetEdmModel();
        }
    }
}
