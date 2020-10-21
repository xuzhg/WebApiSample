# Instance annotation in Web API

## Introduction

It's a very simple ASP.NET Core OData Web Application to test instance annotation.


## Metadata

Customer is open type and has instance annotation.

```xml
<?xml version="1.0" encoding="utf-8"?>
<edmx:Edmx Version="4.0" xmlns:edmx="http://docs.oasis-open.org/odata/ns/edmx">
    <edmx:DataServices>
        <Schema Namespace="InstanceAnnotationWebApiTest.Models" xmlns="http://docs.oasis-open.org/odata/ns/edm">
            <EntityType Name="Customer" OpenType="true">
                <Key>
                    <PropertyRef Name="Id" />
                </Key>
                <Property Name="Id" Type="Edm.Int32" Nullable="false" />
                <Property Name="Name" Type="Edm.String" />
                <Property Name="HomeAddress" Type="InstanceAnnotationWebApiTest.Models.Address" />
            </EntityType>
            <ComplexType Name="Address">
                <Property Name="Street" Type="Edm.String" />
                <Property Name="City" Type="Edm.String" />
            </ComplexType>
        </Schema>
        <Schema Namespace="Default" xmlns="http://docs.oasis-open.org/odata/ns/edm">
            <EntityContainer Name="Container">
                <EntitySet Name="Customers" EntityType="InstanceAnnotationWebApiTest.Models.Customer" />
            </EntityContainer>
        </Schema>
    </edmx:DataServices>
</edmx:Edmx>
```

## Query Customers

* `http://localhost:5000/Customers`

```json
{
    "@odata.context": "http://localhost:5000/odata/$metadata#Customers",
    "@NS.selectedcount": 50,
    "@NS.AddressAnnotation": {
        "@odata.type": "#InstanceAnnotationWebApiTest.Models.Address",
        "Street": "228th NE",
        "City": "Sammamish"
    },
    "value": [
        {
            "@NS.ResourceAddress": {
                "@odata.type": "#InstanceAnnotationWebApiTest.Models.Address",
                "Street": "156th AVE",
                "City": "Redmond"
            },
            "@NS.ResourceName": "Sam",
            "Id": 1,
            "Name@NS.DisplayName": "==UI==",
            "Name": "Freezing",
            "StringDynamicProperty": "abc",
            "HomeAddress": null
        },
        {
            "Id": 2,
            "Name": "Hot",
            "IntDynamicProperty": 123,
            "HomeAddress": null
        }
    ]
}
```


## Query single customer

* http://localhost:5000/odata/Customers(1)

```json
{
    "@odata.context": "http://localhost:5000/odata/$metadata#Customers/$entity",
    "@NS.ResourceAddress": {
        "@odata.type": "#InstanceAnnotationWebApiTest.Models.Address",
        "Street": "156th AVE",
        "City": "Redmond"
    },
    "@NS.ResourceName": "Sam",
    "Id": 1,
    "Name@NS.DisplayName": "==UI==",
    "Name": "Freezing",
    "StringDynamicProperty": "abc",
    "HomeAddress": null
}
```