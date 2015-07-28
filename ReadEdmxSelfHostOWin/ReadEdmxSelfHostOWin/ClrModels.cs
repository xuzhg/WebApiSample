using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadEdmxSelfHostOWin
{
    public class Protocol
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public IList<Implementation> Implementations { get; set; }

        public IList<Field> Fields { get; set; }
    }

    public class Implementation
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class Field
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
