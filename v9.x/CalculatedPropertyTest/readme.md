# it's related to this discussion: https://github.com/OData/AspNetCoreOData/discussions/1508

How to avoid the database duplicated fetching.

In this sample, here's the database structure.


Be noted, the property `LastNames` within `class School` is not stored in the database.
The value is calcuated from table `Student`. As an example, `LastNames` is a string containing all last names for that school, split using `,`.



## 2 using #select

GET {{CalculatedPropertyTest_HostAddress}}/odata/schools?$select=LastNames
Accept: application/json


Here's the response:
```json
{
  "@odata.context": "http://localhost:5116/odata/$metadata#Schools(LastNames)",
  "value": [
    {
      "LastNames": "Alex,Eaine,Rorigo,Rorigo,Clak"
    },
    {
      "LastNames": "Briana,Len,Jay,Oak"
    },
    {
      "LastNames": "Wat,Joshi,Travade,Jay"
    },
    {
      "LastNames": "Wat,Joshi,Travade"
    },
    {
      "LastNames": "Padron,Brook,Johnson"
    },
    {
      "LastNames": "Haney,Frost,Viles"
    },
    {
      "LastNames": "Dally,Vax,Clarey"
    },
    {
      "LastNames": "Singh,Joe,Dalton"
    },
    {
      "LastNames": "Wu,Wottle,Aarav"
    }
  ]
}
```

Here's the database statement:

      SELECT "s"."SchoolId", "s0"."LastName", "s0"."StudentId"
      FROM "Schools" AS "s"
      LEFT JOIN "Students" AS "s0" ON "s"."SchoolId" = "s0"."SchoolId"
      ORDER BY "s"."SchoolId"
