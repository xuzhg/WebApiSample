using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionCallTest.Controllers
{
    public class EngagementController : ODataController
    {
        [HttpGet]
        public string GetEngagementAlertsMetrics(int key, int engagementId, Guid userId, int days)
        {
            return $"In Engagement({key}).GetEngagementAlertsMetrics({engagementId},{userId},{days})";
        }
    }
}
