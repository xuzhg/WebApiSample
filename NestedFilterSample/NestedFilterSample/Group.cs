using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestedFilterSample
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsHidden { get; set; }
        public bool IsShared { get; set; }

        public IList<Query> Queries { get; set; }
    }

    public class Query
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsPinned { get; set; }
    }
}
