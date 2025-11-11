# That's for issue: https://github.com/OData/AspNetCoreOData/issues/1535

# How to support filtering case insensitivity

OData supports the case insensitivity for the key words, but not for the instance value.

so, $filter=Name eq 'Dog' should match the 'Dog' exactly. 

This sample plays the case insensitivity by customizing the FilterBinder in AspNetCore.OData, which is based on SqlLite. Of course, it can apply for other DBs.

Run this sample, and open 'CaseInsensitiveFilterSample.http' file and click 'Send Request' for

`GET {{CaseInsensitiveFilterSample_HostAddress}}/odata/customers?$filter=LastName%20eq%20'dog'`

You can see the following result:

```json
{
  "@odata.context": "http://localhost:5259/odata/$metadata#Customers",
  "value": [
    {
      "CustomerId": 1,
      "FirstName": "Mercury",
      "LastName": "Dog",
      "Address": null
    },
    {
      "CustomerId": 4,
      "FirstName": "Mars",
      "LastName": "dog",
      "Address": null
    },
    {
      "CustomerId": 5,
      "FirstName": "Jupiter",
      "LastName": "doG",
      "Address": null
    }
  ]
}
```

Here's the SqlLite statement:
<img width="1085" height="101" alt="image" src="https://github.com/user-attachments/assets/0e8c4a8b-b0d9-4fae-a3a5-7010ed8e120c" />

Of course, you can try the 'NoEqual' by:

`GET {{CaseInsensitiveFilterSample_HostAddress}}/odata/customers?$filter=LastName%20ne%20'dog'`

