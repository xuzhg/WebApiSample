It works to put a collection of entity:

```C#
PUT http://localhost:1923/odata/Messages(1)/Likes
```

```json
[
        {
            "Id": 11,
            "Reason": "Good"
        },
        {
            "Id": 12,
            "Reason": "Bad"
        }
]

```
Simply return the value itself:

```json
{
    "@odata.context": "http://localhost:1923/odata/$metadata#Collection(CustomODataRouting.Models.Like)",
    "value": [
        {
            "Id": 11,
            "Reason": "Good"
        },
        {
            "Id": 12,
            "Reason": "Bad"
        }
    ]
}
```

