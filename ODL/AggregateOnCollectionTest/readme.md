This is a test for aggregate on collection value properties.

It's a console application, so build and run, you can get:


```cmd
EDM Model in CSDL format:
----------------------------------------------
<?xml version="1.0" encoding="utf-16"?>
<edmx:Edmx Version="4.0" xmlns:edmx="http://docs.oasis-open.org/odata/ns/edmx">
  <edmx:DataServices>
    <Schema Namespace="NS" xmlns="http://docs.oasis-open.org/odata/ns/edm">
      <ComplexType Name="Tag">
        <Property Name="Name" Type="Edm.String" />
        <Property Name="Count" Type="Edm.Int32" Nullable="false" />
        <Property Name="Description" Type="Edm.String" />
      </ComplexType>
      <EntityType Name="Article">
        <Key>
          <PropertyRef Name="Id" />
        </Key>
        <Property Name="Id" Type="Edm.Int32" Nullable="false" />
        <Property Name="Title" Type="Edm.String" />
        <Property Name="Emails" Type="Collection(Edm.String)" />
        <Property Name="Tags" Type="Collection(NS.Tag)" />
      </EntityType>
      <EntityContainer Name="DefaultContainer">
        <EntitySet Name="Articles" EntityType="NS.Article" />
      </EntityContainer>
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>
----------------------------------------------
Register Custom Functions:
Custom Functions Registered.

Press any key to continue to OData Uri parsing...

^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Do OData Uri parsing:  ~/Articles?$apply=aggregate(Emails with Microsoft.Combined as CombinedEmails)
Transformation Kind: Aggregate
  Aggregation: CollectionPropertyAggregate
    CollectionPropertyAccess Name: Emails
    CollectionPropertyAccess Type: Collection(Edm.String)
    MethodKind: Custom
    MethodName: Microsoft.Combined

^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Do OData Uri parsing:  ~/Articles?$apply=aggregate(Tags with Microsoft.Union as DistinctTags)
Transformation Kind: Aggregate
  Aggregation: CollectionPropertyAggregate
    CollectionComplexNode Name: Tags
    CollectionComplexNode Type: Collection(NS.Tag)
    MethodKind: Custom
    MethodName: Microsoft.Union

^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Done!
```


## Be noted, it's using the nightly bit. The nightly bit could be deleted soon. So, I copy and past the bit into the .\Packages folder.

