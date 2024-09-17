Related to: https://stackoverflow.com/questions/78956264/asp-net-core-odata-8-allow-deep-navigation

## Update 09/17/2024 for issue [#63](https://github.com/xuzhg/WebApiSample/issues/63)

I add a new endpoint as:
```C#
http://localhost:5044/v1.0/{**odatapath}
```

It's a catch-all. If you send a request using the correct OData path on 'v1.0', you can get a list of the segment details (for simplicity only). You can achieve more using the details.

Here's the example. 

```C#
http://localhost:5044/v1.0/Services(1)/ServiceArticle(3)
http://localhost:5044/v1.0/Services(1)/ServiceArticle/3
```

You can get:
```json
{
    "@odata.context": "http://localhost:5044/v1.0/$metadata#Collection(Edm.String)",
    "value": [
        "1 |- EntitySet: Services",
        "2 |- Key: ServiceId = 1",
        "3 |- Property: ServiceArticle",
        "4 |- Key: ServiceArticleId = 3"
    ]
}
```

More...
```C#
http://localhost:5044/v1.0/Services(1)/ServiceArticle(3)/Article/Location/City
```

You can get:

```json
{
    "@odata.context": "http://localhost:5044/v1.0/$metadata#Collection(Edm.String)",
    "value": [
        "1 |- EntitySet: Services",
        "2 |- Key: ServiceId = 1",
        "3 |- Property: ServiceArticle",
        "4 |- Key: ServiceArticleId = 3",
        "5 |- Property: Article",
        "6 |- Property: Location",
        "7 |- Property: City"
    ]
}
```

More on function:
```C#
http://localhost:5044/v1.0/Services(1)/ServiceArticle/3/Article/Default.ArticleBoundFunction() 
http://localhost:5044/v1.0/Services(1)/ServiceArticle/3/Article/ArticleBoundFunction()
http://localhost:5044/v1.0/Services(1)/ServiceArticle/3/Article/ArticleBoundFunction
```
You can get:

```json
{
    "@odata.context": "http://localhost:5044/v1.0/$metadata#Collection(Edm.String)",
    "value": [
        "1 |- EntitySet: Services",
        "2 |- Key: ServiceId = 1",
        "3 |- Property: ServiceArticle",
        "4 |- Key: ServiceArticleId = 3",
        "5 |- Property: Article",
        "6 |- BoundOperation: ArticleBoundFunction()"
    ]
}
```



## There's no routing convention to query the single navigation property using the key. This sample creates a convention to generate the endpoint.

So, run it and send:

```http://localhost:5044/$odata``` in the browser:

You can get the following routing table:

![image](https://github.com/user-attachments/assets/60b9a2ad-755b-4955-8125-7008b674d8f9)


## Test for convention routing

```http://localhost:5044/convention/Services(5)/ServiceArticle(8)```

You can get:
```json
{
    "@odata.context": "http://localhost:5044/convention/$metadata#Edm.String",
    "value": "You are calling the convention endpoint using ServiceId=5, ServiceArticleId=8."
}
```


## Test for attribute routing

```http://localhost:5044/attribute/Services(45)/ServiceArticle(12)```

You can get:
```json
{
    "@odata.context": "http://localhost:5044/attribute/$metadata#Edm.String",
    "value": "You are calling the attribute endpoint using ServiceId=45, ServiceArticleId=12.By attribute, only the template in [HttpGet] is in consideration"
}
```
