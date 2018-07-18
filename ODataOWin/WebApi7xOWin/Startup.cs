using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.OData.Edm;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi7xOWin
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration
            {
                IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always
            };

            config.MapODataServiceRoute("odata", "odata", GetEdmModel());


            appBuilder.UseWebApi(config);
        }

        private static IEdmModel GetEdmModel()
        {
            var b = new ODataConventionModelBuilder();
            return b.GetEdmModel();
        }
    }
}
