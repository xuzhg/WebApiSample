using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter.Value;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Query.Wrapper;
using Microsoft.AspNetCore.OData.Routing.Controllers;

using ODataDyncRouteSample.Models;

namespace ODataDyncRouteSample.Controllers
{
    public class CommsEntityController<T> : ODataController
        where T : CommsEntity, new()
    {
        // public Task<ActionResult<ODataResponse<T>>> GetAsync(ODataQueryOptions<T> queryOptions)
        public async Task<IActionResult> GetAsync(ODataQueryOptions<T> queryOptions)
        {
            List<AzureUpdate> result = new List<AzureUpdate>();
            result.Add(new AzureUpdate
            {
                Id = "1",
                Title = "Azure Update 1",
                Description = "Azure Update 1 Description",
            });
            result[0].DynamicProperties.Add("Content", "a string value here");
            result[0].DynamicProperties.Add("otherdata", true);// a boolean value here

            result.Add(new AzureUpdate
            {
                Id = "2",
                Title = "Azure Update 2",
                Description = "Azure Update 2 Description",
            });
            IDictionary<string, object> dynamics = result[1].DynamicProperties;

            // Create the dynamic collection in which contains one object
            dynamics.Add("content1", new List<EdmUntypedObject>
            {
                new EdmUntypedObject
                {
                    {"Id", 1}, {"Name", "My Name 2" }
                }
            });

            // Create the dynamic "object"
            dynamics.Add("content2", new Dictionary<string, object>
            {
                {"Id", 1}, {"Name", "My Name 2" }
            });
            dynamics.Add("additionalmetadata", new EdmUntypedObject
            {
                {"someProperty1", "someValue 1"}, {"someProperty2", "someValue  2" }
            });

            IQueryable<AzureUpdate> resultsAsQueryable = result.AsQueryable();
            IQueryable filteredResults = queryOptions.ApplyTo(resultsAsQueryable);

            IActionResult okResult = Ok(filteredResults);
            return okResult;

#if false  // you don't need the follow
            // There is a known issue where converting selected data back to a list is not easily supported.
            // See the GitHub issue here: https://github.com/OData/AspNetCoreOData/issues/865
            // This is a workaround to handle that scenario until it is better supported officially.
            if (typeof(ISelectExpandWrapper).IsAssignableFrom(filteredResults.ElementType))
            {
                List<AzureUpdate> reprojectedResults = new List<AzureUpdate>();
                foreach (object filteredItem in filteredResults)
                {
                    ISelectExpandWrapper wrappedFilteredItem = filteredItem as ISelectExpandWrapper;
                    IDictionary<string, object> selectedPropertyValues = wrappedFilteredItem.ToDictionary();

                    AzureUpdate recreatedItem = new AzureUpdate();
                    foreach (PropertyInfo entityTypeProperty in typeof(T).GetProperties())
                    {
                        if (selectedPropertyValues.ContainsKey(entityTypeProperty.Name))
                        {
                            entityTypeProperty.SetValue(recreatedItem, selectedPropertyValues[entityTypeProperty.Name]);
                        }
                    }

                    reprojectedResults.Add(recreatedItem);
                }

                result = reprojectedResults;
            }
            else
            {
                result = filteredResults.Cast<AzureUpdate>().ToList();
            }

            ODataResponse<T> response = new ODataResponse<T>();
            response.Value = new ODataValue<T>();
            response.Value.Content = (ICollection<T>)result;

            ActionResult<ODataResponse<T>> actionResult = Ok(response);

            return Task.FromResult(actionResult);
#endif
        }
    }
}
