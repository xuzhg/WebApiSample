using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using ExpressionTreeToString;
using System.Threading.Tasks;

namespace ODataQueryLinqExpressionBuilder
{
    internal class SelectDemo
    {
        // We can use IDictionary<string, object>, or use ExpandoObject, need to check with EF or EFCore
        static MethodInfo addMethod = typeof(Dictionary<string, object>).GetMethod("Add");

        public static void Execute()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Sample Data:");
                Print(ConsoleColor.Green, Program._customers, true);

                // Or use dynamic property as : MyData gt 5.0
                Console.WriteLine("\nPlease input select properties, separate using ',' (For example: Name,Age or Q/q for quit):");
                string cmd = Console.ReadLine();
                if (cmd == "q" || cmd == "Q")
                {
                    break;
                }

                var properties = cmd.Split(',');

                // Linq Expression
                LambdaExpression selectLambda = BuildSelectExpression<Customer>(properties);

                string selectStr = selectLambda.ToString("C#");
                Console.WriteLine($"\n\t==> {selectStr}\n");

                var selected = InvokeSelect(Program._customers, selectLambda);

                Console.WriteLine("Select Result:");
                PrintSelect(ConsoleColor.Blue, selected, properties);

                Console.WriteLine("Press any key to try more");
                Console.ReadKey();
            }
        }

        private static LambdaExpression BuildSelectExpression<T>(string[] selects)
        {
            Type entityType = typeof(T);

            ParameterExpression parameter = Expression.Parameter(entityType, "a");

            IList<ElementInit> inits = new List<ElementInit>();
            foreach (var select in selects)
            {
                var trimPropertyName = select.Trim();

                // for example: Location/City, let's only cover two levels
                var subProperties = trimPropertyName.Split("/");
                Expression propertyAccess;
                if (subProperties.Length > 1)
                {
                    propertyAccess = Expression.Property(parameter, subProperties[0]); // a.Location
                    propertyAccess = Expression.Property(propertyAccess, subProperties[1]); // a.Location.City

                    if (entityType.GetProperty(subProperties[0]).PropertyType.GetProperty(subProperties[1]).PropertyType.IsValueType)
                    {
                        propertyAccess = Expression.Convert(propertyAccess, typeof(object));
                    }

                    trimPropertyName = subProperties[1];
                }
                else
                {
                    propertyAccess = Expression.Property(parameter, trimPropertyName);

                    if (entityType.GetProperty(trimPropertyName).PropertyType.IsValueType)
                    {
                        propertyAccess = Expression.Convert(propertyAccess, typeof(object));
                    }
                }

                ElementInit initElement = Expression.ElementInit(addMethod, Expression.Constant(trimPropertyName, typeof(string)), propertyAccess);
                inits.Add(initElement);
            }

            NewExpression dictNew = Expression.New(typeof(Dictionary<string, object>));

            ListInitExpression dictInit = Expression.ListInit(dictNew, inits);

            return Expression.Lambda(dictInit, parameter);

            // a => new Dictionary<string, object>() { { "Name",  a.Name}, .... }
        }

        private static IEnumerable InvokeSelect<T>(IEnumerable<T> source, LambdaExpression lambda)
        {
            if (lambda == null)
            {
                throw new ArgumentNullException("lambda");
            }

            MethodInfo selectMethod = GetEnumerablSelect();

            selectMethod = selectMethod.MakeGenericMethod(typeof(T), lambda.Body.Type);

            Delegate delegateFunc = lambda.Compile();

            return selectMethod.Invoke(null, new object[] { source, delegateFunc }) as IEnumerable;
        }

        private static MethodInfo GetEnumerablSelect()
        {
            // public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector);
            //  public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector);
            var selectMethods = typeof(Enumerable).GetMethods().Where(x => x.Name == "Select");

            foreach (var selectMethod in selectMethods)
            {
                ParameterInfo[] parameters = selectMethod.GetParameters();
                if (parameters[1].ParameterType.GetGenericArguments().Length == 2)
                {
                    return selectMethod;
                }
            }

            throw new NotSupportedException();
        }

        public static void Print(ConsoleColor color, IEnumerable<Customer> customers, bool head = false)
        {
            ConsoleColor backColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            Console.WriteLine();
            if (head)
            {
                Console.WriteLine($"  | ID |      Name|  Age  |              Location                  |");
                Console.WriteLine($"  ------------------------------------------------------------------");
            }
            foreach (var customer in customers)
            {
                Console.WriteLine($"  |  {customer.Id} |{customer.Name,10}|  {customer.Age}  |{customer.Location, 40}|");
            }
            Console.WriteLine();
            Console.ForegroundColor = backColor;
        }

        public static void PrintSelect(ConsoleColor color, IEnumerable customers, string[] selects)
        {
            ConsoleColor backColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            //Console.WriteLine("  ");
            //foreach (var select in selects)
            //{
            //    Console.Write($"|    {select.Trim()}    ");
            //}
            //Console.Write("|");
            //Console.WriteLine($"  --------------------------------------------------------");

            int index = 1;
            foreach (var customer in customers as IEnumerable<IDictionary<string, object>>)
            {
                Console.Write($"  {index++})");
                foreach (var item in customer)
                {
                    Console.Write($"  {item.Key}:{item.Value}");
                }

                Console.WriteLine();
            }
            Console.WriteLine();
            Console.ForegroundColor = backColor;
        }
    }
}
