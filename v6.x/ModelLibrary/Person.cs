using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    public class Person
    {
        public int Id { get; set; }

        public IDictionary<string, object> DynamicProperties { get; set; }
    }
}
