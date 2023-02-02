It's related to issue: 

https://github.com/OData/AspNetCoreOData/issues/737

**Be noted**: If you use [ApiController] on the controller, it could throw exception that ApiController need the attribute routing.
Since this extension uses the convention to add the endpoint, the endpoints are added after the api controller verification.
In order to making it work, you can create a ModelProvider (make a order less than the order for ApiBehavior...ModelProvider). In the OnProviderExecuting, build the selector for the generic controller action.

# Updated 2/2/2023:
There's a customer who needs key in paranthesis.

<img width="1884" alt="image" src="https://user-images.githubusercontent.com/9426627/216411283-326f9d94-64cd-4636-951a-301f1f331590.png">

Query entity using key in paranthesis:

<img width="705" alt="image" src="https://user-images.githubusercontent.com/9426627/216411663-2bed0c86-f192-40b2-a2f0-a121d14d38f9.png">


Query entity using key as segment:

<img width="685" alt="image" src="https://user-images.githubusercontent.com/9426627/216411514-863b5729-5051-489b-b46b-831eabdeb164.png">



# Updated 1/27/2023
for https://github.com/JasonEades/GenericControllerSample


<img width="1839" alt="image" src="https://user-images.githubusercontent.com/9426627/215216254-75082211-dfe2-4890-bef7-0dd553aa11b9.png">


## Query single

### Single Customer

<img width="719" alt="image" src="https://user-images.githubusercontent.com/9426627/215216389-d1cf8318-7eae-496b-95e9-d2010c5bfd23.png">


### Single Order

<img width="683" alt="image" src="https://user-images.githubusercontent.com/9426627/215216329-d95727a0-f49e-45de-9c93-dd45c1ca9257.png">

