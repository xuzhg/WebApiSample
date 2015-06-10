using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.OData.Builder;
using Microsoft.OData.Edm;
using UnboundNS;

namespace UnboundFunctionConventionRouting.Models
{
    public static class EdmModelBuilder
    {
        private static IEdmModel _edmModel;

        public static IEdmModel EdmModel
        {
            get
            {
                if (_edmModel == null)
                {
                    _edmModel = BuildEdmModel();
                }

                return _edmModel;
            }
        }

        private static IEdmModel BuildEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            builder.ComplexType<Address>();

            var function = builder.Function("UnboundFunction").Returns<string>();
            function.Parameter<int>("p1");
            function.Parameter<string>("p2");
            function.Parameter<Address>("location");

            function = builder.Function("ConventionUnboundFunction").Returns<string>();
            function.Parameter<int>("p1");
            function.Parameter<string>("p2");
            function.Parameter<Address>("location");

            function = builder.Function("RetrieveCustomersByFilter").ReturnsCollectionFromEntitySet<Customer>("Customers");
            function.Parameter<string>("name");
            function.Parameter<string>("lastName");
            function.Parameter<CustomerType>("customerType");

            return builder.GetEdmModel();
        }
    }
}
