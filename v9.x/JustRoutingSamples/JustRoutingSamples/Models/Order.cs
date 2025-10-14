using System.ComponentModel.DataAnnotations;

namespace JustRoutingSamples.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ProductName { get; set; }
        public User User { get; set; }
    }
}
