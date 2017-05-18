
<h2>There's a requirement to skip some properties to serialize, while such properties are presented in schema.</h2>


<h3>Run results</h3>


<h4>Disable</h4>

```xml
<?xml version="1.0" encoding="utf-8"?>
<edmx:Edmx Version="4.0" xmlns:edmx="http://docs.oasis-open.org/odata/ns/edmx">
  <edmx:DataServices>
    <Schema Namespace="WebApiComplexTypeSkipPropertyTest" xmlns="http://docs.oasis-open.org/odata/ns/edm">
      <EntityType Name="Customer">
        <Key>
          <PropertyRef Name="Id" />
        </Key>
        <Property Name="Id" Type="Edm.Int32" Nullable="false" />
        <Property Name="Location" Type="WebApiComplexTypeSkipPropertyTest.Address" />
      </EntityType>
      <ComplexType Name="Address">
        <Property Name="Street" Type="Edm.String" />
        <Property Name="City" Type="Edm.String" />
        <Property Name="Country" Type="Edm.String" />
      </ComplexType>
    </Schema>
    <Schema Namespace="Default" xmlns="http://docs.oasis-open.org/odata/ns/edm">
      <EntityContainer Name="Container">
        <EntitySet Name="Customers" EntityType="WebApiComplexTypeSkipPropertyTest.Customer" />
      </EntityContainer>
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>
```

```json
{
  "@odata.context":"http://localhost/odata/$metadata#Customers","value":[
    {
      "Id":1,"Location":{
        "Street":"156th AVE NE","City":"Redmond","Country":"US"
      }
    }
  ]
}
```
Where, "Country" is serialized in the payload.

<h4>Enable</h4>

```xml
<?xml version="1.0" encoding="utf-8"?>
<edmx:Edmx Version="4.0" xmlns:edmx="http://docs.oasis-open.org/odata/ns/edmx">
  <edmx:DataServices>
    <Schema Namespace="WebApiComplexTypeSkipPropertyTest" xmlns="http://docs.oasis-open.org/odata/ns/edm">
      <EntityType Name="Customer">
        <Key>
          <PropertyRef Name="Id" />
        </Key>
        <Property Name="Id" Type="Edm.Int32" Nullable="false" />
        <Property Name="Location" Type="WebApiComplexTypeSkipPropertyTest.Address" />
      </EntityType>
      <ComplexType Name="Address">
        <Property Name="Street" Type="Edm.String" />
        <Property Name="City" Type="Edm.String" />
        <Property Name="Country" Type="Edm.String" />
      </ComplexType>
    </Schema>
    <Schema Namespace="Default" xmlns="http://docs.oasis-open.org/odata/ns/edm">
      <EntityContainer Name="Container">
        <EntitySet Name="Customers" EntityType="WebApiComplexTypeSkipPropertyTest.Customer" />
      </EntityContainer>
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>
```xml

```json
{
  "@odata.context":"http://localhost/odata/$metadata#Customers","value":[
    {
      "Id":1,"Location":{
        "Street":"156th AVE NE","City":"Redmond"
      }
    }
  ]
}

```

Where, "Country" is **NOT** serialized in the payload.
