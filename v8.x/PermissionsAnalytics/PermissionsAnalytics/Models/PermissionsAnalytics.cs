using Microsoft.OData.ModelBuilder;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PermissionsAnalytics.Models
{
    public class PermissionsAnalytics
    {
        [Contained] public List<Finding> Findings { get; set; }

        //[IgnoreDataMember][Key] public string Id { get; set; }
    }
}
