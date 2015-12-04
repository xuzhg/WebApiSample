using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostingDb.Pandora
{
    public class Component
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Identity { get; set; }

        public IList<ComponentUser> Users { get; set; }

        public IList<Component> Childs { get; set; }
    }

    public class ComponentUser
    {
        public int Id { get; set; }
    }
}
