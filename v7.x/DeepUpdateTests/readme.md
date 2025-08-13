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


