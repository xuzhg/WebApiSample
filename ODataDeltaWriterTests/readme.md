This project test the basic OData Delta writer in request and response:

Build and run, you can get:

```json

In ==> response

{
  "@odata.context": "http://www.example.com/$metadata#Customers/$delta",
  "@odata.count": 5,
  "@odata.deltaLink": "Customers?$expand=Orders&$deltatoken=8015",
  "value": [
    {
      "@odata.id": "Customers(1)",
      "CustomerId": 1,
      "Name": "Sam Xu",
      "HomeAddress": {
        "Street": "154TH AVE NE",
        "City": "Redmond"
      }
    },
    {
      "@odata.context": "http://www.example.com/$metadata#Customers/$deletedEntity",
      "id": "Customers(7)",
      "reason": "changed"
    },
    {
      "@odata.context": "http://www.example.com/$metadata#Customers/$deletedEntity",
      "id": "Customers(19)",
      "reason": "deleted"
    }
  ]
}

In ==> request

{
  "@odata.context": "http://www.example.com/$metadata#Customers/$delta",
  "value": [
    {
      "@odata.id": "Customers(1)",
      "CustomerId": 1,
      "Name": "Sam Xu",
      "HomeAddress": {
        "Street": "154TH AVE NE",
        "City": "Redmond"
      }
    },
    {
      "@odata.context": "http://www.example.com/$metadata#Customers/$deletedEntity",
      "id": "Customers(7)",
      "reason": "changed"
    },
    {
      "@odata.context": "http://www.example.com/$metadata#Customers/$deletedEntity",
      "id": "Customers(19)",
      "reason": "deleted"
    }
  ]
}
Hello World!
```