using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.AspNet.OData.Builder;

namespace AzureFuncDemo
{
    public class EdmModelBuilder
    {
        private static IDictionary<string, IEdmModel> _models;


        public static IEdmModel GetEdmModel(string source)
        {
            Initialize();

            if (_models.TryGetValue(source, out IEdmModel model))
            {
                return model;
            }

            throw new NotSupportedException($"'{source}' is not supported.");
        }


        private static void Initialize()
        {
            if (_models != null)
            {
                return;
            }

            _models = new Dictionary<string, IEdmModel>();

            _models["cn"] = BuildModelSource_CN();
            _models["us"] = BuildModelSource_US();
        }

        private static IEdmModel BuildModelSource_CN()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<CnCustomer>("Customers");
            return builder.GetEdmModel();
        }

        private static IEdmModel BuildModelSource_US()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<UsCustomer>("Customers");
            return builder.GetEdmModel();
        }
    }

    public class CnCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Postcode { get; set; }
    }

    public class UsCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ZipCode { get; set; }
    }
}
