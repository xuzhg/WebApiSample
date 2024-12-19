https://stackoverflow.com/questions/79295036/the-path-template-x-on-the-action-x-in-controller-x-is-not-a-valid-odata-p


### First Version

If you run the service, you will get the following warning:

```cmd
warn: Microsoft.AspNetCore.OData.Routing.Conventions.AttributeRoutingConvention[0]
      The path template 'odata/GetTagMessages' on the action 'GetTagMessages' in controller 'Message' is not a valid OData path template. Resource not found for the segment 'GetTagMessages'.

```

It's expected because there's no 'GetTagMessages' within the Edm model.