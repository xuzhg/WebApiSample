using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ZSpitz.Util;

namespace ODataQueryLinqExpressionBuilder
{
    internal class FilterBinder
    {
        private static readonly Dictionary<BinaryOperatorKind, ExpressionType> BinaryOperatorMapping = new Dictionary<BinaryOperatorKind, ExpressionType>
        {
            { BinaryOperatorKind.Add, ExpressionType.Add },
            { BinaryOperatorKind.And, ExpressionType.AndAlso },
            { BinaryOperatorKind.Divide, ExpressionType.Divide },
            { BinaryOperatorKind.Equal, ExpressionType.Equal },
            { BinaryOperatorKind.GreaterThan, ExpressionType.GreaterThan },
            { BinaryOperatorKind.GreaterThanOrEqual, ExpressionType.GreaterThanOrEqual },
            { BinaryOperatorKind.LessThan, ExpressionType.LessThan },
            { BinaryOperatorKind.LessThanOrEqual, ExpressionType.LessThanOrEqual },
            { BinaryOperatorKind.Modulo, ExpressionType.Modulo },
            { BinaryOperatorKind.Multiply, ExpressionType.Multiply },
            { BinaryOperatorKind.NotEqual, ExpressionType.NotEqual },
            { BinaryOperatorKind.Or, ExpressionType.OrElse },
            { BinaryOperatorKind.Subtract, ExpressionType.Subtract },
        };

        private IDictionary<string, ParameterExpression> _lambdaParameters = new Dictionary<string, ParameterExpression>();

        //public IEdmModel Model { get; }

        //public FilterBinder(IEdmModel model)
        //{
        //    Model = model;
        //}

        public virtual Expression Bind<T>(FilterClause filter)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), filter.RangeVariable.Name);
            _lambdaParameters[filter.RangeVariable.Name] = parameter;

            Expression body = BindExpression(filter.Expression);

            return Expression.Lambda(body, parameter);
        }

        private Expression BindExpression(QueryNode node)
        {
            if (node is CollectionNode collectionNode)
            {
                throw new NotSupportedException();
            }
            else if (node is SingleValueNode singleValueNode)
            {
                return BindSingleValueNode(singleValueNode);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private Expression BindSingleValueNode(SingleValueNode node)
        {
            switch (node.Kind)
            {
                case QueryNodeKind.BinaryOperator:
                    return BindBinaryOperatorNode(node as BinaryOperatorNode);

                case QueryNodeKind.Constant:
                    return BindConstantNode(node as ConstantNode);

                case QueryNodeKind.Convert:
                    return BindConvertNode(node as ConvertNode);

                case QueryNodeKind.SingleComplexNode:
                    return BindSingleComplexNode(node as SingleComplexNode);

                case QueryNodeKind.ResourceRangeVariableReference:
                    return BindRangeVariable((node as ResourceRangeVariableReferenceNode).RangeVariable);

                case QueryNodeKind.NonResourceRangeVariableReference:
                    return BindRangeVariable((node as NonResourceRangeVariableReferenceNode).RangeVariable);

                case QueryNodeKind.SingleValuePropertyAccess:
                    return BindPropertyAccessQueryNode(node as SingleValuePropertyAccessNode);

                case QueryNodeKind.SingleValueOpenPropertyAccess:
                    return BindOpenPropertyAccessQueryNode(node as SingleValueOpenPropertyAccessNode);

                default:
                    throw new NotSupportedException();
            }
        }

        internal Expression CreatePropertyAccessExpression(Expression source, IEdmProperty property)
        {
            return Expression.Property(source, property.Name);
        }

        private Expression BindSingleComplexNode(SingleComplexNode singleComplexNode)
        {
            Expression source = BindExpression(singleComplexNode.Source);

            return Expression.Property(source, singleComplexNode.Property.Name);
        }

        private Expression BindPropertyAccessQueryNode(SingleValuePropertyAccessNode propertyAccessNode)
        {
            Expression source = BindExpression(propertyAccessNode.Source);

            return Expression.Property(source, propertyAccessNode.Property.Name);
        }

        private Expression BindOpenPropertyAccessQueryNode(SingleValueOpenPropertyAccessNode propertyAccessNode)
        {
            Expression source = BindExpression(propertyAccessNode.Source);

            // Source ==> $it

            string DictionaryStringObjectIndexerName = typeof(Dictionary<string, object>).GetDefaultMembers()[0].Name;

           // IDictionary<string, Expression> properties = propertyAccessNode.Properties;

            source = Expression.Property(source, "Properties");

            // $it.Properties


            return Expression.Property(source, DictionaryStringObjectIndexerName, Expression.Constant(propertyAccessNode.Name));
            // $it.Properties.this[]

            //return Expression.Property(source, propertyAccessNode.Name);
        }

        private Expression BindConstantNode(ConstantNode constantNode)
        {
            if (constantNode.Value == null)
            {
                return Expression.Constant(null);
            }

            return Expression.Constant(constantNode.Value);
        }

        public static Type ToNullable(Type t)
        {
            if (t.IsNullable())
            {
                return t;
            }
            else
            {
                return typeof(Nullable<>).MakeGenericType(t);
            }
        }

        private Expression BindBinaryOperatorNode(BinaryOperatorNode binaryOperatorNode)
        {
            Expression left = BindExpression(binaryOperatorNode.Left);
            Expression right = BindExpression(binaryOperatorNode.Right);

            right = Expression.Convert(right, left.Type);

            if (BinaryOperatorMapping.TryGetValue(binaryOperatorNode.OperatorKind, out ExpressionType binaryExpressionType))
            {
                return Expression.MakeBinary(binaryExpressionType, left, right);
            }

            throw new NotSupportedException();
        }

        private Expression BindConvertNode(ConvertNode convertNode)
        {
            Expression source = BindExpression(convertNode.Source);

            IEdmModel model = null;
            Type conversionType = model.GetClrType(convertNode.TypeReference);

            return Expression.Convert(source, conversionType);
        }

        private Expression BindRangeVariable(RangeVariable rangeVariable)
        {
            return _lambdaParameters[rangeVariable.Name];
        }

    }

    public static class EdmModelExtensions
    {
        public static Type GetClrType(this IEdmModel model, IEdmTypeReference typeRef)
        {
            if (typeRef.IsDouble())
            {
                if (typeRef.IsNullable)
                {
                    return typeof(double?);
                }

                return typeof(double);
            }

            if (typeRef.IsSingle())
            {
                if (typeRef.IsNullable)
                {
                    return typeof(double?);
                }

                return typeof(double);
            }

            return null;
        }
    }
}
