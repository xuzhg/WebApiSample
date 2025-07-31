using CalculatedPropertyTest.Models;
using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OData.WebApi.Extensions;

public class SchoolStudentSelectExpandBinder : SelectExpandBinder
{
    public SchoolStudentSelectExpandBinder(IFilterBinder filterBinder, IOrderByBinder orderByBinder)
        : base(filterBinder, orderByBinder)
    {
    }

    // 
    public override Expression CreatePropertyValueExpression(QueryBinderContext context, IEdmStructuredType elementType, IEdmProperty edmProperty, Expression source, FilterClause filterClause, ComputeClause computeClause = null, SearchClause search = null)
    {
        if (!edmProperty.Type.IsCollection())
        {
            IEdmModel model = context.Model;
            if (string.Equals(edmProperty.Name, "LastNames", StringComparison.OrdinalIgnoreCase))
            {
                // $it.Students
                PropertyInfo studentsProperty = source.Type.GetProperty("Students");
                Expression propertyValue = Expression.Property(source, studentsProperty);

                // s
                ParameterExpression studentParameter = Expression.Parameter(typeof(Student), "s");

                // s => s.LastName
                LambdaExpression lastNameSelector = Expression.Lambda(
                    Expression.Property(studentParameter, nameof(Student.LastName)),
                    studentParameter
                );

                // $it.Students.Select(s => s.LastName)
                MethodCallExpression selectExpression = Expression.Call(
                    typeof(Enumerable),
                    nameof(Enumerable.Select),
                    new[] { typeof(Student), typeof(string) },
                    propertyValue,
                    lastNameSelector
                );

                //  MethodInfo selectMethod = _queryableSelectMethod.MakeGenericMethod(type, expression.Body.Type);
                //  return selectMethod.Invoke(null, new object[] { query, expression }) as IQueryable;

                //Expression lastNamesExpression = Expression.Call(
                //    typeof(string),
                //    nameof(string.Join),
                //    Type.EmptyTypes,
                //    Expression.Constant(","),
                //    Expression.Property(propertyValue, nameof(IQueryable<Student>.Select))
                //);
                Expression lastNamesExpression = Expression.Call(
                    typeof(string),
                    nameof(string.Join),
                    Type.EmptyTypes,
                    Expression.Constant(","),
                    selectExpression
                );

                return lastNamesExpression;
            }
        }


        return base.CreatePropertyValueExpression(context, elementType, edmProperty, source, filterClause, computeClause);
    }
}
