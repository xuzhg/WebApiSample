using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.OData.Builder;
using Microsoft.OData.Edm;

namespace LowerCamelCaseSample.Model
{
    public class EdmModelBuilder
    {
        private static IEdmModel _edmModel1;
        private static IEdmModel _edmModel2;

        public static IEdmModel GetEdmModel()
        {
            if (_edmModel1 != null)
            {
                return _edmModel1;
            }

            var builder = GetBuilder();
            _edmModel1 = builder.GetEdmModel();
            return _edmModel1;
        }

        public static IEdmModel GetEdmModelLowerCamelCase()
        {
            if (_edmModel2 != null)
            {
                return _edmModel2;
            }

            var builder = GetBuilder();

            builder.EnableLowerCamelCase(); // not support for type name
            builder.ModelAliasingEnabled = true;// Not support for enum
            builder.EnumType<Color>().Name = "color";
            builder.EnumType<ServiceStatus>().Name = "serviceStatus";

            _edmModel2 = builder.GetEdmModel();
            return _edmModel2;
        }

        public static ODataConventionModelBuilder GetBuilder()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<Order>("Orders");
            return builder;
        }
    }
}
