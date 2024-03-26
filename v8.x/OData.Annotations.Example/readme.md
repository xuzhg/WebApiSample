### This is related to this issue at: https://github.com/OData/AspNetCoreOData/issues/1198

The customer customized the serializer to include the annotations, it works fine to retrieve all as below.

---
![image](https://github.com/xuzhg/WebApiSample/assets/9426627/516c6480-2380-48ad-860f-072c16cef3ed)
---

But, it won't work if using `$select=ownerid`

The root problem is that OData only includes the selected properties in the final result if $select is used.

So, this sample illustrates a way to add the extra properties to the selected result. 

When running this sample using 'https://localhost:7243/odata/accounts?$select=ownerid'

**`Prefer=odata.include-annotations=*`**

You can get the following (similar) response:
---
![image](https://github.com/xuzhg/WebApiSample/assets/9426627/bf8ef5a6-5ed1-4fbc-a090-47092a76f2fa)

---

