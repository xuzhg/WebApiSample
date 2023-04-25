# 

This sample is used to build the generic endpoint for type cast.

basically, if you build the routing template as:

```c#
[HttpGet("aws/findings/{entityType}")]
```

The parameter '{entityType}' is treated as key segment. If you want to make it a type cast, you should change that.

You can find the way to enable it in this sample.

## Usage:

You send 'GET' Request as:

```C#
http://localhost:5034/odata/identityGovernance/permissionsAnalytics/aws/findings/PermissionsAnalytics.Models.IdentityFinding
```

You get:
```json
{
    "@odata.context": "http://localhost:5034/odata/$metadata#identityGovernance/PermissionsAnalytics/Aws/Findings/PermissionsAnalytics.Models.IdentityFinding",
    "value": [
        {
            "Id": 2,
            "CreatedDateTime": null,
            "LastActiveDateTime": null,
            "identity": null
        }
    ]
}
```

You send 'GET' Request as:
```C#
http://localhost:5034/odata/identityGovernance/permissionsAnalytics/azure/findings/PermissionsAnalytics.Models.InactiveAwsResourceFinding
```

You get:
```json
{
    "@odata.context": "http://localhost:5034/odata/$metadata#identityGovernance/PermissionsAnalytics/Azure/Findings/PermissionsAnalytics.Models.InactiveAwsResourceFinding",
    "value": [
        {
            "Id": 4,
            "CreatedDateTime": null,
            "LastActiveDateTime": null,
            "identity": null
        }
    ]
}
```

If you have the $expand

```C#
http://localhost:5034/odata/identityGovernance/permissionsAnalytics/azure/findings/PermissionsAnalytics.Models.InactiveAwsResourceFinding?$expand=PermissionsCreepIndex
```

You can get:

```json
{
    "@odata.context": "http://localhost:5034/odata/$metadata#identityGovernance/PermissionsAnalytics/Azure/Findings/PermissionsAnalytics.Models.InactiveAwsResourceFinding(PermissionsCreepIndex())",
    "value": [
        {
            "Id": 4,
            "CreatedDateTime": null,
            "LastActiveDateTime": null,
            "identity": null,
            "PermissionsCreepIndex": {
                "Score": 22
            }
        }
    ]
}
```

## More
<img width="735" alt="image" src="https://user-images.githubusercontent.com/9426627/234164985-eca4b8fe-6b08-4d2e-9f5c-b3b2fdcd8f54.png">
