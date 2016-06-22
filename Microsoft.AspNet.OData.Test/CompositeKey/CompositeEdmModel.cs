using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.OData.Builder;
using Microsoft.OData.Edm;

namespace Microsoft.AspNet.OData.Test.CompositeKey
{
    public static class CompositeEdmModel
    {
        private static IEdmModel _edmModel;

        public static IEdmModel GetEdmModel()
        {
            if (_edmModel == null)
            {
                var builder = new ODataConventionModelBuilder();

                builder.EntitySet<DriverReleaseLifecycleDescription>("DriverReleaseLifecycleDescriptions");
                _edmModel = builder.GetEdmModel();
            }

            return _edmModel;
        }
    }

    public class DriverReleaseLifecycleDescription
    {
        public string Text { get; set; }
        public string ColorName { get; set; }
        [Key]
        public int DriverReleaseLifecycleStateId { get; set; }
        [Key]
        public int DriverReleaseLifecycleSubstateId { get; set; }
        public DateTimeOffset InsertedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }

        [ConcurrencyCheck]
        public byte[] Version { get; set; }
    }
}
