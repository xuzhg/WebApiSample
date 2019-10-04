using Microsoft.OData;
using System;
using System.Collections.Generic;
using System.Text;

namespace ODataReadWrite
{
    class ODataNestedResourceInfoWrapper : ODataItemWrapper
    {
        public ODataNestedResourceInfo NestedInfo { get; set; }

        public ODataItemWrapper NestedWrapper { get; private set; }

        public override void Append(ODataItemWrapper newItem)
        {
            if (NestedWrapper != null)
            {
                throw new InvalidOperationException("Append multiple item for a nested resource info.");
            }

            NestedWrapper = newItem;
        }
    }
}
