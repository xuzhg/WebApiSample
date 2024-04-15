### This is related to this issue at: https://github.com/OData/AspNetCoreOData/issues/1210
---
I'd use this sample to illustrate how to pass `Complex object` in the ODATA function


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
Where, [FromODataUri] is required.  Here's a snapshot:

![image](https://github.com/xuzhg/WebApiSample/assets/9426627/2026d208-7073-4496-9542-38efb593651e)

The only problem is that the request Uri is longer than expected.

of course, you can use the parameter alias like:

```C#
http://localhost:5219/odata/customers/MyFunction3(d=@p,k=7)?@p={"RequiredConfiguration": false,"Configuration": ["a","b"],"RequiredFullInformation": true,"FullInformation": [],"ServerInformation": true,"RequiredSummary": true,"RequiredConnectionCheck": false}
```

It should work as expected.


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

![image](https://github.com/xuzhg/WebApiSample/assets/9426627/4aa161f6-1b95-4696-904b-7bc37a362ca3)


The problem is that the request Uri should contain the 'dummy' parameter,for example: (d='d') to match the endpoint.

Maybe we can config the 'd' as optional parameter. I'd leave it to 'YOU'.

## 3, customize the routing

In order to get rid of the 'dummy' parameter, we can customize the routing. Please refer to the sample

```C#
http://localhost:5219/odata/MyFunction(k=18)
```

At controller side, you should have an action to handle the above request:

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

![image](https://github.com/xuzhg/WebApiSample/assets/9426627/a3c12b33-2711-44b1-9301-dd4e2f6ab16e)

