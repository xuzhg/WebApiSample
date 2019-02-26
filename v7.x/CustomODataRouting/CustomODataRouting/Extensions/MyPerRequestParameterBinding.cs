using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace CustomODataRouting.Extensions
{
    internal class MyPerRequestParameterBinding : HttpParameterBinding
    {
        private MediaTypeFormatter _formatter;

        public MyPerRequestParameterBinding(HttpParameterDescriptor descriptor,
            MediaTypeFormatter formatter)
            : base(descriptor)
        {
            _formatter = formatter;
        }

        public override bool WillReadBody
        {
            get
            {
                return true;
            }
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            List<MediaTypeFormatter> perRequestFormatters = new List<MediaTypeFormatter>();

            MediaTypeFormatter perRequestFormatter = _formatter.GetPerRequestFormatterInstance(Descriptor.ParameterType, actionContext.Request, actionContext.Request.Content.Headers.ContentType);
            perRequestFormatters.Add(perRequestFormatter);

            HttpParameterBinding innerBinding = CreateInnerBinding(perRequestFormatters);

            return innerBinding.ExecuteBindingAsync(metadataProvider, actionContext, cancellationToken);
        }

        protected virtual HttpParameterBinding CreateInnerBinding(IEnumerable<MediaTypeFormatter> perRequestFormatters)
        {
            return Descriptor.BindWithFormatter(perRequestFormatters);
        }
    }
}