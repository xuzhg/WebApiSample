# For Issue at: https://github.com/OData/AspNetCoreOData/issues/1432

## Scenario 1
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

## Scenario 2

`GET {{issue1432DollarIt_HostAddress}}/odata/customers(1)?$select=emails($filter=endswith($this,'.com'))`

It gets
```json
{
  "@odata.context": "http://localhost:5171/odata/$metadata#Customers(Emails)/$entity",
  "Emails": [
    "efg.com",
    "xyg.com"
  ]
}
```

`GET {{issue1432DollarIt_HostAddress}}/odata/customers(1)?$select=emails($filter=endswith($it,'.com'))`

It gets
```xmd
{
  "error": {
    "code": "",
    "message": "The query specified in the URI is not valid. No function signature for the function with name 'endswith' matches the specified arguments. The function signatures considered are: endswith(Edm.String Nullable=true, Edm.String Nullable=true).",
    "details": [],
    "innererror": {
      "message": "No function signature for the function with name 'endswith' matches the specified arguments. The function signatures considered are: endswith(Edm.String Nullable=true, Edm.String Nullable=true).",
      "type": "Microsoft.OData.ODataException",
      "stacktrace": "   at Microsoft.OData.UriParser.FunctionCallBinder.MatchSignatureToUriFunction(String functionCallToken, SingleValueNode[] argumentNodes, IList`1 nameSignatures)\r\n   at Microsoft.OData.UriParser.FunctionCallBinder.BindAsUriFunction(FunctionCallToken functionCallToken, List`1 argumentNodes)\r\n   at Microsoft.OData.UriParser.FunctionCallBinder.BindFunctionCall(FunctionCallToken functionCallToken)\r\n   at Microsoft.OData.UriParser.MetadataBinder.BindFunctionCall(FunctionCallToken functionCallToken)\r\n   at Microsoft.OData.UriParser.MetadataBinder.Bind(QueryToken token)\r\n   at Microsoft.OData.UriParser.FilterBinder.BindFilter(QueryToken filter)\r\n   at Microsoft.OData.UriParser.SelectExpandBinder.BindFilter(QueryToken filterToken, IEdmNavigationSource resourcePathNavigationSource, IEdmNavigationSource targetNavigationSource, IEdmTypeReference elementType, HashSet`1 generatedProperties, Boolean collapsed)\r\n   at Microsoft.OData.UriParser.SelectExpandBinder.GenerateSelectItem(SelectTermToken tokenIn)\r\n   at Microsoft.OData.UriParser.SelectExpandBinder.Bind(ExpandToken expandToken, SelectToken selectToken)\r\n   at Microsoft.OData.UriParser.SelectExpandSemanticBinder.Bind(ODataPathInfo odataPathInfo, ExpandToken expandToken, SelectToken selectToken, ODataUriParserConfiguration configuration, BindingState state)\r\n   at Microsoft.OData.UriParser.ODataQueryOptionParser.ParseSelectAndExpandImplementation(String select, String expand, ODataUriParserConfiguration configuration, ODataPathInfo odataPathInfo)\r\n   at Microsoft.OData.UriParser.ODataQueryOptionParser.ParseSelectAndExpand()\r\n   at Microsoft.AspNetCore.OData.Query.SelectExpandQueryOption.get_SelectExpandClause()\r\n   at Microsoft.AspNetCore.OData.Query.Validator.SelectExpandQueryValidator.Validate(SelectExpandQueryOption selectExpandQueryOption, ODataValidationSettings validationSettings)\r\n   at Microsoft.AspNetCore.OData.Query.SelectExpandQueryOption.Validate(ODataValidationSettings validationSettings)\r\n   at Microsoft.AspNetCore.OData.Query.Validator.ODataQueryValidator.Validate(ODataQueryOptions options, ODataValidationSettings validationSettings)\r\n   at Microsoft.AspNetCore.OData.Query.ODataQueryOptions.Validate(ODataValidationSettings validationSettings)\r\n   at Microsoft.AspNetCore.OData.Query.EnableQueryAttribute.ValidateQuery(HttpRequest request, ODataQueryOptions queryOptions)\r\n   at Microsoft.AspNetCore.OData.Query.EnableQueryAttribute.OnActionExecuting(ActionExecutingContext actionExecutingContext)"
    }
  }
}
```


## Scenario 3
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
