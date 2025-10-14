using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace JustRoutingSamples.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public IList<Order> Orders { get; set; }
    }
}
