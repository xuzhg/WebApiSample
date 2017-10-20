using Microsoft.OData.UriParser;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.OData.Extensions;

using ODataPath = System.Web.OData.Routing.ODataPath;

namespace WebApi6xFeaturesTest.Dynamic
{
    public class ODataDynamicValueMediaTypeMapping : MediaTypeMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODataPrimitiveValueMediaTypeMapping"/> class.
        /// </summary>
        public ODataDynamicValueMediaTypeMapping()
            : base("text/plain")
        {
        }

        public override double TryMatchMediaType(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return (IsRawValueRequest(request) && IsDynamicProperty(request)) ? 1 : 0;
        }

        internal static bool IsRawValueRequest(HttpRequestMessage request)
        {
            ODataPath path = request.ODataProperties().Path;
            return path != null && path.Segments.LastOrDefault() is ValueSegment;
        }

        private static bool IsDynamicProperty(HttpRequestMessage request)
        {
            ODataPath odataPath = request.ODataProperties().Path;
            if (odataPath == null || odataPath.Segments.Count < 2)
            {
                return false;
            }
            return odataPath.Segments[odataPath.Segments.Count - 2] is DynamicPathSegment;
        }
    }
}
