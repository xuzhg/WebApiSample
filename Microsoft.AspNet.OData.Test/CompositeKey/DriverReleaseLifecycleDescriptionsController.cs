using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using Xunit;

namespace Microsoft.AspNet.OData.Test.CompositeKey
{
    public class DriverReleaseLifecycleDescriptionsController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get(int keyDriverReleaseLifecycleStateId, int keyDriverReleaseLifecycleSubstateId)
        {
            string result = "(" + keyDriverReleaseLifecycleStateId + "," + keyDriverReleaseLifecycleSubstateId + ")";
            return Ok(result);
        }

        [EnableQuery]
        public IHttpActionResult Patch(int keyDriverReleaseLifecycleStateId, int keyDriverReleaseLifecycleSubstateId,
            Delta<DriverReleaseLifecycleDescription> delta)
        {
            Assert.Equal(4, keyDriverReleaseLifecycleStateId);

            Assert.Equal(5, keyDriverReleaseLifecycleSubstateId);

            DriverReleaseLifecycleDescription origin = new DriverReleaseLifecycleDescription();
            delta.Patch(origin);

            Assert.Equal("Waiting for launch approval", origin.Text);
            Assert.Equal("Gray", origin.ColorName);

            return Updated(origin);
        }
    }
}
