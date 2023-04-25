
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Abstracts;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PermissionsAnalytics.Extensions;
using PermissionsAnalytics.Models;

namespace PermissionsAnalytics.Controllers
{
    [Route($"odata/identityGovernance/permissionsAnalytics")]
    public class PermissionsAnalyticsController : ODataController
    {
        private IMappingService _mapping;

        public PermissionsAnalyticsController(IMappingService mappingService)
        {
            _mapping = mappingService;
        }

        // TODO: {cloud}/findings/identities/ throws not found


        [HttpGet("aws/findings/{entityType}")]
        [GenericEntityTypeActionConvention]
        [EnableQuery]
        public Task<IActionResult> GetAwsIdentites(string entityType, ODataQueryOptions<IdentitySearchProperties> queryOptions)
        {
            return GetIdentites(AuthorizationType.Aws, entityType, queryOptions);
        }

        [HttpGet("azure/findings/{entityType}")]
        [GenericEntityTypeActionConvention]
        [EnableQuery]
        public Task<IActionResult> GetAzureIdentites(string entityType, ODataQueryOptions<IdentitySearchProperties> queryOptions)
        {
            return GetIdentites(AuthorizationType.Azure, entityType, queryOptions);
        }

        [HttpGet("gcp/findings/{entityType}")]
        [GenericEntityTypeActionConvention]
        [EnableQuery]
        public Task<IActionResult> GetGcpIdentites(string entityType, ODataQueryOptions<IdentitySearchProperties> queryOptions)
        {
            return GetIdentites(AuthorizationType.GCP, entityType, queryOptions);
        }

        private async Task<IActionResult> GetIdentites(AuthorizationType cloudType, string entityType, ODataQueryOptions<IdentitySearchProperties> queryOptions)
        {
            IODataFeature oDataFeature = Request.ODataFeature();

            //return Ok($"We have cloudType={cloudType} with ET={entityType}");
            //try
            //{
            //    return Ok(await mapping.GetPagedList(queryOptions, cloudType, entityType));
            //}
            //catch (MissingMappingException ex)
            //{
            //    return NotFound(ex.NotFoundErrorMessage);
            //}
            var findings = await _mapping.GetFindings(cloudType, entityType);

            switch (entityType)
            {
                case "PermissionsAnalytics.Models.IdentityFinding":
                    return Ok(findings.Cast<IdentityFinding>());

                case "PermissionsAnalytics.Models.IdentitySearchProperties":
                    return Ok(findings.Cast<IdentitySearchProperties>());

                case "PermissionsAnalytics.Models.InactiveAwsResourceFinding":
                    return Ok(findings.Cast<InactiveAwsResourceFinding>());

                case "PermissionsAnalytics.Models.InactiveAwsRoleFinding":
                    return Ok(findings.Cast<InactiveAwsRoleFinding>());

                case "PermissionsAnalytics.Models.InactiveAzureServicePrincipalFinding":
                    return Ok(findings.Cast<InactiveAzureServicePrincipalFinding>());

                case "PermissionsAnalytics.Models.InactiveGcpServiceAccountFinding":
                    return Ok(findings.Cast<InactiveGcpServiceAccountFinding>());

                case "PermissionsAnalytics.Models.InactiveServerlessFunctionFinding":
                    return Ok(findings.Cast<InactiveServerlessFunctionFinding>());

                case "PermissionsAnalytics.Models.AwsExternalSystemAccessFinding":
                    return Ok(findings.Cast<AwsExternalSystemAccessFinding>());

                case "PermissionsAnalytics.Models.AwsExternalSystemAccessRoleFinding":
                    return Ok(findings.Cast<AwsExternalSystemAccessRoleFinding>());

                case "PermissionsAnalytics.Models.AwsIdentityAccessManagementKeyAgeFinding":
                    return Ok(findings.Cast<AwsIdentityAccessManagementKeyAgeFinding>());

                case "PermissionsAnalytics.Models.AwsIdentityAccessManagementKeyUsageFinding":
                    return Ok(findings.Cast<AwsIdentityAccessManagementKeyUsageFinding>());
            }

            return Ok(findings);
        }

        // NOTE: We need to look into how to handle this with OData
        [HttpGet]
        [Route("{cloud}/findings/{itemType}/aggregatedSummary(authorizationSystemIds={authorizationSystemIds})")]
        public IActionResult GetAggregatedSummary(string cloud, string itemType, string authorizationSystemIds)
        {
            AuthorizationType cloudType = ConvertCloudType(cloud);

            //try
            //{
            //    return Ok(mapping.GetAggregateSummary(cloudType, itemType, authorizationSystemIds));
            //}
            //catch (MissingMappingException ex)
            //{
            //    return NotFound(ex.NotFoundErrorMessage);
            //}
            return Ok();
        }

        private static AuthorizationType ConvertCloudType(string cloud)
        {
            return cloud.ToLower() switch
            {
                "aws" => AuthorizationType.Aws,
                "azure" => AuthorizationType.Azure,
                "gcp" => AuthorizationType.GCP,
                _ => throw new System.Exception($"Cloud type {cloud} not found")
            };
        }
    }
}
