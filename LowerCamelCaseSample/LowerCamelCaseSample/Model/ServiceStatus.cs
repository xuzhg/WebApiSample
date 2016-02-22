using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LowerCamelCaseSample
{
    public enum ServiceStatus
    {
        Active = 1,
        Sleep = 2,
        ShutDown = 3,
        Broken = 4
    }

    [DataContract(Name = "color")]
    public enum Color
    {
        Red,
        Blue,
        Green
    }
}
