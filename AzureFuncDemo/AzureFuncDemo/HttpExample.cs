using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AzureFuncDemo
{
    public static class HttpExample
    {
        private static IList<CnCustomer> cnCustomers = new List<CnCustomer>
            {
                new CnCustomer { Id = 1, Name = "Sam", Postcode = "201501" },
                new CnCustomer { Id = 2, Name = "Liu", Postcode = "111578" },
                new CnCustomer { Id = 3, Name = "Wu", Postcode = "198201" },
                new CnCustomer { Id = 4, Name = "Kerry", Postcode = "200998" }
            };

        private static IList<UsCustomer> usCustomers = new List<UsCustomer>
            {
                new UsCustomer { Id = 1, Name = "Peter", ZipCode = 98019 },
                new UsCustomer { Id = 2, Name = "Kate", ZipCode = 98020 },
                new UsCustomer { Id = 3, Name = "John", ZipCode = 98030 },
                new UsCustomer { Id = 4, Name = "Cathy", ZipCode = 98040 }
            };

        [FunctionName("HttpExample")]
        [EnableQuery]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("customers")]
        [EnableQuery]
        public static async Task<IActionResult> RunCustomers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "v1/{source}/customers")] HttpRequest req, string source,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            IEdmModel model = EdmModelBuilder.GetEdmModel(source);
            IODataFeature feature = req.ODataFeature();

            ODataUriUtils.InitializeOData(req, model, source);

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            if (source == "cn")
            {
                return GetCnCustomers(model, req);
            }
            else
            {
                return GetUsCustomers(model, req);
            }
        }

        private static OkObjectResult GetCnCustomers(IEdmModel model, HttpRequest request)
        {

            var features = request.ODataFeature();

            ODataQueryContext context = new ODataQueryContext(model, typeof(CnCustomer), features.Path);
            ODataQueryOptions<CnCustomer> querys = new ODataQueryOptions<CnCustomer>(context, request);
            var data = querys.ApplyTo(cnCustomers.AsQueryable(), new ODataQuerySettings());

            return new OkObjectResult(data);
        }

        private static OkObjectResult GetUsCustomers(IEdmModel model, HttpRequest request)
        {

            var features = request.ODataFeature();

            ODataQueryContext context = new ODataQueryContext(model, typeof(UsCustomer), features.Path);
            ODataQueryOptions<UsCustomer> querys = new ODataQueryOptions<UsCustomer>(context, request);
            var data = querys.ApplyTo(usCustomers.AsQueryable(), new ODataQuerySettings());

            return new OkObjectResult(data);
        }
    }

    public static class HttpExample2
    {
        [FunctionName("HttpExample___2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
