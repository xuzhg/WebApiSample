// Configure the HTTP request pipeline.



public class Customer
{
    public int Id { get; set; }

    public string Name { get; set; }

    public IList<string> Emails { get; set; }

    public Address Address { get; set; }

    public IList<Order> Orders { get; set; }
}
