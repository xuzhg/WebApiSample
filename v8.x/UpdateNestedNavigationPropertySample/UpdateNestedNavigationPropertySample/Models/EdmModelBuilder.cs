﻿using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace UpdateNestedNavigationPropertySample.Models
{
    public class EdmModelBuilder
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<EducationClass>("Classes");
            builder.EntitySet<EducationGradingCategory>("GradingCategories");

            return builder.GetEdmModel();
        }
    }
}
