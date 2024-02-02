using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace SpeakerWebApi.Models
{
    public class EdmModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Class>("Classes");
            builder.EntitySet<Student>("Students");
            builder.EntitySet<SpeakerSubmissionResource>("Resources");
            return builder.GetEdmModel();
        }
    }
}
