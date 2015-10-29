using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.OData.Builder;
using Microsoft.OData.Edm;

namespace ODataQueryNodeParserTest.Models
{
    public class EdmModelBuilder
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
            return builder.GetEdmModel();
        }
    }
}
