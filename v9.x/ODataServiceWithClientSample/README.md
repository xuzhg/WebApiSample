# ODataServiceWithClientSample

A minimal solution demonstrating an ASP.NET Core OData service consumed by a console OData client.

## Projects

- **ODataService** – ASP.NET Core Web API using `Microsoft.AspNetCore.OData` **9.5.0**.
  Exposes an in-memory `Customers` (and `Orders`) entity set at `http://localhost:5251/odata`
  with full query features enabled (`$filter`, `$select`, `$orderby`, `$expand`, `$top`, etc.).
- **ODataClient** – Console application that acts as an OData client, using `HttpClient`
  to issue several OData queries against the service and print the JSON responses.

## Running

Start the service in one terminal:

```powershell
dotnet run --project ODataService --urls http://localhost:5251
```

Then run the client in another terminal:

```powershell
dotnet run --project ODataClient
```

The client targets `http://localhost:5251/odata` by default. Pass a different service
root as the first argument:

```powershell
dotnet run --project ODataClient -- http://localhost:5251/odata
```
