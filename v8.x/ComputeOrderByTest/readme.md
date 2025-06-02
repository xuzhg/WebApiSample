## A sample for issue https://github.com/OData/AspNetCoreOData/issues/1483:

Run the sample and go to 'ComputeOrderByTest.http' to test the request/response

## 1  No query
`GET {{ComputeOrderByTest_HostAddress}}/odata/customers`

```json
{
  "@odata.context": "http://localhost:5182/odata/$metadata#Customers",
  "value": [
    {
      "Id": 1,
      "Name": "Freezing",
      "Age": 34
    },
    {
      "Id": 2,
      "Name": "Bracing",
      "Age": 54
    },
    {
      "Id": 3,
      "Name": "Chilly",
      "Age": 10
    },
    {
      "Id": 4,
      "Name": "Cool",
      "Age": 8
    },
    {
      "Id": 5,
      "Name": "Mild",
      "Age": 19
    }
  ]
}
```


## 2 With $compute & $select query

`GET {{ComputeOrderByTest_HostAddress}}/odata/customers?$compute=age add 10 as agePlusTen&$select=id,name,agePlusTen`

```json
{
  "@odata.context": "http://localhost:5182/odata/$metadata#Customers(Id,Name,agePlusTen)",
  "value": [
    {
      "Id": 1,
      "Name": "Freezing",
      "agePlusTen": 44
    },
    {
      "Id": 2,
      "Name": "Bracing",
      "agePlusTen": 64
    },
    {
      "Id": 3,
      "Name": "Chilly",
      "agePlusTen": 20
    },
    {
      "Id": 4,
      "Name": "Cool",
      "agePlusTen": 18
    },
    {
      "Id": 5,
      "Name": "Mild",
      "agePlusTen": 29
    }
  ]
}
```


## 3 With $orderby and $filter on computed property

`GET {{ComputeOrderByTest_HostAddress}}/odata/customers?$compute=age add 10 as agePlusTen&$orderby=agePlusTen desc&$select=id,name,agePlusTen&$filter=agePlusTen ge 21`

```json
{
  "@odata.context": "http://localhost:5182/odata/$metadata#Customers(Id,Name,agePlusTen)",
  "value": [
    {
      "Id": 2,
      "Name": "Bracing",
      "agePlusTen": 64
    },
    {
      "Id": 1,
      "Name": "Freezing",
      "agePlusTen": 44
    },
    {
      "Id": 5,
      "Name": "Mild",
      "agePlusTen": 29
    }
  ]
}
```

