It's related to this stack overflow question: https://stackoverflow.com/questions/78684713/how-can-i-write-an-odata-query-to-filter-a-property-that-is-two-expands-deep


Here's the test result:

### 1.Query entity set

```C#
http://localhost:5017/odata/Thing1Thing2RelationTable
```

You can get:
```json
{
    "@odata.context": "http://localhost:5017/odata/$metadata#Thing1Thing2RelationTable",
    "value": [
        {
            "Id": 1,
            "Thing2Id": 11
        },
        {
            "Id": 2,
            "Thing2Id": 22
        },
        {
            "Id": 3,
            "Thing2Id": 33
        },
        {
            "Id": 4,
            "Thing2Id": 22
        }
    ]
}
```

### 2. Query using $expand


```C#
http://localhost:5017/odata/Thing1Thing2RelationTable?$expand=Thing1($select=ID,DisplayName;$expand=Attribute)
```

You can get:
```json
{
    "@odata.context": "http://localhost:5017/odata/$metadata#Thing1Thing2RelationTable(Thing1(Id,DisplayName,Attribute()))",
    "value": [
        {
            "Id": 1,
            "Thing2Id": 11,
            "Thing1": {
                "Id": 111,
                "DisplayName": "Foo",
                "Attribute": {
                    "Type": "Float"
                }
            }
        },
        {
            "Id": 2,
            "Thing2Id": 22,
            "Thing1": {
                "Id": 222,
                "DisplayName": "Zoo",
                "Attribute": {
                    "Type": "Decimal"
                }
            }
        },
        {
            "Id": 3,
            "Thing2Id": 33,
            "Thing1": {
                "Id": 333,
                "DisplayName": "Bar",
                "Attribute": {
                    "Type": "Int"
                }
            }
        },
        {
            "Id": 4,
            "Thing2Id": 22,
            "Thing1": {
                "Id": 333,
                "DisplayName": "Tik",
                "Attribute": {
                    "Type": "Money"
                }
            }
        }
    ]
}
```


### 3. Query using $expand and others

```C#
http://localhost:5017/odata/Thing1Thing2RelationTable?$expand=Thing1($select=ID,DisplayName;$expand=Attribute)&$filter=Thing1/Attribute/Type%20in%20(%27Float%27,%20%27Int%27)
```
You can get:
```json
{
    "@odata.context": "http://localhost:5017/odata/$metadata#Thing1Thing2RelationTable(Thing1(Id,DisplayName,Attribute()))",
    "value": [
        {
            "Id": 1,
            "Thing2Id": 11,
            "Thing1": {
                "Id": 111,
                "DisplayName": "Foo",
                "Attribute": {
                    "Type": "Float"
                }
            }
        },
        {
            "Id": 3,
            "Thing2Id": 33,
            "Thing1": {
                "Id": 333,
                "DisplayName": "Bar",
                "Attribute": {
                    "Type": "Int"
                }
            }
        }
    ]
}
```

### 4. More Query using $expand and others

```C#
http://localhost:5017/odata/Thing1Thing2RelationTable?$expand=Thing1($select=ID,DisplayName;$expand=Attribute)&$filter=Thing2Id%20eq%2022%20and%20Thing1/Attribute/Type%20in%20(%27Float%27,%20%27Int%27,%27Money%27,%27Decimal%27)&$orderby=Thing1/DisplayName
```

You can get:

```json
{
    "@odata.context": "http://localhost:5017/odata/$metadata#Thing1Thing2RelationTable(Thing1(Id,DisplayName,Attribute()))",
    "value": [
        {
            "Id": 4,
            "Thing2Id": 22,
            "Thing1": {
                "Id": 333,
                "DisplayName": "Tik",
                "Attribute": {
                    "Type": "Money"
                }
            }
        },
        {
            "Id": 2,
            "Thing2Id": 22,
            "Thing1": {
                "Id": 222,
                "DisplayName": "Zoo",
                "Attribute": {
                    "Type": "Decimal"
                }
            }
        }
    ]
}
```