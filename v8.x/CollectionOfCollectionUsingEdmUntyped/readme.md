This is related to issue at: "[Serialization of int[][] does not work](https://github.com/OData/AspNetCoreOData/issues/1386)"

Basically, OData spec doesn't define the part for collection of collection, since the type of collection of collection of primitive cannot be presented.
So, Collection of collection is not supported. But, you can achieve it using a lot of ways.

1) Create a complex type contains a collection of primitive, then use this complex type to define a collection.
2) or using Edm.Untyped, see details about Edm.Untyped [here](https://devblogs.microsoft.com/odata/enable-un-typed-within-asp-net-core-odata/)

This sample illustrates the usage of Edm.Untyped for collection of collection.

# 1 Metadata

If you send request to: "http://localhost:5157/odata/$metadata"
You can get the following metadata:

```xml
<edmx:Edmx xmlns:edmx="http://docs.oasis-open.org/odata/ns/edmx" Version="4.0">
  <edmx:DataServices>
    <Schema xmlns="http://docs.oasis-open.org/odata/ns/edm" Namespace="CollectionOfCollectionUsingEdmUntyped.Models">
      <EntityType Name="PlaneDto">
        <Key>
          <PropertyRef Name="Id"/>
        </Key>
        <Property Name="Id" Type="Edm.Guid" Nullable="false"/>
        <Property Name="Normal" Type="Collection(Edm.Double)" Nullable="false"/>
        <Property Name="Point" Type="Collection(Edm.Double)" Nullable="false"/>
        <Property Name="Points" Type="Collection(Edm.Double)" Nullable="false"/>
        <Property Name="Contours" Type="Edm.Untyped"/>
      </EntityType>
      </Schema>
      <Schema xmlns="http://docs.oasis-open.org/odata/ns/edm" Namespace="Default">
        <EntityContainer Name="Container">
          <EntitySet Name="Planes" EntityType="CollectionOfCollectionUsingEdmUntyped.Models.PlaneDto"/>
        </EntityContainer>
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>
```


# 2 Query data

If you send request to: "http://localhost:5157/odata/planes(775F9E1A-97EB-457C-B560-880330C69997)"

Then you can get:

```json
{
    "@odata.context": "http://localhost:5157/odata/$metadata#Planes/$entity",
    "Id": "775f9e1a-97eb-457c-b560-880330c69997",
    "Normal": [
        -0.99999999974639,
        0.00000822496291971287,
        0.0000209659328342425
    ],
    "Point": [
        -22.4877607468324,
        0.000184960998336979,
        0.000471476881530173
    ],
    "Points": [
        -22.4822136189295,
        -13.6948240459868,
        269.9512
    ],
    "Contours": [
        [
            1,
            2,
            3
        ],
        [
            2,
            3,
            4
        ]
    ]
}
```
