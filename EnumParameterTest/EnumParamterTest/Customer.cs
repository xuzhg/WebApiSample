using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumParamterTest
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; }
    }

    public enum Category
    {
        Vip,
        General
    }
}
