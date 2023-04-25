using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace PermissionsAnalytics.Models
{
    public static class EdmModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntityType<PermissionsAnalyticsAggregation>();
            builder.EntityType<PermissionsAnalytics>();
            builder.EntityType<PermissionsCreepIndex>();
            builder.EntityType<ActionSummary>();
            builder.EntityType<AuthorizationSystemIdentity>();

            builder.Singleton<IdentityGovernance>("identityGovernance");
            return builder.GetEdmModel();
        }
    }
}
