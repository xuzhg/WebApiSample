namespace DynamicRouteSample.Models;

public class DataSource
{
    public static IList<Customer> Customers = new List<Customer>
    {
        new Customer { Id = 1, Name = "Sam", Address = new Address { City = "Earth", Street = "101 ST" } },
        new Customer { Id = 2, Name = "Peter", Address = new Address { City = "Mars", Street = "V ST" } },
        new Customer { Id = 3, Name = "Kerry", Address = new Address { City = "Cherry", Street = "98 RD" } },
        new Customer { Id = 4, Name = "Wu", Address = new Address { City = "Sun", Street = "185 AVE" } },
    };

    public static IList<Order> Orders = new List<Order>
    {
        new Order { Id = 1, Title = "Food", Price = 1.92},
        new Order { Id = 2, Title = "Snack" , Price = 9.99},
        new Order { Id = 3, Title = "Oil", Price =9.2 },
        new Order { Id = 4, Title = "Gun", Price = 2.9 },
    };

    public static IList<Person> PeopleV1 = new List<Person>
    {
        new Person { Id = 1, Name = "Chilly", Emails = new List<string> { "Earth@abc.com", "Xia@st.com" } },
        new Person { Id = 2, Name = "Mild", Emails = new List<string> { "Pat@abc.com", "101@YE.com" } },
        new Person { Id = 3, Name = "Balmy", Emails = new List<string> { "GG@abc.com", "1241@WU.com" } } ,
        new Person { Id = 4, Name = "Hot", Emails = new List<string> { "Ket@abc.com", "14@YU.com" } },
    };

    public static IList<Person> PeopleV2 = new List<Person>
    {
        new Person { Id = 51, Name = "Sweltering", Emails = new List<string> { "aa@d.com", "ad@c.com" } },
        new Person { Id = 52, Name = "Peter", Emails = new List<string> { "gt@d.com", "55@b.com" } },
        new Person { Id = 53, Name = "Cool", Emails = new List<string> { "re@f.com", "54d@a.com" } } ,
        new Person { Id = 54, Name = "Bracing", Emails = new List<string> { "t@d.com", "14@d.com" } },
    };
}

