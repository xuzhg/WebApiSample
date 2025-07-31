# it's related to this discussion: https://github.com/OData/AspNetCoreOData/discussions/1508

How to avoid the database duplicated fetching.

In this sample, here's the database structure.

<img width="761" height="585" alt="image" src="https://github.com/user-attachments/assets/b48c2844-e48e-4999-99f2-e664cf27d0f8" />

Here's the C# classes:

```C#
public class School
{
    public int SchoolId { get; set; }

    public string SchoolName { get; set; }

    // It's for Edm model, but calculated from 'Student' table
    // all last list is seperated by comma
    public string LastNames { get; set; }

    public Address MailAddress { get; set; }

    public SchoolDetail Details { get; set; }

    public IList<Student> Students { get; set; }
}

public class Student
{
   // omit ...
}
```
**Be noted**, the property `LastNames` within `class School` is not stored in the database.
The value is calcuated from table `Student`. As an example, `LastNames` is a string containing all Students' last names for that school, split using `,`.

Let's verify some scenarios

## 1 without any query options:

```cmd
GET {{CalculatedPropertyTest_HostAddress}}/odata/schools/
Accept: application/json
```

Here's the result:

```json
{
  "@odata.context": "http://localhost:5116/odata/$metadata#Schools",
  "value": [
    {
      "SchoolId": 1,
      "SchoolName": "Mercury Middle School",
      "LastNames": "Alex,Eaine,Rorigo,Rorigo,Clak",
      "MailAddress": {
        "AptNo": 241,
        "City": "Kirk",
        "Street": "156TH AVE",
        "ZipCode": "98051"
      }
    },
    {
      "SchoolId": 2,
      "SchoolName": "Venus High School",
      "LastNames": "Briana,Len,Jay,Oak",
      "MailAddress": {
  ...
```

Here's the database query:

<img width="2462" height="350" alt="image" src="https://github.com/user-attachments/assets/4b5e677a-905f-4ced-913a-1136cd705873" />

**be noted**, there are two 'left join' because in the `SchoolsController`, I have the following codes:
```C#
return Ok(_context.Schools.Select(c => new School
{
    SchoolId = c.SchoolId,
    SchoolName = c.SchoolName,
    LastNames = string.Join(",", c.Students.Select(s => s.LastName)), // this line constructs one let join
    MailAddress = c.MailAddress,
    Students = c.Students // This line constructs the other left join
}));
```

If you remove `Students = c.Students`, send the request again, you will see the updated SQL statement as:
<img width="2467" height="296" alt="image" src="https://github.com/user-attachments/assets/6fcd2fd7-386a-472b-854d-61861ff72c99" />

## 2 using #select

```cmd
GET {{CalculatedPropertyTest_HostAddress}}/odata/schools?$select=LastNames
Accept: application/json
```

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

<img width="1486" height="220" alt="image" src="https://github.com/user-attachments/assets/599e8100-f6a4-4bfb-8cbc-11298906318b" />

      SELECT "s"."SchoolId", "s0"."LastName", "s0"."StudentId"
      FROM "Schools" AS "s"
      LEFT JOIN "Students" AS "s0" ON "s"."SchoolId" = "s0"."SchoolId"
      ORDER BY "s"."SchoolId"

**be noted**, only the last name and student id are fetched.

## 3 using #$expand

```cmd
###
GET {{CalculatedPropertyTest_HostAddress}}/odata/schools?$expand=Students
Accept: application/json
```

In this case, the `LastNames` is `null`. In select all (no $select), OData just uses the "$it" to represent the instance. However, "$it" instance doesn't contains the "LastNames". 

Please leave your comments so that I can dig more for your reference.
