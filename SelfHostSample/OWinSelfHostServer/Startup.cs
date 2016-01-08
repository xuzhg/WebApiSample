using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Formatter;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;
using Microsoft.OData.Core;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Library;
using Owin;

namespace OWinSelfHostServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.Formatters.Clear();
            config.Formatters.AddRange(ODataMediaTypeFormatters.Create());

            config.MapODataServiceRoute("odata", "odata", GetEdmModel());

            IContentNegotiator originalContentNegotiator = config.Services.GetContentNegotiator();
            IContentNegotiator contentNegotiator = new MyRequestContentNegotiator(originalContentNegotiator);
            config.Services.Replace(typeof(IContentNegotiator), contentNegotiator);

            appBuilder.UseWebApi(config);
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            return builder.GetEdmModel();
        }
    }

    internal class MyRequestContentNegotiator : IContentNegotiator
    {
        private IContentNegotiator _innerContentNegotiator;

        public MyRequestContentNegotiator(IContentNegotiator innerContentNegotiator)
        {
            _innerContentNegotiator = innerContentNegotiator;
        }

        public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            MediaTypeHeaderValue mediaType = request.Content == null ? null : request.Content.Headers.ContentType;

            if (request.ODataProperties().Model == null)
            {
                request.ODataProperties().Model = new EdmModel();
            }

            List<MediaTypeFormatter> perRequestFormatters = new List<MediaTypeFormatter>();
            foreach (MediaTypeFormatter formatter in formatters)
            {
                if (formatter != null)
                {
                    perRequestFormatters.Add(formatter.GetPerRequestFormatterInstance(type, request, mediaType));
                }
            }

            return _innerContentNegotiator.Negotiate(type, request, perRequestFormatters);
        }
    }
}
