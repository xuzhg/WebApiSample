using System;
using System.Collections.Generic;
using System.Text;

namespace ODataReadWrite
{
    abstract class ODataItemWrapper
    {
        public abstract void Append(ODataItemWrapper newItem);
    }
}
