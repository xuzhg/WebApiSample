using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using System;

namespace ODataOWin
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultSelectAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultHiddenAttribute : Attribute
    {
    }
}
