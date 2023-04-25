using System.Text.Json.Serialization;

namespace PermissionsAnalytics.Models
{
    public class IdentitySearchProperties : Finding
    {
        public PermissionsCreepIndex PermissionsCreepIndex { get; set; }

        /// <summary>
        /// Identity use for Data Source ID and Timestamp Filtering
        /// </summary>
        /// [JsonIgnore]
        public CloudknoxIdentity identity { get; set; }
    }

    public class AwsExternalSystemAccessFinding : IdentitySearchProperties
    {
    }

    public class AwsExternalSystemAccessRoleFinding : IdentitySearchProperties
    { }

    public class AwsIdentityAccessManagementKeyAgeFinding : IdentitySearchProperties
    { }

    public class AwsIdentityAccessManagementKeyUsageFinding : IdentitySearchProperties
    { }
    
}
