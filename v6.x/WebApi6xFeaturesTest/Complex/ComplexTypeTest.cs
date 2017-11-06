using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.AspNet.OData.Test;
using Microsoft.OData.Edm;
using ModelLibrary;
using Xunit;
using Xunit.Abstractions;

namespace WebApi6xFeaturesTest.QueryComplexTypeTest
{
    public class ComplexTypeTest
    {
        private readonly ITestOutputHelper _output;

        public ComplexTypeTest(ITestOutputHelper output)
        {
            this._output = output;
        }
    }
}
