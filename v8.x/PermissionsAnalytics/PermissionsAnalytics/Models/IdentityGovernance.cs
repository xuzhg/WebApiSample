using Microsoft.OData.ModelBuilder;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PermissionsAnalytics.Models
{
    public class IdentityGovernance
    {
        //[IgnoreDataMember]
        //[Key]
        //public string Id { get; set; }

        [Contained]
        public PermissionsAnalyticsAggregation PermissionsAnalytics { get; set; }
    }
}
