using System.Reflection;
using System.Xml;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using Microsoft.OData.UriParser;

Version? v = typeof(ODataUri).Assembly.GetName().Version;
Console.WriteLine($"Microsoft.OData.Core under test: {v}\n");

const string csdl = """
    <edmx:Edmx Version="4.0" xmlns:edmx="http://docs.oasis-open.org/odata/ns/edmx">
      <edmx:DataServices>
        <Schema Namespace="Test" xmlns="http://docs.oasis-open.org/odata/ns/edm">
          <EnumType Name="ItemStatus">
            <Member Name="Active" Value="0"/>
            <Member Name="Deleted" Value="1"/>
          </EnumType>
          <EntityType Name="Item">
            <Key><PropertyRef Name="Id"/></Key>
            <Property Name="Id" Type="Edm.String" Nullable="false"/>
            <Property Name="Status" Type="Test.ItemStatus" Nullable="false"/>
          </EntityType>
          <EntityContainer Name="Container">
            <EntitySet Name="Items" EntityType="Test.Item"/>
          </EntityContainer>
        </Schema>
      </edmx:DataServices>
    </edmx:Edmx>
    """;

using var sr = new StringReader(csdl);
using var xr = XmlReader.Create(sr);
CsdlReader.TryParse(xr, out IEdmModel model, out IEnumerable<EdmError> _);

var serviceRoot = new Uri("https://example.com/");
var requestUri = new Uri("https://example.com/Items?$filter=Status eq 'Deleted'");

// Test 1: with StringAsEnumResolver (the bug-triggering path)
var parserA = new ODataUriParser(model, serviceRoot, requestUri)
{
    Resolver = new StringAsEnumResolver(),
};
Uri rebuiltA = parserA.ParseUri().BuildUri(ODataUrlKeyDelimiter.Slash);

// Control test: default resolver (the fixed path from PR #1391)
var parserB = new ODataUriParser(model, serviceRoot, requestUri); // no Resolver = ...
Uri rebuiltB = parserB.ParseUri().BuildUri(ODataUrlKeyDelimiter.Slash);

string original = Uri.UnescapeDataString(requestUri.Query);
string actualA = Uri.UnescapeDataString(rebuiltA.Query);
string actualB = Uri.UnescapeDataString(rebuiltB.Query);

Console.WriteLine($" Input:                                   {original}");
Console.WriteLine($" Rebuilt with StringAsEnumResolver:       {actualA}");
Console.WriteLine($" Rebuilt with default resolver (control): {actualB}");

bool bugFires = !actualA.Contains("eq 'Deleted'");
bool controlPasses = actualB.Contains("eq 'Deleted'");
return (bugFires && controlPasses) ? 1 : (bugFires ? 2 : 0);
