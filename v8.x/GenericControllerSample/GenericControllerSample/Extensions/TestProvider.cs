﻿using GenericControllerSample.Controllers;
using GenericControllerSample.Models;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace GenericControllerSample.Extensions
{
    public class TestProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            TypeInfo controllerType = typeof(ODataController<>)
                 .MakeGenericType(typeof(Customer)).GetTypeInfo();
            feature.Controllers.Add(controllerType);

            controllerType = typeof(ODataController<>)
                 .MakeGenericType(typeof(Order)).GetTypeInfo();

            feature.Controllers.Add(controllerType);
            // This is required to identify the generic controller
            controllerType = typeof(CustomersController<Customer>).GetTypeInfo();
            feature.Controllers.Add(controllerType);

            // This is required to identify the generic controller
            controllerType = typeof(EngagementEntitySetController<,>).MakeGenericType(typeof(Customer), typeof(int)).GetTypeInfo();
            feature.Controllers.Add(controllerType);
        }
    }
}
