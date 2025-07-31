using System.Text.Json;

namespace CalculatedPropertyTest.Models;

public class School
{
    public int SchoolId { get; set; }

    public string SchoolName { get; set; }

    // It's for Edm model, but calculated from 'Student' table
    // all last list is seperated by comma
    public string LastNames { get; set; }

    public Address MailAddress { get; set; }

    public SchoolDetail Details { get; set; }

    public IList<Student> Students { get; set; }
}
