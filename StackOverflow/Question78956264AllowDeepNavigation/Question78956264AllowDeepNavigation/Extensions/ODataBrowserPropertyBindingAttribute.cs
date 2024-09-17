using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Question78956264AllowDeepNavigation.Extensions;

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
