using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HostingDb.Pandora;

namespace ExpandTestInV3
{
    // I just use the in-memory data set
    public class ComponentContext
    {
        public IList<Component> Components { get; set; }

        public IList<Component> ChildComponents { get; set; }

        public ComponentContext()
        {
            int childId = 1;
            Components = Enumerable.Range(1, 5).Select(e => new Component
            {
                Id = "XYZ" + e,
                Name = "Component_" + e,
                Identity = "identity" + e,
                Type = "TYPE" + e,
                Childs = Enumerable.Range(1, 3).Select(f => new Component
                {
                    Id = "Sub" + childId,
                    Name = "SubComponent_" + childId,
                    Identity = "identity" + e, // same the parent
                    Type = "SubTYPE" + childId++,
                }).ToList(),
                Users = Enumerable.Range(1, 4).Select(g => new ComponentUser
                {
                    Id = g
                }).ToList()
            }).ToList();

            ChildComponents = new List<Component>();
            foreach (var component in Components)
            {
                foreach (var child in component.Childs)
                {
                    ChildComponents.Add(child);
                }
            }
        }
    }
}
