using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalDateTimeSample.Models
{
    public class LocalDateTimeConverter : IPrimitiveValueConverter
    {
        public object ConvertFromUnderlyingType(object value)
        {
            return value;
        }

        public object ConvertToUnderlyingType(object value)
        {
            return value;
        }
    }
}
