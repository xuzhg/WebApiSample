using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DollarLevelSample
{
    public class TreeItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<TreeItem> Children { get; set; }
    }
}
