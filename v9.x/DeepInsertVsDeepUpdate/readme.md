# Run and use 'DeepInsertVsDeepUpdate.http' to send request:

## Step 1: Check the initial status:

`GET {{DeepInsertVsDeepUpdate_HostAddress}}/odata/AgentInstances?$expand=manifest,manifest2`

Get the following:
```json
{
  "@odata.context": "http://localhost:5064/odata/$metadata#AgentInstances(Manifest(),Manifest2())",
  "value": [
    {
      "Id": 1,
      "Name": "Peter",
      "Manifest": {
        "Id": 1,
        "DisplayName": "Manifest One"
      },
      "Manifest2": {
        "Id": 21,
        "DisplayName": "Manifest2 One"
      }
    },
    {
      "Id": 11,
      "Name": "Sam",
      "Manifest": {
        "Id": 2,
        "DisplayName": "Manifest Sam"
      },
      "Manifest2": {
        "Id": 22,
        "DisplayName": "Manifest2 Sam"
      }
    }
  ]
}
```

## Step 2: Check the initial status of manifests:

`GET {{DeepInsertVsDeepUpdate_HostAddress}}/odata/manifests`

It gets:
```json
{
  "@odata.context": "http://localhost:5064/odata/$metadata#Manifests",
  "value": [
    {
      "Id": 1,
      "DisplayName": "Manifest One"
    },
    {
      "Id": 21,
      "DisplayName": "Manifest2 One"
    },
    {
      "Id": 2,
      "DisplayName": "Manifest Sam"
    },
    {
      "Id": 22,
      "DisplayName": "Manifest2 Sam"
    }
  ]
}
```


## step3: create a new instance
```cmd
POST {{DeepInsertVsDeepUpdate_HostAddress}}/odata/AgentInstances
Content-Type: application/json

{
  "Name": "New Agent Instance",
  "Manifest": {
    "Id": 3,
    "DisplayName": "New Manifest"
  },
  "Manifest2": {
    "Id": 31,
    "DisplayName": "New Manifest2"
  }
}
```
Then, do step 1, you can see a new agent instance created.

```json
{
      "Id": 12,
      "Name": "New Agent Instance",
      "Manifest": {
        "Id": 3,
        "DisplayName": "New Manifest"
      },
      "Manifest2": {
        "Id": 31,
        "DisplayName": "New Manifest2"
      }
    }
```

## Step 4: Create a new agent, mix the deep create and deep update

```cmd
POST {{DeepInsertVsDeepUpdate_HostAddress}}/odata/AgentInstances
Content-Type: application/json

{
  "Name": "New Agent Instance to link the existed manifest, meanwhile create a Manifeat2 ",
  "Manifest": {
    "@odata.id": "Manifests(2)"
  },
  "Manifest2": {
     "Id": 41,
     "DisplayName": "New 41 Manifest2"
  }
}
```
Then, do step 1, you can see a new agent instance created.
```json
    {
      "Id": 13,
      "Name": "New Agent Instance to link the existed manifest, meanwhile create a Manifeat2 ",
      "Manifest": {
        "Id": 2,
        "DisplayName": "Manifest Sam"
      },
      "Manifest2": {
        "Id": 41,
        "DisplayName": "New 41 Manifest2"
      }
```

## Step 5: failing test

```cmd
POST {{DeepInsertVsDeepUpdate_HostAddress}}/odata/AgentInstances
Content-Type: application/json

{
  "Name": "New Agent Instance to link the existed manifest",
  "Manifest": {
    "@odata.id": "Manifests(51)"
  },
  "Manifest2": {
     "Id": 61,
     "DisplayName": "New 61 Manifest2"
  }
}
```
Then, get an error message:
```json
{
  "error": {
    "code": "404",
    "message": "An AgentCardManifest with ODataId 'Manifests(51)' for navigation property 'Manifest' not existed."
  }
}
```

## Step 6: test using OData.bind

```cmd
###
POST {{DeepInsertVsDeepUpdate_HostAddress}}/odata/AgentInstances
Content-Type: application/json

{
  "Name": "New Agent Instance to using OData.bind",
  "Manifest@odata.bind": "Manifests(2)",
  "Manifest2@odata.bind": "Manifests(1)"
}
```
Then, do step 1, you can see a new agent instance created.
```json
    {
      "Id": 14,
      "Name": "New Agent Instance to using OData.bind",
      "Manifest": {
        "Id": 2,
        "DisplayName": "Manifest Sam"
      },
      "Manifest2": {
        "Id": 1,
        "DisplayName": "Manifest One"
      }
```

## Step 7: finial check the manifests

`GET {{DeepInsertVsDeepUpdate_HostAddress}}/odata/manifests`

You can see the final manifests created:

```json
{
  "@odata.context": "http://localhost:5064/odata/$metadata#Manifests",
  "value": [
    {
      "Id": 1,
      "DisplayName": "Manifest One"
    },
    {
      "Id": 21,
      "DisplayName": "Manifest2 One"
    },
    {
      "Id": 2,
      "DisplayName": "Manifest Sam"
    },
    {
      "Id": 22,
      "DisplayName": "Manifest2 Sam"
    },
    {
      "Id": 3,
      "DisplayName": "New Manifest"
    },
    {
      "Id": 31,
      "DisplayName": "New Manifest2"
    },
    {
      "Id": 41,
      "DisplayName": "New 41 Manifest2"
    }
  ]
}
```


