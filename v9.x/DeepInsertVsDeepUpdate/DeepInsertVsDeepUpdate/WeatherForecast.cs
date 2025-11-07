using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace DeepInsertVsDeepUpdate
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }


    public class AgentInstance
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public AgentCardManifest? Manifest { get; set; }

        public AgentCardManifest? Manifest2 { get; set; }

        public ODataDeepUpdateMetadataCollection DeepUpdateMetadata { get; set; } = new ODataDeepUpdateMetadataCollection();

    }

    public class AgentCardManifest
    {
        public int Id { get; set; }

        public string? DisplayName { get; set; }

    }


    public class ODataDeepInsertMetadata
    {
        public string PropertyName { get; set; }
        
        public string ODataId { get; set; }
    }

    public class ODataDeepUpdateMetadataCollection : List<ODataDeepInsertMetadata>
    {
    }


    public class EdmModelBuilder
    {
        public static IEdmModel BuildEdmModel()
        {
            // Implementation for building the EDM model goes here.
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<AgentInstance>("AgentInstances");
            builder.EntitySet<AgentCardManifest>("Manifests");
            builder.EntityType<AgentInstance>().Ignore(a => a.DeepUpdateMetadata); // 
            return builder.GetEdmModel();
        }
    }
}
