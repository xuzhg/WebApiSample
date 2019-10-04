using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderbyTest.Models
{
    public static class EdmModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<Order>("Orders");
            builder.ComplexType<MyComplexType<int>>();
            return builder.GetEdmModel();
        }

    }

    public class MyComplexType<T>
    {
        public string key { get; set; }
        public T value { get; set; }
    }
}
