namespace ODataService.Models;

public class Customer
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string City { get; set; }

    public CustomerCategory Category { get; set; }

    public DateTime RegisteredAt { get; set; }

    public DateTime? LastOrderDate { get; set; }

    public List<Order> Orders { get; set; }
}

public class Order
{
    public int Id { get; set; }

    public string Product { get; set; }

    public decimal Amount { get; set; }
}

public enum CustomerCategory
{
    Standard,
    Premium,
    Vip
}
