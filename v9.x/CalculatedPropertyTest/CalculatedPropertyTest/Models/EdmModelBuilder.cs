using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.Net;
namespace CalculatedPropertyTest.Models;

public static class EdmModelBuilder
{
    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder buildler = new();

        buildler.EntitySet<School>("Schools");
        buildler.EntitySet<SchoolDetail>("Details");
        buildler.EntitySet<Student>("Students");

        var schoolConf = buildler.EntityType<School>();
      //  schoolConf.Ignore(c => c.Emails);
      //  schoolConf.CollectionProperty(c => c.ContactEmails);

      //  schoolConf.Ignore(c => c.Branches);
      //  schoolConf.CollectionProperty(c => c.BranchAddresses).Name = "Branches"; // Change the name same as the name in DB

        return buildler.GetEdmModel();
    }
}
