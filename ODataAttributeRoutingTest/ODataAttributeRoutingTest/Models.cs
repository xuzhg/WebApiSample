using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataAttributeRoutingTest
{
    public class Restaurant
    {
        public int Id { get; set; }

        public IEnumerable<Dish> ResDishes { get; set; }
    }

    public class Dish
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
