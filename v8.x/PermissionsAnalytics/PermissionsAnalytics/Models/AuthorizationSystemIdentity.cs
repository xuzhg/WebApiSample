
namespace PermissionsAnalytics.Models
{
    public class AuthorizationSystemIdentity
    {
      
        public string Id
        {
            get;
            set;
        }

        public string ExternalId { get; set; }

        public string DisplayName { get; set; }

        public long IdentitySubType { get; set; }

    }
}
