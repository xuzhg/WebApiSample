using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.OData.Builder;
using Microsoft.OData.Edm;

namespace WebApiWithEFSample
{
    public class EdmModelBuilder
    {
        private static IEdmModel _edmModel;
        public static IEdmModel GetEdmModel()
        {
            if (_edmModel != null)
            {
                return _edmModel;
            }

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Person>("People");

            // add action
            builder.Action("AddPerson");
            //
            _edmModel = builder.GetEdmModel();
            return _edmModel;
        }
    }
}
