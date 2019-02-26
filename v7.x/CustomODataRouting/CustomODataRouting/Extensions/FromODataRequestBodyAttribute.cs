using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace CustomODataRouting.Extensions
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public sealed class FromODataRequestBodyAttribute : ParameterBindingAttribute
    {
        private static MediaTypeFormatter formatter = new MyMediaFormatter();

        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            return new NonValidatingParameterBinding(parameter, formatter);
        }

        private sealed class NonValidatingParameterBinding : MyPerRequestParameterBinding
        {
            public NonValidatingParameterBinding(HttpParameterDescriptor descriptor,
                MediaTypeFormatter formatter)
                : base(descriptor, formatter)
            {
            }

            protected override HttpParameterBinding CreateInnerBinding(IEnumerable<MediaTypeFormatter> perRequestFormatters)
            {
                return Descriptor.BindWithFormatter(perRequestFormatters, bodyModelValidator: null);
            }
        }
    }
}