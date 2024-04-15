### This is related to this issue at: https://github.com/OData/AspNetCoreOData/issues/1210

I'd use this sample to illustrate how to pass `Complex object` in the ODATA function
---

## 1, use the request route data directly

You can use the 'JSON data' directly in the request Uri as:


```C#
http://localhost:5219/odata/customers/MyFunction3(d={"RequiredConfiguration": false,"Configuration": ["a","b"],"RequiredFullInformation": true,"FullInformation": [],"ServerInformation": true,"RequiredSummary": true,"RequiredConnectionCheck": false},k=7)
```

At controller side, you should have an action to handle the above request (using the conventional routing):

```C#
[HttpGet]
public IActionResult MyFunction3(int k, [FromODataUri] Detail d)
{
    d.KFromFunctionCall = k;
    return Ok(d);
}
```
Where, [FromODataUri] is required. 

The only problem is that the request Uri is longer than expected.

## 2, use the request body data

You can use the 'JSON data' directly in the request body as:

```C#
http://localhost:5219/odata/customers/MyFunction2(d='d',k=8)
```

At controller side, you should have an action to handle the above request (using the conventional routing):

```C#
[HttpGet]
public IActionResult MyFunction2(int k, [FromBody]Detail d)
{
    d.KFromFunctionCall = k;
    return Ok(d);
}
```

Where, [FromBody] is required. 

The problem is that the request Uri should contains the 'dummy' parameter,for example: (d='d') to match the endpoint.


## 3, customize the routing

In order to get rid of the 'dummy' parameter, we can customize the routing. Please refer to the sample

```C#
http://localhost:5219/odata/MyFunction(k=18)
```

At controller side, you should have an action to handle the above request (using the conventional routing):

```C#
[HttpGet]
[ODataFunction("odata")]
public IActionResult MyFunction([FromRoute]int k, [FromBody]Detail detail)
{
    detail.KFromFunctionCall = k;
    return Ok(detail);
}
```

Where, 'ODataFunction' is the customized attribute to build the endpoint.

Where, [FromRoute] for 'k' is required. 
