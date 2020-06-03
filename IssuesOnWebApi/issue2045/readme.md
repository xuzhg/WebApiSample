It's an investigation sampel for https://github.com/OData/WebApi/issues/2045

Run it, and send Get request as;

# Bound/unbound action call:

1) bound action: namespace + action namespace
2) unbound action: unbound action name

## 1

POST http://localhost:5000/odata/InspectionDuty/Default.SingleChange or
http://localhost:5000/odata/UnboundSingleChange
Head: 
    Content-Type: application/json
Body:
   
```json
{
   "Comment": "928D877A-D9C7-4699-B0E9-2817CB854881",
   "Change": {}
}
```

you will get:

```json
{
    "@odata.context": "http://localhost:5000/odata/$metadata#Edm.String",
    "value": "Received both parameters."
}

```

## 1

POST http://localhost:5000/odata/InspectionDuty/Default.SingleChange or
http://localhost:5000/odata/UnboundSingleChange
Head: 
    Content-Type: application/json
Body:
   
```json
{
   "Comment": "928D877A-D9C7-4699-B0E9-2817CB854881",
   "Change": {
   	 "Operations": "Update",
   	 "Identity": "928D877A-D9C7-4699-B0E9-2817CB854881",
   	 "Interval": {
   	 	"Step": 42,
   	 	"Size": "Month"
   	 }
   }
}
```

you will get:

```json
{
    "@odata.context": "http://localhost:5000/odata/$metadata#Edm.String",
    "value": "Received both parameters."
}

```

at debug side, the "ODataActionParameters" will have two items.
