
`GET {{issue1432DollarIt_HostAddress}}/odata/customers/1/emails`

It gets
```json
{
  "@odata.context": "http://localhost:5171/odata/$metadata#Collection(Edm.String)",
  "value": [
    "abc.org",
    "efg.com",
    "xyg.com"
  ]
}
```



`GET {{issue1432DollarIt_HostAddress}}/odata/customers/1/emails?$filter=endswith($it,'.com')`

It gets
```json
{
  "@odata.context": "http://localhost:5171/odata/$metadata#Collection(Edm.String)",
  "value": [
    "efg.com",
    "xyg.com"
  ]
}
```

`GET {{issue1432DollarIt_HostAddress}}/odata/customers(1)?$expand=orders`

It gets
```json
{
  "@odata.context": "http://localhost:5171/odata/$metadata#Customers(Orders())/$entity",
  "Id": 1,
  "Name": "Sam",
  "Emails": [
    "abc.org",
    "efg.com",
    "xyg.com"
  ],
  "Address": {
    "Street": "120TH AVE",
    "City": "Remond"
  },
  "Orders": [
    {
      "Id": 11,
      "Price": 8,
      "ShipTo": {
        "Street": "120TH AVE",
        "City": "Remond"
      }
    },
    {
      "Id": 12,
      "Price": 43,
      "ShipTo": {
        "Street": "145TH AVE",
        "City": "Issaqu"
      }
    },
    {
      "Id": 13,
      "Price": 18,
      "ShipTo": {
        "Street": "10TH AVE",
        "City": "Bellevue"
      }
    }
  ]
}
```

`GET {{issue1432DollarIt_HostAddress}}/odata/customers(1)?$expand=orders($filter=$it/Address/City%20eq%20ShipTo/City)`

It gets
```json
{
  "@odata.context": "http://localhost:5171/odata/$metadata#Customers(Orders())/$entity",
  "Id": 1,
  "Name": "Sam",
  "Emails": [
    "abc.org",
    "efg.com",
    "xyg.com"
  ],
  "Address": {
    "Street": "120TH AVE",
    "City": "Remond"
  },
  "Orders": [
    {
      "Id": 11,
      "Price": 8,
      "ShipTo": {
        "Street": "120TH AVE",
        "City": "Remond"
      }
    }
  ]
}
```
