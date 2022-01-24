# 

This sample, I added a serializer extension to add the "type annotation" for dynamic property.

By default, in the "minimal" metadata level, the "odata.type" is omitted for dynamic values whose types are clear to client.

However, we can override this default behaviour by setting the type annotation on the property.

## Here's the test 1: (minimal metadata, omit the metadata level means the minimal metadata level)

```C#
http://localhost:5134/odata/customers
```

You can get:
```json
{
    "@odata.context": "http://localhost:5134/odata/$metadata#Customers",
    "value": [
        {
            "Id": 1,
            "Name": "Bracing",
            "DynamicLong@odata.type": "#Int64",
            "DynamicLong": 44,
            "DynamicBool": false,
            "DynamicString": "abc"
        },
        {
            "Id": 2,
            "Name": "Chilly",
            "DynamicGuid@odata.type": "#Guid",
            "DynamicGuid": "6c0aad01-b8a7-45f7-9e8c-c5dad40b8d5b",
            "DynamicByte@odata.type": "#Byte",
            "DynamicByte": 5
        }
    ]
}
```

## Full metadata Test

```c#
http://localhost:5134/odata/customers?$format=application/json;odata.metadata=full
```
you can get:
```json
{
    "@odata.context": "http://localhost:5134/odata/$metadata#Customers",
    "value": [
        {
            "@odata.type": "#AddTypeAnnotationExtensions.Models.Customer",
            "@odata.id": "http://localhost:5134/odata/Customers(1)",
            "@odata.editLink": "Customers(1)",
            "Id": 1,
            "Name": "Bracing",
            "DynamicLong@odata.type": "#Int64",
            "DynamicLong": 44,
            "DynamicBool": false,
            "DynamicString": "abc"
        },
        {
            "@odata.type": "#AddTypeAnnotationExtensions.Models.Customer",
            "@odata.id": "http://localhost:5134/odata/Customers(2)",
            "@odata.editLink": "Customers(2)",
            "Id": 2,
            "Name": "Chilly",
            "DynamicGuid@odata.type": "#Guid",
            "DynamicGuid": "a84115b4-37c8-4eda-a679-c29461fe831e",
            "DynamicByte@odata.type": "#Byte",
            "DynamicByte": 5
        }
    ]
}
```

## none metadata level
```C#
http://localhost:5134/odata/customers?$format=application/json;odata.metadata=none
```

You can get
```json
{
    "value": [
        {
            "Id": 1,
            "Name": "Bracing",
            "DynamicLong@odata.type": "#Int64",
            "DynamicLong": 44,
            "DynamicBool": false,
            "DynamicString": "abc"
        },
        {
            "Id": 2,
            "Name": "Chilly",
            "DynamicGuid@odata.type": "#Guid",
            "DynamicGuid": "a84115b4-37c8-4eda-a679-c29461fe831e",
            "DynamicByte@odata.type": "#Byte",
            "DynamicByte": 5
        }
    ]
}
```