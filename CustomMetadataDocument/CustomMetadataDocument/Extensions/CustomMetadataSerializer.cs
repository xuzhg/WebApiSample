using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.OData.Formatter.Serialization;
using Microsoft.OData.Core;

namespace CustomMetadataDocument.Extensions
{
    public class CustomMetadataSerializer : ODataMetadataSerializer
    {
        public override void WriteObject(object graph, Type type, ODataMessageWriter messageWriter, ODataSerializerContext writeContext)
        {
            // Console.WriteLine("YYYYYYYYYYY");
            base.WriteObject(graph, type, messageWriter, writeContext);
        }
    }
}
