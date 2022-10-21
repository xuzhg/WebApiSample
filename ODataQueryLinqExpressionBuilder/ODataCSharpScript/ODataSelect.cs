using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ODataCSharpScript
{
    internal class ODataSelect : ODataQuery
    {
        public async Task<Expression> EvaluateAsync(IQueryable<Customer> source, string select)
        {
            var values = source.Select(x => x.Name).Expression;
            var globals = new Globals<Customer>();
            globals.DataSource = source;

            ScriptOptions options = ScriptOptions.Default;
            options = options.AddReferences("System", "System.Linq", "System.Collections.Generic");
            options = options.AddImports("System", "System.Linq", "System.Collections.Generic");

            // 
            string[] selects = select.Split(',');

            string scripts;
            if (selects.Length == 1)
            {
                scripts = $@"return DataSource.Select(x => x.{select}).Expression;";
            }
            else
            {
                // .Select(x => new { x.{s1}, x.{s2} }
                int index = 0;
                StringBuilder sb = new StringBuilder("x => new {");
                foreach (var s in selects)
                {
                    sb.Append($" x.{s}");
                    ++index;
                    if (index != selects.Length)
                    {
                        sb.Append(" ,");
                    }
                    
                }
                sb.Append(" }");

                scripts = $@"return DataSource.Select({sb.ToString()}).Expression;";
            }

            var state = await CSharpScript.RunAsync<Expression>(scripts, options, globals);

            return state.ReturnValue;
        }

        public IQueryable ExecuteAsync<T>(IQueryable<T> sources, Expression exp)
        {
            MethodCallExpression call = exp as MethodCallExpression;

            //  Select(...)
            MethodInfo methodInfo = call.Method;

            UnaryExpression  unary = call.Arguments[1] as UnaryExpression;

            LambdaExpression lambda = unary.Operand as LambdaExpression;

           // Delegate delegateFunc = lambda.Compile();

            return methodInfo.Invoke(null, new object[] { sources, lambda }) as IQueryable;
        }

        public IQueryable ExecuteAsync1<T>(IQueryable<T> sources, Expression exp)
        {
            MethodCallExpression call = exp as MethodCallExpression;
            LambdaExpression lambda = Expression.Lambda(call);

            return null;
        }

        public async Task<object> EvaluateAsync(IEnumerable<Customer> source, string select)
        {
         //   source.Select(x => x.Name);
            var globals = new Globals<Customer>();
       //     globals.DataSource = source;

            ScriptOptions options = ScriptOptions.Default;
            options.AddReferences(typeof(System.Linq.Enumerable).Assembly);

            options = options.AddReferences("System");
            options = options.AddReferences("System.Linq");
            options = options.AddReferences("System.Linq.Queryable");
            options = options.AddReferences("System.Collections.Generic");

            options.WithImports("System");
            options.WithImports("System.Linq");
            options.WithImports("System.Linq.Queryable");

            // using System.Linq;
            // using System.Linq.Queryable;
            // 
            string scripts = $@"
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
                
                return DataSource.Select(x => x.{select});
";

            var state = await CSharpScript.RunAsync(scripts, options, globals);

            return state.ReturnValue;
        }
    }

    public class Globals<T>
    {
        //public IEnumerable<T> DataSource { get; set; }

        //public IQueryable<T> DataSource1 { get; set; }

        public IQueryable<T> DataSource { get; set; }
    }
}
