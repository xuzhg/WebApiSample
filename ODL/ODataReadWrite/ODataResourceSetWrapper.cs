using Microsoft.OData;
using System;
using System.Collections.Generic;

namespace ODataReadWrite
{
    class ODataResourceSetWrapper : ODataItemWrapper
    {
        public ODataResourceSet ResourceSet { get; set; }

        public IList<ODataResourceWrapper> Resources { get; set; } = new List<ODataResourceWrapper>();


        public override void Append(ODataItemWrapper newItem)
        {
            ODataResourceWrapper resourceWrapper = newItem as ODataResourceWrapper;
            if (resourceWrapper == null)
            {
                throw new InvalidOperationException("Append a non resource info to the resourceset");
            }

            Resources.Add(resourceWrapper);
        }
    }
}
