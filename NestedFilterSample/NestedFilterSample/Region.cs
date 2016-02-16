using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestedFilterSample
{
    public class Region
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<Facility> Facilities { get; set; }
    }

    public class Facility
    {
        public int Id { get; set; }

        public bool Active { get; set; }

        public IList<Department> Departments { get; set; }
    }

    public class Department
    {
        public int Id { get; set; }

        public bool Active { get; set; }
    }
}
