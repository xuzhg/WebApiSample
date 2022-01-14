This sample plays the show/hidden properties.

1) If you send request "GET http://localhost:49330/odata/Customers(1)"

you will get

```json
{
    "@odata.context": "http://localhost:49330/odata/$metadata#Customers/$entity",
    "Id": 2,
    "Name": "Kerry",
    "Age": 4,
    "Nums": [
        3,
        7,
        9
    ],
    "Emails": [
        "111@abc.com",
        "xyz@xyz.com"
    ],
    "HomeAdress": {
        "City": "kerryCity",
        "Street": "kerryStreet"
    },
    "NoHiddenAddress": {
        "City": "nohidden-kerryCity",
        "Street": "nohidden-kerryStreet"
    },
    "MailAddresses": [
        {
            "City": "kerryMailCity1",
            "Street": "kerryMailStreet1"
        },
        {
            "City": "kerryMailCity2",
            "Street": "kerryMailStreet2"
        }
    ]
}
```

2) if you send the same request with header:
http://localhost:49330/odata/Customers(1)
Priviledged: true

You will get:
```json
{
    "@odata.context": "http://localhost:49330/odata/$metadata#Customers/$entity",
    "Id": 2,
    "Name": "Kerry",
    "Emails": [
        "111@abc.com",
        "xyz@xyz.com"
    ],
    "NoHiddenAddress": {
        "City": "nohidden-kerryCity",
        "Street": "nohidden-kerryStreet"
    }
}
```

3) Here's the metadata:

```xml
<?xml version="1.0" encoding="utf-8"?>
<edmx:Edmx Version="4.0" xmlns:edmx="http://docs.oasis-open.org/odata/ns/edmx">
    <edmx:DataServices>
        <Schema Namespace="AspNetClassicOData.Models" xmlns="http://docs.oasis-open.org/odata/ns/edm">
            <EntityType Name="Customer">
                <Key>
                    <PropertyRef Name="Id" />
                </Key>
                <Property Name="Id" Type="Edm.Int32" Nullable="false" />
                <Property Name="Name" Type="Edm.String" />
                <Property Name="Age" Type="Edm.Int32" Nullable="false">
                    <Annotation Term="NS.IsShowHidden" Bool="true" />
                </Property>
                <Property Name="Nums" Type="Collection(Edm.Int32)" Nullable="false">
                    <Annotation Term="NS.IsShowHidden" Bool="true" />
                </Property>
                <Property Name="Emails" Type="Collection(Edm.String)" />
                <Property Name="HomeAdress" Type="AspNetClassicOData.Models.Address">
                    <Annotation Term="NS.IsShowHidden" Bool="true" />
                </Property>
                <Property Name="NoHiddenAddress" Type="AspNetClassicOData.Models.Address" />
                <Property Name="MailAddresses" Type="Collection(AspNetClassicOData.Models.Address)">
                    <Annotation Term="NS.IsShowHidden" Bool="true" />
                </Property>
                <NavigationProperty Name="SingleOrder" Type="AspNetClassicOData.Models.Order">
                    <Annotation Term="NS.IsShowHidden" Bool="true" />
                </NavigationProperty>
                <NavigationProperty Name="Orders" Type="Collection(AspNetClassicOData.Models.Order)">
                    <Annotation Term="NS.IsShowHidden" Bool="true" />
                </NavigationProperty>
            </EntityType>
            <ComplexType Name="Address">
                <Property Name="City" Type="Edm.String" />
                <Property Name="Street" Type="Edm.String" />
            </ComplexType>
            <EntityType Name="Order">
                <Key>
                    <PropertyRef Name="Id" />
                </Key>
                <Property Name="Id" Type="Edm.Int32" Nullable="false" />
                <Property Name="Title" Type="Edm.String" />
            </EntityType>
        </Schema>
        <Schema Namespace="Default" xmlns="http://docs.oasis-open.org/odata/ns/edm">
            <EntityContainer Name="Container">
                <EntitySet Name="Customers" EntityType="AspNetClassicOData.Models.Customer" />
            </EntityContainer>
        </Schema>
        <Schema Namespace="NS" xmlns="http://docs.oasis-open.org/odata/ns/edm">
            <Term Name="IsShowHidden" Type="Edm.Boolean" />
        </Schema>
    </edmx:DataServices>
</edmx:Edmx>
```
