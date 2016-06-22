using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using Xunit;

namespace Microsoft.AspNet.OData.Test.CompositeKey
{
    public class AnyController : ODataController
    {
        [HttpGet]
        [ODataRoute("DriverReleaseLifecycleDescriptions(DriverReleaseLifecycleStateId={stateId},DriverReleaseLifecycleSubstateId={subStateId})")]
        public IHttpActionResult Method1(int stateId, int subStateId)
        {
            string result = "(" + stateId + "," + subStateId + ")";
            return Ok(result);
        }

        [HttpPatch]
        [ODataRoute("DriverReleaseLifecycleDescriptions(DriverReleaseLifecycleStateId={stateId},DriverReleaseLifecycleSubstateId={subStateId})")]
        public IHttpActionResult Method2(int stateId, int subStateId, Delta<DriverReleaseLifecycleDescription> delta)
        {
            Assert.Equal(4, stateId);

            Assert.Equal(5, subStateId);

            DriverReleaseLifecycleDescription origin = new DriverReleaseLifecycleDescription();
            delta.Patch(origin);

            Assert.Equal("Waiting for launch approval", origin.Text);
            Assert.Equal("Gray", origin.ColorName);

            return Updated(origin);
        }
    }
}
