It's an investigation sampel for https://github.com/OData/WebApi/issues/2180

Run it, and send Get request as;

http://localhost:5000/odata/Customers?$filter=Color in ('Red','Blue')

you will get:

```json
{
    "@odata.context": "http://localhost:5000/odata/$metadata#Customers",
    "value": [
        {
            "Id": 1,
            "Color": "Red"
        },
        {
            "Id": 2,
            "Color": "Blue"
        }
    ]
}

```

It's using 7.4.1 version.


