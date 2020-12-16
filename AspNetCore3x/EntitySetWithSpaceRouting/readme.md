# Customize the convention routing 

## Introduction

The entity set name has white space.


## Customize the convention routing

Implement `IODataRoutingConvention` to map the request to the controller.

## Test

Send `~/odata/Order Details`

you will get the response with following payload:

```json
{
    "@odata.context": "http://localhost:5000/odata/$metadata#Edm.String",
    "value": "Here's any things"
}
```
