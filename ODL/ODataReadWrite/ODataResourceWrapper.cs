using Microsoft.OData;
using System;
using System.Collections.Generic;
using System.Text;

namespace ODataReadWrite
{
    class ODataResourceWrapper : ODataItemWrapper
    {
        public string TypeName => Resource.TypeName;

        public ODataResource Resource { get; set; }

        public IList<ODataNestedResourceInfoWrapper> NestedResourceInfos { get; set; } = new List<ODataNestedResourceInfoWrapper>();

        public override void Append(ODataItemWrapper newItem)
        {
            ODataNestedResourceInfoWrapper newWrapper = newItem as ODataNestedResourceInfoWrapper;
            if (newWrapper == null)
            {
                throw new InvalidOperationException("Append a non nested resource info to the resource");
            }

            NestedResourceInfos.Add(newWrapper);
        }
    }
}
