# 

This sample is used to build the generic endpoint for type cast.

basically, if you build the routing template as:

```c#
[HttpGet("aws/findings/{entityType}")]
```

The parameter '{entityType}' is treated as key segment. If you want to make it a type cast, you should change that.

You can find the way to enable it in this sample.

