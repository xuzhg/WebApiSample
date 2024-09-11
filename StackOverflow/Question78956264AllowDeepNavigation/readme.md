Related to: https://stackoverflow.com/questions/78956264/asp-net-core-odata-8-allow-deep-navigation

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
