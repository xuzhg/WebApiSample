namespace DynamicRouteSample.Models;

public class Person
{
    public int Id { get; set; }

    public string Name { get; set; }

    public IList<string> Emails { get; set; }
}