// See https://aka.ms/new-console-template for more information

using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.UriParser;

IEdmModel model = EdmModelProvider.GetEdmModel();

Console.WriteLine("EDM Model in CSDL format:");
Console.WriteLine("----------------------------------------------");
Console.WriteLine(CsdlWriterHelper.GetCsdl(model));
Console.WriteLine("----------------------------------------------");


Console.WriteLine("Register Custom Functions:");
EdmHelperHelper.RegisterCustomFunctions(model);
Console.WriteLine("Custom Functions Registered.\n");

Console.WriteLine("Press any key to continue to OData Uri parsing...");
Console.ReadKey();

Console.WriteLine("\n^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
Console.WriteLine("Do OData Uri parsing:  ~/Articles?$apply=aggregate(Emails with Microsoft.Combined as CombinedEmails)");
EdmHelperHelper.DoUriParse(model, "Articles?$apply=aggregate(Emails with Microsoft.Combined as CombinedEmails)");

Console.WriteLine("\n^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
Console.WriteLine("Do OData Uri parsing:  ~/Articles?$apply=aggregate(Tags with Microsoft.Union as DistinctTags)");
EdmHelperHelper.DoUriParse(model, "Articles?$apply=aggregate(Tags with Microsoft.Union as DistinctTags)");
Console.WriteLine("\n^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");

Console.WriteLine("Done!");