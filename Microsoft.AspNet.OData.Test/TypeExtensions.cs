using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace Microsoft.AspNet.OData.Test
{
    internal static class TypeExtensions
    {
        public static HttpConfiguration GetHttpConfiguration(this Type[] controllers)
        {
            var resolver = new TestAssemblyResolver(new MockAssembly(controllers));
            var configuration = new HttpConfiguration();
            configuration.Services.Replace(typeof(IAssembliesResolver), resolver);
            return configuration;
        }
    }

    internal sealed class MockAssembly : Assembly
    {
        Type[] _types;

        public MockAssembly(params Type[] types)
        {
            _types = types;
        }

        public override Type[] GetTypes()
        {
            return _types;
        }
    }

    internal class TestAssemblyResolver : IAssembliesResolver
    {
        List<Assembly> _assemblies;

        public TestAssemblyResolver(MockAssembly assembly)
        {
            _assemblies = new List<Assembly>();
            _assemblies.Add(assembly);
        }

        public TestAssemblyResolver(params Type[] types)
            : this(new MockAssembly(types))
        {
        }

        public ICollection<Assembly> GetAssemblies()
        {
            return _assemblies;
        }
    }
}
