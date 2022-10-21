using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ODataCSharpScript
{
    internal class ODataFilter : ODataQuery
    {

        public async Task<Expression> EvaluateAsync(IQueryable<Customer> source, string filter)
        {
            var values = source.Where(x => x.Name == "abc").Expression;

            var globals = new Globals<Customer>();
            globals.DataSource = source;

            ScriptOptions options = ScriptOptions.Default;
            options = options.AddReferences("System", "System.Linq", "System.Collections.Generic");
            options = options.AddImports("System", "System.Linq", "System.Collections.Generic");

            // using System.Linq;
            // using System.Linq.Queryable;
            // 
            string scripts = $@"
                return DataSource.Where(x => x.{filter}).Expression;
";

            var state = await CSharpScript.RunAsync<Expression>(scripts, options, globals);

            return state.ReturnValue;
        }
    }
}
