https://stackoverflow.com/questions/79295036/the-path-template-x-on-the-action-x-in-controller-x-is-not-a-valid-odata-p


### First Version

If you run the service, you will get the following warning:

```cmd
warn: Microsoft.AspNetCore.OData.Routing.Conventions.AttributeRoutingConvention[0]
      The path template 'odata/GetTagMessages' on the action 'GetTagMessages' in controller 'Message' is not a valid OData path template. Resource not found for the segment 'GetTagMessages'.

```

It's expected because there's no 'GetTagMessages' within the Edm model.

### Second version

Create the Edm action and use it in the controller. See the codes:

If you run the service, and send the following request: 
http://localhost:5260/$odata

You can get the endpoint tables:

```
Controller & Action |	HttpMethods|	Template |	IsConventional |
Question79295036AttributeRoutingWarning.Controllers.MessageController.GetTagMessages (Question79295036AttributeRoutingWarning)|	POST|	odata/Message/Default.GetTagMessages |	Yes
Question79295036AttributeRoutingWarning.Controllers.MessageController.GetTagMessages (Question79295036AttributeRoutingWarning)|	POST|	odata/Message/GetTagMessages |	Yes
```

then send Post request: http://localhost:5260/odata/Message/GetTagMessages
Body: application/json

{
    "tagIds": [1, 2, 4]
}

you can get:

```json
{
    "@odata.context": "http://localhost:5260/odata/$metadata#Collection(Question79295036AttributeRoutingWarning.Models.MessageViewModel)",
    "value": [{
        "@odata.type": "#Question79295036AttributeRoutingWarning.Models.MessageViewModel",
        "Id": 1
    }, {
        "@odata.type": "#Question79295036AttributeRoutingWarning.Models.MessageViewModel",
        "Id": 2
    }, {
        "@odata.type": "#Question79295036AttributeRoutingWarning.Models.MessageViewModel",
        "Id": 4
    }]
}
```

No warning because there's no attribute configration.