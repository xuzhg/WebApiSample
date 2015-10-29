using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OData.Core.UriParser.TreeNodeKinds;

namespace ODataQueryNodeParserTest.Models
{
    public class ODataQueryNode
    {
        public ODataQueryNode(string propertyName, BinaryOperatorKind @operator, object value)
        {
            PropertyName = propertyName;
            Operator = @operator;
            Value = value;
        }

        public string PropertyName { get; set; }
        public BinaryOperatorKind Operator { get; set; }
        public object Value { get; set; }

    }
}
