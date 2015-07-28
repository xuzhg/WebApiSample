using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Xml;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using Owin;

namespace ReadEdmxSelfHostOWin
{
    public class StartUp
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.MapODataServiceRoute("odata1", "odata1", GetEdmModel());

            config.MapODataServiceRoute("odata2", "odata2", ReadEdmModel());

            appBuilder.UseWebApi(config);
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Protocol>("Protocols");
            builder.EntitySet<Implementation>("Implementations");
            builder.EntitySet<Field>("Fields");
            return builder.GetEdmModel();
        }

        private static IEdmModel ReadEdmModel()
        {
            using (Stream stream = GetStream("EdmModel.xml"))
            {
                IEnumerable<EdmError> errors;
                Debug.Assert(stream != null, "EdmModel.xml: stream!=null");
                IEdmModel model;
                if (EdmxReader.TryParse(XmlReader.Create(stream), out model, out errors))
                {
                    return model;
                }
            }

            return null;
        }

        private static Stream GetStream(string fileName)
        {
            const string projectDefaultNamespace = "ReadEdmxSelfHostOWin";
            const string pathSeparator = ".";
            string path = projectDefaultNamespace + pathSeparator + fileName;

            Stream stream = typeof(StartUp).Assembly.GetManifestResourceStream(path);

            if (stream == null)
            {
                string message = String.Format("The embedded resource '{0}' was not found.", path);
                throw new FileNotFoundException(message, path);
            }

            return stream;
        }
    }
}
