using Microsoft.AspNetCore.OData.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PermissionsAnalytics.Models
{
    public interface IMappingService
    {
        Task<IList<Finding>> GetFindings(AuthorizationType cloudType, string itemType);
    }

    public class DefaultMappingService : IMappingService
    {
        private static IdentityGovernance _identityGovernance = new IdentityGovernance
        {
            PermissionsAnalytics = new PermissionsAnalyticsAggregation
            {
                Aws = new PermissionsAnalytics
                {
                    Findings = new List<Finding>
                    {
                        new IdentitySearchProperties
                        {
                            Id = 1,
                            PermissionsCreepIndex = new PermissionsCreepIndex
                            {
                                Score = 1
                            }
                        },
                        new IdentityFinding
                        {
                            Id = 2,
                            ActionSummary = new ActionSummary
                            {
                                Assigned = 12, Available = 13, Exercised = 14
                            },
                            PermissionsCreepIndex = new PermissionsCreepIndex
                            {
                                Score = 11
                            }
                        }
                    }
                },
                Azure = new PermissionsAnalytics
                {
                    Findings = new List<Finding>
                    {
                        new IdentitySearchProperties
                        {
                            Id = 3,
                            PermissionsCreepIndex = new PermissionsCreepIndex
                            {
                                Score = 2
                            }
                        },
                        new InactiveAwsResourceFinding
                        {
                            Id = 4,
                            ActionSummary = new ActionSummary
                            {
                                Assigned = 21, Available = 22, Exercised = 23
                            },
                            PermissionsCreepIndex = new PermissionsCreepIndex
                            {
                                Score = 22
                            }
                        },
                        new InactiveAzureServicePrincipalFinding
                        {
                            Id =5,
                            ActionSummary = new ActionSummary
                            {
                                Assigned = 27, Available = 28, Exercised = 29
                            },
                            PermissionsCreepIndex = new PermissionsCreepIndex
                            {
                                Score = 25
                            }
                        }
                    }
                },
                Gcp = new PermissionsAnalytics
                {
                    Findings = new List<Finding>
                    {
                        new InactiveGcpServiceAccountFinding
                        {
                            Id = 6,
                            ActionSummary = new ActionSummary
                            {
                                Assigned = 31, Available = 32, Exercised = 33
                            },
                            PermissionsCreepIndex = new PermissionsCreepIndex
                            {
                                Score = 3
                            }
                        },
                        new AwsExternalSystemAccessFinding
                        {
                            Id = 7,
                            PermissionsCreepIndex = new PermissionsCreepIndex
                            {
                                Score = 4
                            }
                        },
                        new AwsIdentityAccessManagementKeyAgeFinding
                        {
                            Id = 8,
                            PermissionsCreepIndex = new PermissionsCreepIndex
                            {
                                Score = 5
                            }
                        }
                    }
                }
            }
        };

        public async Task<IList<Finding>> GetFindings(AuthorizationType cloudType, string itemType)
        {
            IList<Finding> findings = null;
            switch (cloudType)
            {
                case AuthorizationType.Aws:
                    findings = _identityGovernance.PermissionsAnalytics.Aws.Findings;
                    break;

                case AuthorizationType.Azure:
                    findings = _identityGovernance.PermissionsAnalytics.Azure.Findings;
                    break;

                case AuthorizationType.GCP:
                    findings = _identityGovernance.PermissionsAnalytics.Gcp.Findings;
                    break;

                default:
                    throw new NotImplementedException($"Not implemented for type {cloudType}");
            }

            if (findings == null)
            {
                return null;
            }

            IList<Finding> newFindings = findings
                .Where(f => string.Equals(f.GetType().FullName, itemType, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            return await Task.FromResult(newFindings);

#if false
            switch (itemType)
            {
                case "PermissionsAnalytics.Models.IdentityFinding":
                    return await Task.FromResult(findings.OfType<IdentityFinding>().ToList());

                case "PermissionsAnalytics.Models.IdentitySearchProperties":
                    return await Task.FromResult(findings.OfType<IdentitySearchProperties>().ToList());

                case "PermissionsAnalytics.Models.InactiveAwsResourceFinding":
                    return await Task.FromResult(findings.OfType<InactiveAwsResourceFinding>().ToList());

                case "PermissionsAnalytics.Models.InactiveAwsRoleFinding":
                    await Task.FromResult(findings.OfType<InactiveAwsRoleFinding>().ToList());
                    break;

                case "PermissionsAnalytics.Models.InactiveAzureServicePrincipalFinding":
                    await Task.FromResult(findings.OfType<InactiveAzureServicePrincipalFinding>().ToList());
                    break;

                case "PermissionsAnalytics.Models.InactiveGcpServiceAccountFinding":
                    await Task.FromResult(findings.OfType<InactiveGcpServiceAccountFinding>().ToList());
                    break;

                case "PermissionsAnalytics.Models.InactiveServerlessFunctionFinding":
                    await Task.FromResult(findings.OfType<InactiveServerlessFunctionFinding>().ToList());
                    break;

                case "PermissionsAnalytics.Models.AwsExternalSystemAccessFinding":
                    await Task.FromResult(findings.OfType<AwsExternalSystemAccessFinding>().ToList());
                    break;

                case "PermissionsAnalytics.Models.AwsExternalSystemAccessRoleFinding":
                    await Task.FromResult(findings.OfType<AwsExternalSystemAccessRoleFinding>().ToList());
                    break;

                case "PermissionsAnalytics.Models.AwsIdentityAccessManagementKeyAgeFinding":
                    await Task.FromResult(findings.OfType<AwsIdentityAccessManagementKeyAgeFinding>().ToList());
                    break;

                case "PermissionsAnalytics.Models.AwsIdentityAccessManagementKeyUsageFinding":
                    await Task.FromResult(findings.OfType<AwsIdentityAccessManagementKeyUsageFinding>().ToList());
                    break;
            }

            throw new InvalidCastException("....");
#endif
        }
    }
}
