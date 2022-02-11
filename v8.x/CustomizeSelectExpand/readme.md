# Test how to get the result for $select and $expand in the controller

```C#
http://localhost:5000/weatherforecast?$select=summary&$expand=departs($select=Name;$top=2)
```

you can get:
```json
[
    {
        "Departs": [
            {
                "Name": "Mild"
            },
            {
                "Name": "Freezing"
            }
        ],
        "Summary": "Cool"
    },
    {
        "Departs": [
            {
                "Name": "Bracing"
            },
            {
                "Name": "Chilly"
            }
        ],
        "Summary": "Balmy"
    },
    {
        "Departs": [
            {
                "Name": "Chilly"
            },
            {
                "Name": "Freezing"
            }
        ],
        "Summary": "Balmy"
    },
    {
        "Departs": [
            {
                "Name": "Bracing"
            },
            {
                "Name": "Bracing"
            }
        ],
        "Summary": "Freezing"
    },
    {
        "Departs": [
            {
                "Name": "Scorching"
            },
            {
                "Name": "Cool"
            }
        ],
        "Summary": "Bracing"
    }
]
```