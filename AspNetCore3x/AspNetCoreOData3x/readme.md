# ASP.NET Core OData running on ASP.NET Core 3.0

## Introduction

It's a very simple ASP.NET Core OData Web Application. It targets to ASP.NET Core 3.0!!

## OData package

It's using ASP.NET Core OData 3.0 nightly package at:

https://www.myget.org/feed/webapinetcore/package/nuget/Microsoft.AspNetCore.OData/7.3.0-Nightly201911222308

You can use feed https://www.myget.org/F/webapinetcore/api/v3/index.json at visual studio.

## Functionalities

Only the following functiionalities are added:

1. Query ~/Customers
2. Query ~/Customers({id})
3. Support $select, $expand, ....

For detail discussion, please refer to: https://github.com/OData/WebApi/issues/1748
