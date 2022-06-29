## Azure Function with OData 7

It's a demo to test OData 7.5.x on Azure Function.

If you build and run it locally, 

1) Send Get `http://localhost:7071/api/v1/cn/customers?$select=name,Id&$filter=Id lt 4&$top=2&$count=true`

you can get:
```json
{
    "@odata.context": "http://localhost:7071/api/v1/cn/$metadata#Customers(Name,Id)",
    "@odata.count": 3,
    "value": [
        {
            "Id": 1,
            "Name": "Sam"
        },
        {
            "Id": 2,
            "Name": "Liu"
        }
    ]
}
```


2) Send Get `http://localhost:7071/api/v1/us/customers?$select=name,Id&$filter=Id lt 4&$top=2&$count=true`

you can get:

```json
{
    "@odata.context": "http://localhost:7071/api/v1/us/$metadata#Customers(Name,Id)",
    "@odata.count": 3,
    "value": [
        {
            "Id": 1,
            "Name": "Peter"
        },
        {
            "Id": 2,
            "Name": "Kate"
        }
    ]
}
```