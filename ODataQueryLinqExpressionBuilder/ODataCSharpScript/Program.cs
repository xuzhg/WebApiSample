using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODataCSharpScript;

internal class Program
{
    static async Task Main(string[] args)
    {
        var customers = new List<Customer>
        {
            new Customer {Id = 1, Name ="John", Age=19},

            new Customer {Id = 2, Name ="Peter", Age=39}
        };

        CustomerContext context = new CustomerContext();
        context.Database.EnsureCreated();
        if (!context.Customers.Any())
        {
            foreach (var c in customers)
            {
                context.Customers.Add(new Customer
                {
                    Name = c.Name + " EF",
                    Age = c.Age
                });
            }

            foreach (var c in customers)
            {
                context.Customers.Add(new Customer
                {
                    Name = c.Name + " XY",
                    Age = c.Age
                });
            }

            await context.SaveChangesAsync();
        }

        Globals<Customer> gl = new Globals<Customer>
        {
            DataSource = customers.AsQueryable()
        };

        var r = await TestAsync("Select(x=> x.Name)", gl);
   //     if (r != null) { return; }

        ODataSelect select = new ODataSelect();
     //   var result1 = await select.EvaluateAsync(customers, "Name");

        IQueryable<Customer> queryable = customers.AsQueryable();

        var expression = await select.EvaluateAsync(queryable, "Name,Age");

        // Execute on Database
        var result3 = select.ExecuteAsync(context.Customers, expression);

        foreach (var c in result3)
        {
            Console.WriteLine($" -  '{c}'");
        }

        //while (true)
        //{
        //    var codeToEval = Console.ReadLine();
        //    var result = await CSharpScript.EvaluateAsync(codeToEval);
        //    Console.WriteLine(result);
        //}
    }

    public static async Task<object> TestAsync<T>(string linQuery, Globals<T> globals) where T : Customer
    {
        var a = globals.DataSource.Select(x => x.Name).Expression;

        ScriptOptions scriptOptions = ScriptOptions.Default;
        scriptOptions = scriptOptions.AddReferences("System");
        scriptOptions = scriptOptions.AddReferences("System.Linq");
       // scriptOptions = scriptOptions.AddReferences("System.Linq.Expressions");
        scriptOptions = scriptOptions.AddReferences("System.Collections.Generic");

        var state = await CSharpScript.RunAsync($@"
                using System;
                using System.Linq;
                using System.Linq.Expressions;
                using System.Collections.Generic;

                Console.WriteLine(DataSource.GetType());
                var a = DataSource.Expression;

                var res = DataSource.{linQuery};
                Console.WriteLine(res.GetType());

                Console.WriteLine(""{linQuery}"");
                
                IQueryable query = res as IQueryable;
                if (query != null) 
                {{
                    Console.WriteLine(""OK"");
                }}

                var exp = res.Expression;

                return DataSource.{linQuery}.Expression;", scriptOptions, globals);

        return state.ReturnValue;
    }

    private static QueryKind ParseODataQuery(string odataQuery)
    {
        var queries = odataQuery.Split('$');

        return QueryKind.Select;
    }
}
//public class Globals<T> : IGlobals<T>
//{
//    public IEnumerable<T> DataSource { get; set; }
//}
/*
 
        string scripts = $@"
                using System;
                using System.Linq;
                using System.Collections.Generic;
                
                return Querysource.{linQuery}.Expression;";
 */
//public interface IGlobals<T>
//{
//    IEnumerable<T> DataSource { get; set; }
//}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}