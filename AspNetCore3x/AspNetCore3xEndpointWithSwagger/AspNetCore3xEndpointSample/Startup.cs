// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore3xEndpointSample.Models;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OData.Edm;
using Microsoft.OpenApi.Models;

namespace AspNetCore3xEndpointSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CustomerOrderContext>(opt => opt.UseLazyLoadingProxies().UseInMemoryDatabase("CustomerOrderList"));
            services.AddOData();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "1" });

                var xmlCommentsPath = Path.Combine(System.AppContext.BaseDirectory, "GenericControllers.xml");

                c.IncludeXmlComments(xmlCommentsPath);

                //c.OperationFilter<ApplySummariesOperationFilter>();
            });

            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }

                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //      ApiDescriptionGroupCollectionProvider provider = new ApiDescriptionGroupCollectionProvider(null, null);
            //   DefaultApiDescriptionProvider provider = new DefaultApiDescriptionProvider(null, null, null, null, null);

            // ActionModel action = new ActionModel(null);
         //   ControllerModel m = new ControllerModel(null);
            ApiExplorerModel apiModel = new ApiExplorerModel();
            apiModel.IsVisible = true;
            ApiExplorerModel another = new ApiExplorerModel(apiModel);

            IEdmModel model = EdmModelBuilder.GetEdmModel();

            app.UseRouting();

            app.UseAuthorization();

            app.UseODataBatching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Select().Expand().Filter().OrderBy().MaxTop(100).Count();

                endpoints.MapODataRoute("nullPrefix", null, model);

                endpoints.MapODataRoute("odataPrefix", "odata", model);

                endpoints.MapODataRoute("myPrefix", "my/{data}", model);

                endpoints.MapODataRoute("msPrefix", "ms", model, new DefaultODataBatchHandler());
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Test API (V1)");
            });
        }
    }
}
