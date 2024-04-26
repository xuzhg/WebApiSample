ASP.NET Core OData v8 builds the endpoints ahead (during startup) which makes every endpoints declared explicitly and be there for whole application life.

Customers benefit from this mechanisam for simple request, however sometime when the endpoint number is huge, it could be a problem. 

There are many ways to reduce the endpoint number. This sample illustrates a way to use 'dynamic controller' to avoid build the OData endpoint ahead.

## Endpoints overview

Here's endpoints created ahead. You can there's no OData endpoint created.
<img width="1893" alt="image" src="https://github.com/xuzhg/WebApiSample/assets/9426627/db045f1a-6d7f-45ff-b9f7-54c96ee00d88">

## Query entityset

I created an action as below to handle "entityset query"
```
[HttpGet]
[EnableQuery]
[ODataRoute("{entityset}")]
public IActionResult Get(string entitySet)
```

So, you can send the follow (4) requests to get "Customers, Orders, People":

<img width="1768" alt="image" src="https://github.com/xuzhg/WebApiSample/assets/9426627/e6c056f7-0f25-4a50-ad95-8156ea53bed2">

## Query single entity

I created an action as below to handle "single entity query"

```
[HttpGet]
[EnableQuery]
[ODataRoute("{entityset}({key})")]
public IActionResult Get(string entitySet, int key)
```

So, you can send the follow (4) requests to get entity:

<img width="1539" alt="image" src="https://github.com/xuzhg/WebApiSample/assets/9426627/099c548d-7fb0-4a51-8c20-009969a9641c">

## Query property

I created two actions as below to handle "property query"

### 1. using static string
```C#
[HttpGet]
[ODataRoute("v1", "customers/{key}/Address")]
public IActionResult GetAddress(int key)
```

### 2. using template string
```C#
[HttpGet]
[ODataRoute("v2", "{entityset}({key})/{property}")]
public IActionResult Get(string entitySet, int key, string property)
```

<img width="1053" alt="image" src="https://github.com/xuzhg/WebApiSample/assets/9426627/545c84a9-544c-4bb9-bf42-710996c0b953">

You can see, if you try to query "v1/people(2)/emails", it's not found.

## Query options

I enabled the OData query ($select only for your reference, you can enable more) for 'v1' as:


```C#
app.MapODataRoute("v1", q => q.EnableSelect = true);
app.MapODataRoute("v2");
```
Here's the requests:
<img width="1341" alt="image" src="https://github.com/xuzhg/WebApiSample/assets/9426627/978d440d-fa9f-40d0-9575-f5b0cf63aa27">




