using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.OData.Abstracts;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.Routing.Matching;

namespace DynamicRouteSample.Extensions;

internal class ODataEndpointSelectorPolicy : MatcherPolicy, IEndpointSelectorPolicy
{
    // Run shortly after the DynamicControllerMatcherPolicy
    public override int Order => int.MinValue + 200;

    public bool AppliesToEndpoints(IReadOnlyList<Endpoint> endpoints)
    {
        // If OData is in use, then run this policy everywhere.
        return true;
    }

    public Task ApplyAsync(HttpContext httpContext, CandidateSet candidates)
    {
        IODataFeature odataFeature = httpContext.ODataFeature();
        if (!odataFeature.RoutingConventionsStore.ContainsKey("odata_action"))
        {
            // This means the request didn't match an OData endpoint. Just ignore it.
            return Task.CompletedTask;
        }

        ActionDescriptor descriptor = (ActionDescriptor)odataFeature.RoutingConventionsStore["odata_action"];

        for (int i = 0; i < candidates.Count; i++)
        {
            if (candidates[i].Endpoint == null)
            {
                candidates.SetValidity(i, false);
                continue;
            }

            ActionDescriptor action = candidates[i].Endpoint.Metadata.GetMetadata<ActionDescriptor>();

            if (action != null && !object.ReferenceEquals(action, descriptor))
            {
                // This candidate is not the one we matched earlier, so disallow it.
                candidates.SetValidity(i, false);
            }
        }

        return Task.CompletedTask;
    }
}
