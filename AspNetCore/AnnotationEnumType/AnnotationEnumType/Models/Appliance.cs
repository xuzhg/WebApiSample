using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnnotationEnumType.Models
{

    [Flags]
    public enum Appliance : Int64
    {
        Stove = 1,
        Washer = 2,
        Dryer = 4,
        Microwave = 8
    }

    public enum Color
    {
        Red, Blue, Green
    }

    public enum Permission
    {
        NotAllowed,
        ReadOnly,
        WriteOnly,
        ReadWrite
    }

    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
