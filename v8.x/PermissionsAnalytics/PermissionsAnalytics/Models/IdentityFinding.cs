using Microsoft.OData.ModelBuilder;
using System.Text.Json.Serialization;

namespace PermissionsAnalytics.Models
{
    public class IdentityFinding : IdentitySearchProperties
    {
        public ActionSummary ActionSummary { get; set; }

        public DateTimeOffset? LastActiveDateTime { get; set; }

        [JsonPropertyName("identity")]
       // [AutoExpand]
        public virtual AuthorizationSystemIdentity AuthorizationIdentity { get; set; }
    }

    public class InactiveAwsResourceFinding : IdentityFinding
    {
    }

    public class InactiveAwsRoleFinding : IdentityFinding
    {
    }

    public class InactiveAzureServicePrincipalFinding : IdentityFinding
    {
    }

    public class InactiveGcpServiceAccountFinding : IdentityFinding
    {
    }

    public class InactiveServerlessFunctionFinding : IdentityFinding
    {
    }

    public class InactiveUserFinding : IdentityFinding
    {
    }

    public class OverprovisionedAwsResourceFinding : IdentityFinding
    {
    }
}