# ASP.NET Core OData running on ASP.NET Core 3.0

## Introduction

It's a very simple ASP.NET Core OData Web Application. It targets to ASP.NET Core 3.0!!

## OData package

~~It's using ASP.NET Core OData 3.0 nightly package at:~~

~~https://www.myget.org/feed/webapinetcore/package/nuget/Microsoft.AspNetCore.OData/7.3.0-Nightly201911222308~~

~~You can use feed https://www.myget.org/F/webapinetcore/api/v3/index.json at visual studio.~~

Updated 12/13/2019:

It's using ASP.NET Core OData 3.0 beta package at:

https://www.nuget.org/packages/Microsoft.AspNetCore.OData/7.3.0-beta

You can use feed https://www.nuget.org at visual studio.

## Functionalities

Only the following functionalities are added in the sample project:

1. Query ~/Customers
2. Query ~/Customers({id})
3. Support $select, $expand, ....
4. $select enhancement 

1) It supports the $select path, for example: `http://localhost:5000/odata/Customers?$select=HomeAddress/Street`
   
The output is:
```json
{
    "@odata.context": "http://localhost:5000/odata/$metadata#Customers(HomeAddress/Street)",
    "value": [
        {
            "HomeAddress": {
                "Street": "156 AVE NE"
            }
        },
        {
            "HomeAddress": {
                "Street": "Main St NE"
            }
        },
        {
            "HomeAddress": {
                "Street": "Main St NE"
            }
        }
    ]
}
```
   
   
2) It supports the nested $select in the $select, for example: `http://localhost:5000/odata/Customers?$select=HomeAddress($select=Street)`

The output is:
```json
{
    "@odata.context": "http://localhost:5000/odata/$metadata#Customers(HomeAddress)",
    "value": [
        {
            "HomeAddress": {
                "Street": "156 AVE NE"
            }
        },
        {
            "HomeAddress": {
                "Street": "Main St NE"
            }
        },
        {
            "HomeAddress": {
                "Street": "Main St NE"
            }
        }
    ]
}

```

The json ouput is the almost same except the context uri. It's a known issue and will fix in the next ODL release.

For detail discussion, please refer to: https://github.com/OData/WebApi/issues/1748
