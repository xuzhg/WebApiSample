# Introduce

This project test the basic OData Delta writer in request and response:

Build and run, you can get:


## In ==> response            OData Version V4

```json
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
```

## In ==> request            OData Version V4

```json
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
```
## In ==> response            OData Version V401

```json
{
  "@context": "http://www.example.com/$metadata#Customers/$delta",
  "@count": 5,
  "@deltaLink": "Customers?$expand=Orders&$deltatoken=8015",
  "value": [
    {
      "@id": "Customers(1)",
      "CustomerId": 1,
      "Name": "Sam Xu",
      "HomeAddress": {
        "Street": "154TH AVE NE",
        "City": "Redmond"
      }
    },
    {
      "@removed": {
        "reason": "changed"
      },
      "@id": "Customers(7)",
      "CustomerId": 7,
      "Name": "Peter"
    },
    {
      "@removed": {
        "reason": "deleted"
      },
      "@id": "Customers(19)"
    }
  ]
}
```

## In ==> request            OData Version V401

```
{
  "@context": "http://www.example.com/$metadata#Customers/$delta",
  "value": [
    {
      "@id": "Customers(1)",
      "CustomerId": 1,
      "Name": "Sam Xu",
      "HomeAddress": {
        "Street": "154TH AVE NE",
        "City": "Redmond"
      }
    },
    {
      "@removed": {
        "reason": "changed"
      },
      "@id": "Customers(7)",
      "CustomerId": 7,
      "Name": "Peter"
    },
    {
      "@removed": {
        "reason": "deleted"
      },
      "@id": "Customers(19)"
    }
  ]
}
```
Hello World!
