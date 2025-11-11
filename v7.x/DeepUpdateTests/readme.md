See updates at 09/09/2025 from https://github.com/xuzhg/WebApiSample/tree/main/v7.x/DeepUpdateTests#updated-at-09092025: 

For 7.x version, 
Build and run the app, then use the '[.\DeepUpdateTests.http](https://github.com/xuzhg/WebApiSample/blob/main/v7.x/DeepUpdateTests/DeepUpdateTests/DeepUpdateTests.http)' to test

## Without OData-Version header
```cmd
PATCH {{DeepUpdateTests_HostAddress}}/odata/Customers(1)
Content-Type: application/json
```

```json
{
    "Name": "Updated Customer",
    "Orders@odata.delta": [
        {
            "Title": "Updated Order",
            "Amount": 150
        },
        {
         "@removed":{"reason":"deleted" },
         "@id":"Orders(13)"
        },
        {
          "@id":"Orders(42)",
          "Title":"Microsoft",
          "Amount": 300
       }
  ]
}
```

At debug side, you can 
<img width="1743" height="585" alt="image" src="https://github.com/user-attachments/assets/6c8a8728-89b7-4ec9-a1ce-cfe3fd971a9e" />

All are 'entry'

## With OData-Version header
```cmd
PATCH {{DeepUpdateTests_HostAddress}}/odata/Customers(1)
Content-Type: application/json
OData-Version: 4.01
```

```json
{
    "Name": "Updated Customer",
    "Orders@odata.delta": [
        {
            "Title": "Updated Order",
            "Amount": 150
        },
        {
         "@removed":{"reason":"deleted" },
         "@id":"Orders(13)"
        },
        {
          "@id":"Orders(42)",
          "Title":"Microsoft",
          "Amount": 300
       }
  ]
}
```

At debug side, you can 
<img width="1897" height="331" alt="image" src="https://github.com/user-attachments/assets/1973d800-9b6c-48ca-8e51-712f64f870aa" />


You can see the second one is 'DeltaDeletedEntityObject'.


# Updated at 09/09/2025

Let's test the deep delta writing. It's not fully supported in 7.x, but it seems it works fine at 9.x.

Go to '[DeepUpdateTests_9x]((https://github.com/xuzhg/WebApiSample/edit/main/v7.x/DeepUpdateTests/DeepUpdateTests_9x)', run and use .http file to test:

`GET {{DeepUpdateTests_9x_HostAddress}}/odata/Customers(1)/orders?$expand=list&$deltaToken=abcd`

You can get

```json
{
  "@odata.context": "http://localhost:5136/odata/$metadata#Orders/$delta",
  "value": [
    {
      "Id": 121,
      "Items@delta": [
        {
          "Id": 0,
          "Description": "ChangedDescription"
        }
      ]
    },
    {
      "@odata.removed": {
        "reason": "deleted"
      },
      "@odata.id": "http://localhost/odata/orders(3)",
      "Id": 0
    }
  ]
}
```


# Updated at 09/22/2025

Update after 9/9/2025. For Web API 7.x, if you want to write the change set, you should specify the version to 4.01.


Send the following request for 
```cmd
GET {{DeepUpdateTests_HostAddress}}/odata/Customers(1)/orders?$expand=list&$deltaToken=abcd
OData-Version: 4.01
```

You can get the following payload:


```json
{
  "@context": "http://localhost:5048/odata/$metadata#Orders/$delta",
  "value": [
    {
      "Id": 1
    },
    {
      "@removed": {
        "reason": "deleted"
      },
      "@id": "http://tempuri.org/Orders(2)",
      "Id": 2,
      "Amount": 0
    },
    {
      "Id": 1,
      "Amount": 8,
      "Items@delta": [
        {
          "Id": 1
        }
      ]
    }
  ]
}
```

