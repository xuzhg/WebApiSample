namespace CalculatedPropertyTest.Models;

public class Student
{
    public int StudentId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string FavoriteSport { get; set; }

    public int Grade { get; set; }

    public int SchoolId { get; set; }

    public DateOnly BirthDay { get; set; }
}
