using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiComplexTypeSkipPropertyTest
{
    public class SkipPropertyAnnotation
    {
        public SkipPropertyAnnotation(params string[] properties)
        {
            Skips = new List<string>(properties);
        }

        public SkipPropertyAnnotation(IList<string> properties)
        {
            Skips = properties;
        }

        public IList<string> Skips { get; private set; }
    }
}
