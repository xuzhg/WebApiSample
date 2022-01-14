using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetClassicOData.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ShowHiddenAttribute : Attribute
    {
    }
}