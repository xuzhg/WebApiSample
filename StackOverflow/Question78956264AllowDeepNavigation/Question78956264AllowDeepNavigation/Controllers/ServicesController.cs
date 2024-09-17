using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Question78956264AllowDeepNavigation.Extensions;
using System.Text;

namespace Question78956264AllowDeepNavigation.Controllers
{
    // Use the OData convention rules to build the endpoint
    public class ServicesController : ODataController
    {
        [HttpGet]
        [ODataRouteComponent("convention")] // this line is used to make sure only 'convention' component is allowed in convention routing 
        public IActionResult GetServiceArticle(int keyServiceId, int keyServiceArticleId)
        {
            return Ok($"You are calling the convention endpoint using ServiceId={keyServiceId}, ServiceArticleId={keyServiceArticleId}.");
        }

        [BrowserProperty]
        public IActionResult GetProperties(BrowserInfos propertyLists)
        {
            var segs = propertyLists.Segments.Select((s,idx) => $"{idx + 1} |- {s.ToString()}").ToList();
            return Ok(segs);
        }
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public sealed class ODataBrowserPropertyBindingAttribute : ModelBinderAttribute
    {
        public ODataBrowserPropertyBindingAttribute()
            : base(typeof(ODataBrowserPropertyBinding))
        {
        }

        internal class ODataBrowserPropertyBinding : IModelBinder
        {
            public Task BindModelAsync(ModelBindingContext bindingContext)
            {
                var infos = bindingContext.HttpContext.Request.RouteValues.FirstOrDefault(c => c.Value is BrowserInfos);

                if (infos.Value == null)
                {
                    return Task.CompletedTask;
                }


                bindingContext.Result = ModelBindingResult.Success(infos.Value);

                return Task.CompletedTask;
            }
        }
    }

    [ODataBrowserPropertyBinding]
    public class BrowserInfos
    {
        private IList<SegmentInfo> segmentInfos = new List<SegmentInfo>();

        public IList<SegmentInfo> Segments => segmentInfos;

        public void Add(SegmentInfo segmentInfo)
        {
            segmentInfos.Add(segmentInfo);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var segmentInfo in segmentInfos)
            {
                sb.Append(segmentInfo.ToString());
                sb.Append("/");
            }

            return sb.ToString();
        }
    }

    public abstract class SegmentInfo {}

    public class EntitySetSegmentInfo : SegmentInfo
    {
        public string EntitySet { get; set; }

        public override string ToString()
        {
            return $"EntitySet: {EntitySet}";
        }
    }
    public class IntKeySegmentInfo : SegmentInfo
    {
        public string KeyName { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return $"Key: {KeyName} = {Value}";
        }
    }

    public class PropertySegmentInfo : SegmentInfo
    {
        public string PropertyName { get; set; }

        public override string ToString()
        {
            return $"Property: {PropertyName}";
        }
    }

    public class BoundOperationSegmentInfo : SegmentInfo
    {
        public string OperationName { get; set; }

        // Without take parameters into consideration for simplicity
        public override string ToString()
        {
            return $"BoundOperation: {OperationName}()";
        }
    }

    public class PeopleController : Controller
    {
        public IActionResult List(string ssn)
        {
            return Ok(ssn);
        }

        public IActionResult Get(string odataPath)
        {
            return Ok(odataPath);
        }
    }
}
