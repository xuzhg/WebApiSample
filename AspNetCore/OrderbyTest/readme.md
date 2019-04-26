# Test the Orderby parameter

## Basic $orderby usage

If you query `http://localhost:3012/odata/Customers?$orderby=Name`

```json
{
    "@odata.context": "http://localhost:3012/odata/$metadata#Customers",
    "value": [
        {
            "@odata.type": "#OrderbyTest.Models.SpecialCustomer",
            "Id": 3,
            "Name": "aaa",
            "Token": "token3",
            "Locations": [
                {
                    "Street": "5th AVE"
                },
                {
                    "Street": "98th AVE"
                }
            ]
        },
        {
            "Id": 1,
            "Name": "abc",
            "Locations": [
                {
                    "Street": "148th AVE"
                },
                {
                    "Street": "136th AVE"
                }
            ]
        },
        {
            "@odata.type": "#OrderbyTest.Models.SpecialCustomer",
            "Id": 2,
            "Name": "efj",
            "Token": "token5",
            "Locations": [
                {
                    "Street": "8th AVE"
                },
                {
                    "Street": "88th AVE"
                }
            ]
        }
    ]
}
```


## $orderby in $expand

If you query `http://localhost:3012/odata/Customers?$expand=Orders($orderby=Name)`

```json
{
    "@odata.context": "http://localhost:3012/odata/$metadata#Customers",
    "value": [
        {
            "Id": 1,
            "Name": "abc",
            "Locations": [
                {
                    "Street": "148th AVE"
                },
                {
                    "Street": "136th AVE"
                }
            ],
            "Orders": [
                {
                    "Id": 12,
                    "Name": "OrderIjk"
                },
                {
                    "Id": 11,
                    "Name": "OrderXyz"
                }
            ]
        },
        {
            "@odata.type": "#OrderbyTest.Models.SpecialCustomer",
            "Id": 2,
            "Name": "efj",
            "Token": "token5",
            "Locations": [
                {
                    "Street": "8th AVE"
                },
                {
                    "Street": "88th AVE"
                }
            ],
            "Orders": [
                {
                    "Id": 21,
                    "Name": "OrderEfj"
                },
                {
                    "Id": 22,
                    "Name": "OrderHik"
                }
            ]
        },
        {
            "@odata.type": "#OrderbyTest.Models.SpecialCustomer",
            "Id": 3,
            "Name": "aaa",
            "Token": "token3",
            "Locations": [
                {
                    "Street": "5th AVE"
                },
                {
                    "Street": "98th AVE"
                }
            ],
            "Orders": [
                {
                    "@odata.type": "#OrderbyTest.Models.SpecialOrder",
                    "Id": 32,
                    "Name": "aik",
                    "Price": 8
                },
                {
                    "@odata.type": "#OrderbyTest.Models.SpecialOrder",
                    "Id": 31,
                    "Name": "hfj",
                    "Price": 9
                }
            ]
        }
    ]
}
```

## $orderby with type cast


If you query `http://localhost:3012/odata/Customers?$orderby=OrderbyTest.Models.SpecialCustomer/Token`

It's failed:
```json
{
   ...
   "message": "The query specified in the URI is not valid. The method or operation is not implemented.",
}



## $orderby with type cast in $expand

If you query `http://localhost:3012/odata/Customers?$expand=Orders($orderby=OrderbyTest.Models.SpecialOrder/Price)`

It's failed:
```json
{
   ...
   "message": "The query specified in the URI is not valid. The method or operation is not implemented.",
}

