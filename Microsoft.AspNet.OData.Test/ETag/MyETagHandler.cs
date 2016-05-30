using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.OData.Formatter;
using Microsoft.OData.Core;
using Microsoft.OData.Core.UriParser;

namespace Microsoft.AspNet.OData.Test.ETag
{
    // It's just for test.
    public class MyETagHandler : IETagHandler
    {
        public EntityTagHeaderValue CreateETag(IDictionary<string, object> properties)
        {
            Debug.Assert(properties != null);

            if (properties.Count == 0)
            {
                return null;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append('\"');// necessary
            bool firstProperty = true;
            foreach (var property in properties)
            {
                if (firstProperty)
                {
                    firstProperty = false;
                }
                else
                {
                    builder.Append("/");
                }

                builder.Append(property.Key).Append(",");
                string value = ODataUriUtils.ConvertToUriLiteral(property.Value, ODataVersion.V4);
                builder.Append(value);
            }

            builder.Append('\"'); // necessary
            string tag = builder.ToString();
            return new EntityTagHeaderValue(tag, isWeak: true);
        }

        public IDictionary<string, object> ParseETag(EntityTagHeaderValue etagHeaderValue)
        {
            string[] tags = etagHeaderValue.Tag.Trim('\"').Split('/');

            IDictionary<string, object> properties = new Dictionary<string, object>();
            for (int index = 0; index < tags.Length; index++)
            {
                string[] rawValues = tags[index].Split(',');

                object obj = ODataUriUtils.ConvertFromUriLiteral(rawValues[1], ODataVersion.V4);
                properties.Add(rawValues[0], obj);
            }

            return properties;
        }
    }
}
