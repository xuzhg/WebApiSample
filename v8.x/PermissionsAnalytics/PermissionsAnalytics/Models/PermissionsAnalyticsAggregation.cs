using Microsoft.OData.ModelBuilder;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PermissionsAnalytics.Models
{
    public class PermissionsAnalyticsAggregation
    {
        [Contained] public PermissionsAnalytics Aws { get; set; }

        [Contained] public PermissionsAnalytics Azure { get; set; }

        [Contained] public PermissionsAnalytics Gcp { get; set; }

        //[IgnoreDataMember][Key] public int Id { get; set; }
    }
}
