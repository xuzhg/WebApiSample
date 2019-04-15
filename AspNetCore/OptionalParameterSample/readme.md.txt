# Test the optional parameter

## Optional vs Nullable parameter

* Optional means you can omit to provide the value for such parameter
* Nullable means the parameter can be null or not.

## Optional & Nullable in metadata

If you query `http://localhost:5000/odata/$metadata`

```xml
<?xml version="1.0" encoding="utf-8"?>
<edmx:Edmx Version="4.0" xmlns:edmx="http://docs.oasis-open.org/odata/ns/edmx">
    <edmx:DataServices>
        <Schema Namespace="OptionalParameterSample" xmlns="http://docs.oasis-open.org/odata/ns/edm">
            <EntityType Name="User">
                <Key>
                    <PropertyRef Name="Id" />
                </Key>
                <Property Name="Id" Type="Edm.Int32" Nullable="false" />
            </EntityType>
            <ComplexType Name="AppTile">
                <Property Name="Name" Type="Edm.String" />
            </ComplexType>
        </Schema>
        <Schema Namespace="Default" xmlns="http://docs.oasis-open.org/odata/ns/edm">
            <Function Name="getAssignedAppTiles" IsBound="true">
                <Parameter Name="bindingParameter" Type="OptionalParameterSample.User" />
                <Parameter Name="param" Type="Edm.String" />
                <Parameter Name="includeOfficeFirstParty" Type="Edm.Boolean">
                    <Annotation Term="Org.OData.Core.V1.OptionalParameter">
                        <Record>
                            <PropertyValue Property="DefaultValue" String="false" />
                        </Record>
                    </Annotation>
                </Parameter>
                <ReturnType Type="OptionalParameterSample.AppTile" />
            </Function>
            <EntityContainer Name="Container">
                <EntitySet Name="Users" EntityType="OptionalParameterSample.User" />
            </EntityContainer>
        </Schema>
    </edmx:DataServices>
</edmx:Edmx>
```

You can find: 
* `param` is nullable parameter, everytime to call `getAssignedAppTiles`, you should provide a value for it, can be null.
* `includeOfficeFirstParty` is nullable and optional parameter (check the OptionalParameter annotation). you can omit it when call `getAssignedAppTiles`

## Usage of optional parameter

1.  http://localhost:5000/odata/Users(2)/getAssignedAppTiles(param='abc')

You can get 
```json
{
    "@odata.context": "http://localhost:5000/odata/$metadata#Edm.String",
    "value": "User Key = 2, param = abc, includeOfficeFirstParty = "
}
```

2. http://localhost:5000/odata/Users(2)/getAssignedAppTiles(param='abc', includeOfficeFirstParty=true)

you can get
```json
{
    "@odata.context": "http://localhost:5000/odata/$metadata#Edm.String",
    "value": "User Key = 2, param = abc, includeOfficeFirstParty = True"
}
```

3. http://localhost:5000/odata/Users(2)/getAssignedAppTiles(includeOfficeFirstParty=true)

You get nothing.

