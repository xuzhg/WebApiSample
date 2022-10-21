using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataQueryLinqExpressionBuilder.SelectExpand
{
    internal abstract class SelectExpandWrapper : ISelectExpandWrapper
    {
        public virtual IDictionary<string, object> ToDictionary()
        {
            throw new NotImplementedException();
        }
    }
}
