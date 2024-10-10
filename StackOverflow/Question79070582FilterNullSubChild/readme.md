https://stackoverflow.com/questions/79070582/filter-using-child-fields-to-return-parent-with-non-empty-nodes


### Get all workers
```C#
http://localhost:5269/odata/Workers
```

Here's the result:

```json
{
    "@odata.context": "http://localhost:5269/odata/$metadata#Workers",
    "value": [
        {
            "Id": 1,
            "Name": "Sam"
        },
        {
            "Id": 2,
            "Name": "Wu"
        }
    ]
}
```

### Get all workers and expand all subs
```C#
http://localhost:5269/odata/Workers?$expand=AbsenceHeaders($expand=AbsenceDailyDetails)
```

Here's the result
```json
{
    "@odata.context": "http://localhost:5269/odata/$metadata#Workers(AbsenceHeaders(AbsenceDailyDetails()))",
    "value": [
        {
            "Id": 1,
            "Name": "Sam",
            "AbsenceHeaders": [
                {
                    "AbsenceHeaderId": 111,
                    "AbsenceDailyDetails": [
                        {
                            "AbsenceDailyDetailId": 1111,
                            "Date": "2024-10-01",
                            "AbsenceHeaderId": 111,
                            "IsAbsentDay": true
                        },
                        {
                            "AbsenceDailyDetailId": 1112,
                            "Date": "2024-10-02",
                            "AbsenceHeaderId": 111,
                            "IsAbsentDay": false
                        }
                    ]
                },
                {
                    "AbsenceHeaderId": 122,
                    "AbsenceDailyDetails": []
                },
                {
                    "AbsenceHeaderId": 133,
                    "AbsenceDailyDetails": []
                }
            ]
        },
        {
            "Id": 2,
            "Name": "Wu",
            "AbsenceHeaders": [
                {
                    "AbsenceHeaderId": 222,
                    "AbsenceDailyDetails": [
                        {
                            "AbsenceDailyDetailId": 111,
                            "Date": "2025-10-01",
                            "AbsenceHeaderId": 11,
                            "IsAbsentDay": true
                        },
                        {
                            "AbsenceDailyDetailId": 112,
                            "Date": "2026-10-02",
                            "AbsenceHeaderId": 11,
                            "IsAbsentDay": false
                        }
                    ]
                },
                {
                    "AbsenceHeaderId": 222,
                    "AbsenceDailyDetails": []
                },
                {
                    "AbsenceHeaderId": 233,
                    "AbsenceDailyDetails": []
                }
            ]
        }
    ]
}
```

### Get all workers and expand all subs where the sub child is non-empty

using the build-in `any` function:

```C#
http://localhost:5269/odata/Workers?$expand=AbsenceHeaders($expand=AbsenceDailyDetails;$filter=AbsenceDailyDetails/Any())
```

```json
{
    "@odata.context": "http://localhost:5269/odata/$metadata#Workers(AbsenceHeaders(AbsenceDailyDetails()))",
    "value": [
        {
            "Id": 1,
            "Name": "Sam",
            "AbsenceHeaders": [
                {
                    "AbsenceHeaderId": 111,
                    "AbsenceDailyDetails": [
                        {
                            "AbsenceDailyDetailId": 1111,
                            "Date": "2024-10-01",
                            "AbsenceHeaderId": 111,
                            "IsAbsentDay": true
                        },
                        {
                            "AbsenceDailyDetailId": 1112,
                            "Date": "2024-10-02",
                            "AbsenceHeaderId": 111,
                            "IsAbsentDay": false
                        }
                    ]
                }
            ]
        },
        {
            "Id": 2,
            "Name": "Wu",
            "AbsenceHeaders": [
                {
                    "AbsenceHeaderId": 222,
                    "AbsenceDailyDetails": [
                        {
                            "AbsenceDailyDetailId": 111,
                            "Date": "2025-10-01",
                            "AbsenceHeaderId": 11,
                            "IsAbsentDay": true
                        },
                        {
                            "AbsenceDailyDetailId": 112,
                            "Date": "2026-10-02",
                            "AbsenceHeaderId": 11,
                            "IsAbsentDay": false
                        }
                    ]
                }
            ]
        }
    ]
}
```

### Get all workers and expand all subs where the sub child is empty

use the keyword `not` and `any` function together:

```
http://localhost:5269/odata/Workers?$expand=AbsenceHeaders($expand=AbsenceDailyDetails;$filter=not AbsenceDailyDetails/Any())
```

Here's the result:


```json
{
    "@odata.context": "http://localhost:5269/odata/$metadata#Workers(AbsenceHeaders(AbsenceDailyDetails()))",
    "value": [
        {
            "Id": 1,
            "Name": "Sam",
            "AbsenceHeaders": [
                {
                    "AbsenceHeaderId": 122,
                    "AbsenceDailyDetails": []
                }
            ]
        },
        {
            "Id": 2,
            "Name": "Wu",
            "AbsenceHeaders": [
                {
                    "AbsenceHeaderId": 222,
                    "AbsenceDailyDetails": []
                }
            ]
        }
    ]
}
```

Be noted: the `"AbsenceHeaderId": 133` and `"AbsenceHeaderId": 233` are not return because their value is 'null' at the backend.

