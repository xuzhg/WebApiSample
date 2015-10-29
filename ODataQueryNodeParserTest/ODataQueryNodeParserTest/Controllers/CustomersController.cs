using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.OData;
using System.Web.OData.Query;
using Microsoft.OData.Core.UriParser.Semantic;
using Microsoft.OData.Core.UriParser.TreeNodeKinds;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Library;
using ODataQueryNodeParserTest.Models;

namespace ODataQueryNodeParserTest.Controllers
{
    public class CustomersController : ODataController
    {
        private static IList<Customer> _customers;
        static CustomersController()
        {
            _customers = Enumerable.Range(1, 5).Select(e => new Customer
            {
                Id = e,
                CreatedOn = new DateTimeOffset(2015, 10, e, 1, 2, 3, 4, TimeSpan.Zero)
            }).ToList();
        }

        public IHttpActionResult Get(ODataQueryOptions<Customer> options)
        {
            if (options.Filter != null)
            {
                var filterClause = options.Filter.FilterClause;

                var nodes = ParserToQueryNodes(options);

                Console.WriteLine();
                foreach (var oDataQueryNode in nodes)
                {
                    Console.WriteLine(" [" + oDataQueryNode.PropertyName + " " + oDataQueryNode.Operator + " " + oDataQueryNode.Value + " ]");
                }
                Console.WriteLine();
            }

            var result = options.ApplyTo(_customers.AsQueryable()).AsQueryable();
            return Ok(result, result.GetType());

            return Ok(_customers);
        }

        private IHttpActionResult Ok(object content, Type type)
        {
            var resultType = typeof(OkNegotiatedContentResult<>).MakeGenericType(type);
            return Activator.CreateInstance(resultType, content, this) as IHttpActionResult;
        }

        public static IEnumerable<ODataQueryNode> ParserToQueryNodes(ODataQueryOptions<Customer> options)
        {
            if (options == null)
            {
                return Enumerable.Empty<ODataQueryNode>();
            }

            IList<ODataQueryNode> queryNodes = new List<ODataQueryNode>();
            if (options.Filter != null)
            {
                ParserFilterClause(options.Filter, queryNodes);
            }

            return queryNodes;
        }

        private static void ParserFilterClause(FilterQueryOption filter, IList<ODataQueryNode> queryNodes)
        {
            if (filter == null || filter.FilterClause == null)
            {
                return;
            }

            var filterClause = filter.FilterClause;
            var range = filterClause.RangeVariable;

            Bind(filterClause.Expression, queryNodes);
        }

        private static Expression Bind(QueryNode node, IList<ODataQueryNode> queryNodes)
        {
            CollectionNode collectionNode = node as CollectionNode;
            SingleValueNode singleValueNode = node as SingleValueNode;

            if (collectionNode != null)
            {
                // TODO: 
            }
            else if (singleValueNode != null)
            {
                switch (singleValueNode.Kind)
                {
                    case QueryNodeKind.BinaryOperator:
                        return BindBinaryOperatorNode(node as BinaryOperatorNode, queryNodes);

                    case QueryNodeKind.Constant:
                        return BindConstantNode(node as ConstantNode, queryNodes);

                    case QueryNodeKind.SingleValuePropertyAccess:
                        return BindPropertyAccessQueryNode(node as SingleValuePropertyAccessNode, queryNodes);

                  //  case QueryNodeKind.EntityRangeVariableReference:
                   //     return BindRangeVariable((node as EntityRangeVariableReferenceNode).RangeVariable);
                }
            }

            return null;
        }

        private static Expression BindBinaryOperatorNode(BinaryOperatorNode binaryOperatorNode,
            IList<ODataQueryNode> queryNodes)
        {
            Expression left = Bind(binaryOperatorNode.Left, queryNodes);
            Expression right = Bind(binaryOperatorNode.Right, queryNodes);

            switch (binaryOperatorNode.OperatorKind)
            {
                case BinaryOperatorKind.Add:
                case BinaryOperatorKind.Or:
                    //Bind(binaryOperatorNode.Left, queryNodes);
                    //Bind(binaryOperatorNode.Right, queryNodes);
                    break;

                case BinaryOperatorKind.Equal:
                case BinaryOperatorKind.NotEqual:
                case BinaryOperatorKind.GreaterThan:
                case BinaryOperatorKind.GreaterThanOrEqual:
                case BinaryOperatorKind.LessThan:
                case BinaryOperatorKind.LessThanOrEqual:
                    ConstantExpression member = (ConstantExpression)left;
                     ConstantExpression const1 = (ConstantExpression) right;
                     queryNodes.Add(new ODataQueryNode(member.Value.ToString(), binaryOperatorNode.OperatorKind, const1.Value));
                    /*
                    if (left.NodeType == ExpressionType.MemberAccess)
                    {
                        MemberExpression member = (MemberExpression) left;
                        ConstantExpression const1 = (ConstantExpression) right;
                        queryNodes.Add(new ODataQueryNode(member.Member.Name, binaryOperatorNode.OperatorKind,
                            const1.Value));
                    }
                    else
                    {
                        MemberExpression member = (MemberExpression)right;
                        ConstantExpression const1 = (ConstantExpression)left;
                        queryNodes.Add(new ODataQueryNode(member.Member.Name, binaryOperatorNode.OperatorKind,
                            const1.Value));
                    }*/
                    break;
            }

            return Expression.Constant(0);
        }

        private static Expression BindConstantNode(ConstantNode constantNode, IList<ODataQueryNode> queryNodes)
        {
            // no need to parameterize null's as there cannot be multiple values for null.
            if (constantNode.Value == null)
            {
                return Expression.Constant(null);
            }

            object value = constantNode.Value;
            return Expression.Constant(value);
        }

        private static Expression BindPropertyAccessQueryNode(SingleValuePropertyAccessNode propertyAccessNode, IList<ODataQueryNode> queryNodes)
        {
            //Expression source = Bind(propertyAccessNode.Source, queryNodes);
            string propertyName = propertyAccessNode.Property.Name;
            return Expression.Constant(propertyName, typeof(string));
        }
    }
}
