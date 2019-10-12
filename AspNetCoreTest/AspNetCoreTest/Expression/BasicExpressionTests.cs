using NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace Microsoft.AspNetCore.Test.ExpressionTest
{
    public class BasicExpressionTests
    {
        [Fact]
        public void BasicTest()
        {
            IList<Customer> customers = new List<Customer>();
            customers.Select(c => c.HomeLocation.Street);

            Expression<Func<IList<Customer>, IEnumerable<string>>> exprTree = (cs) => cs.Select(c => c.HomeLocation.Street);

            Assert.NotNull(exprTree);
            Expression body = exprTree.Body;
            Assert.NotNull(body);
        }
    }
}
