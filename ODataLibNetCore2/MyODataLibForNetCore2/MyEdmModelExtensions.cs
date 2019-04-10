using Microsoft.OData.Edm;
using System;
using System.IO;

namespace MyODataLibForNetCore2
{
    public static class MyEdmModelExtensions
    {
        public static string SerializeAsJson(this IEdmModel model)
        {
            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(ms))
            {
                writer.WriteLine("{");

                writer.Write("  \"$EntitySetContainer:\"");
                writer.WriteLine("\"" + model.EntityContainer.FullName() + "\"");

                writer.WriteLine("}");
                writer.Flush();

                ms.Seek(0, SeekOrigin.Begin);
                return new StreamReader(ms).ReadToEnd();
            }
        }
    }
}
