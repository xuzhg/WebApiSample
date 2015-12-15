using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsNS
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Address Address { get; set; }
    }

    public class Manager : Employee
    {
        public decimal Salary { get; set; }
    }

    public class Seller : Employee
    {
        public double Bonus { get; set; }
    }
}
