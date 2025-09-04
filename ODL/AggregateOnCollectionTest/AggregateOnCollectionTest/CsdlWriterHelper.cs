// See https://aka.ms/new-console-template for more information
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Validation;

public class CsdlWriterHelper
{
    public static string GetCsdl(IEdmModel model, bool indent = true)
    {
        using (var stringWriter = new StringWriter())
        {
            using (var xmlWriter = System.Xml.XmlWriter.Create(stringWriter, new System.Xml.XmlWriterSettings() { Indent = indent }))
            {
                IEnumerable<EdmError> errors;
                if (!Microsoft.OData.Edm.Csdl.CsdlWriter.TryWriteCsdl(model, xmlWriter, Microsoft.OData.Edm.Csdl.CsdlTarget.OData, out errors))
                {
                    throw new Exception("Failed to write CSDL: " + string.Join(", ", errors.Select(e => e.ToString())));
                }
            }

            return stringWriter.ToString();
        }
    }
}