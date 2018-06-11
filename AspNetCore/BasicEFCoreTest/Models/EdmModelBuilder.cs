using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicEFCoreTest.Models
{
    public static class EdmModelBuilder
    {
        private static IEdmModel _model;

        public static IEdmModel GetEdmModel()
        {
            if (_model == null)
            {
                var builder = new ODataConventionModelBuilder();
                builder.EntitySet<Customer>("Customers");
                builder.EntitySet<Order>("Orders");
                _model = builder.GetEdmModel();
            }

            return _model;
        }
    }
}
