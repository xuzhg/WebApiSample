using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using System.Linq.Expressions;

namespace CaseInsensitiveFilterSample
{
    public class MyFilterBinder : FilterBinder
    {
        //  EF.Functions.Collate()
        static Expression functions = Expression.Constant(EF.Functions);

        public override Expression BindBinaryOperatorNode(Microsoft.OData.UriParser.BinaryOperatorNode binaryOperatorNode, QueryBinderContext context)
        {
            if (binaryOperatorNode.OperatorKind == Microsoft.OData.UriParser.BinaryOperatorKind.Equal
                || binaryOperatorNode.OperatorKind == Microsoft.OData.UriParser.BinaryOperatorKind.NotEqual)
            {
                if (binaryOperatorNode.Left.TypeReference.IsString() && binaryOperatorNode.Right.TypeReference.IsString())
                {
                    var left = this.Bind(binaryOperatorNode.Left, context);
                    var right = this.Bind(binaryOperatorNode.Right, context);
                    var collateMethod = typeof(RelationalDbFunctionsExtensions).GetMethod(
                        nameof(RelationalDbFunctionsExtensions.Collate))!
                        .MakeGenericMethod(typeof(string));
                    var leftCollated = Expression.Call(
                        collateMethod,
                        functions,
                        left,
                        Expression.Constant("NOCASE"));
                    var rightCollated = Expression.Call(
                        collateMethod,
                        functions,
                        right,
                        Expression.Constant("NOCASE"));
                    var binaryExpression = binaryOperatorNode.OperatorKind == Microsoft.OData.UriParser.BinaryOperatorKind.Equal
                        ? Expression.Equal(leftCollated, rightCollated)
                        : Expression.NotEqual(leftCollated, rightCollated);
                    return binaryExpression;
                }
            }

            return base.BindBinaryOperatorNode(binaryOperatorNode, context);
        }
    }
}
