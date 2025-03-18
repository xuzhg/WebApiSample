It's an investigation sampel for https://github.com/OData/AspnetCoreOData/issues/1444

Debug run it, and send Get request as;

`http://localhost:5243/odata/GetTest()`

You will hit the following breaking point.

![image](https://github.com/user-attachments/assets/3647b0da-44a9-4fb0-b313-7ee1c19035d0)

Be noted, the Edm type name of the property `GroupTypes` is  `Collection([System.Collections.Generic.KeyValuePair_2OfInt32_List_1OfInt32 Nullable=True])`

Check the `http://localhost:5243/odata/$metadata`, 

```xml
<?xml version="1.0" encoding="utf-8"?>
  <edmx:Edmx Version="4.0" xmlns:edmx="http://docs.oasis-open.org/odata/ns/edmx">
    <edmx:DataServices>
      <Schema Namespace="Default" xmlns="http://docs.oasis-open.org/odata/ns/edm">
        <ComplexType Name="TestInfoDto">
          <Property Name="GroupTypes" Type="Collection(System.Collections.Generic.KeyValuePair_2OfInt32_List_1OfInt32)" />
        </ComplexType>
        <Function Name="GetTest">
          <ReturnType Type="Collection(Default.TestInfoDto)" />
        </Function>
       <EntityContainer Name="Container">
         <FunctionImport Name="GetTest" Function="Default.GetTest" IncludeInServiceDocument="true" />
       </EntityContainer>
      </Schema>
      <Schema Namespace="System.Collections.Generic" xmlns="http://docs.oasis-open.org/odata/ns/edm">
        <ComplexType Name="KeyValuePair_2OfInt32_List_1OfInt32">
          <Property Name="Value" Type="Collection(Edm.Int32)" Nullable="false" />
        </ComplexType>
      </Schema>
    </edmx:DataServices>
</edmx:Edmx>
```

Be noted:
1) I added 'GetTest()' function to match the route as OData route.
2) The complex type 'KeyValuePair_2OfInt32_List_1OfInt32` contains only the 'Value' property. So, the OData result is like:

```json
{"@odata.context":"http://localhost:5243/odata/$metadata#Default.TestInfoDto","GroupTypes":[{"Value":[20,20,20]},{"Value":[30,30,30]}]}
```

