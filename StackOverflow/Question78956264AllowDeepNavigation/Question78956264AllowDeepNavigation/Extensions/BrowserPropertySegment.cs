using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Routing.Template;
using Microsoft.Extensions.Options;
using Microsoft.OData.UriParser;

namespace Question78956264AllowDeepNavigation.Extensions;

public class BrowserPropertySegment : ODataSegmentTemplate
{
    public BrowserPropertySegment(string prefix)
    {
        Prefix = prefix;
    }

    public string Prefix { get; }

    public override IEnumerable<string> GetTemplates(ODataRouteOptions options)
    {
        yield return "{**odataPath}";
    }

    public override bool TryTranslate(ODataTemplateTranslateContext context)
    {
        if (!context.RouteValues.TryGetValue("odataPath", out object oDataPathValue))
        {
            return false;
        }
        string odataPath = oDataPathValue as string;

        HttpContext httpContext = context.HttpContext;

        ODataOptions odataOptions = httpContext.RequestServices.GetService<IOptions<ODataOptions>>().Value;

        IServiceProvider sp;
        if (!odataOptions.RouteComponents.ContainsKey(Prefix))
        {
            odataOptions.AddRouteComponents(Prefix, context.Model);
        }

        sp = odataOptions.RouteComponents[Prefix].ServiceProvider;

        ODataUriParser parser = new ODataUriParser(context.Model, new Uri(odataPath, UriKind.Relative), sp);

        try
        {
            ODataPath path = parser.ParsePath();

            RetrieveSegments(path, context.UpdatedValues);

            foreach (var s in path)
            {
                context.Segments.Add(s);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }


    private void RetrieveSegments(ODataPath path, RouteValueDictionary values)
    {
        BrowserInfos propertyLists = new BrowserInfos();

        foreach (var segment in path) // skip the first one because it's processed
        {
            if (segment is EntitySetSegment entitySet)
            {
                EntitySetSegmentInfo seg = new EntitySetSegmentInfo
                {
                    EntitySet = entitySet.EntitySet.Name
                };
                propertyLists.Add(seg);
            }
            else if (segment is KeySegment key)
            {
                var keyValue = key.Keys.First(); // for simplicity, only take care of one key scenario
                IntKeySegmentInfo keySeg = new IntKeySegmentInfo
                {
                    KeyName = keyValue.Key,
                    Value = (int)keyValue.Value
                };
                propertyLists.Add(keySeg);
            }
            else if (segment is PropertySegment prop)
            {
                PropertySegmentInfo seg = new PropertySegmentInfo
                {
                    PropertyName = prop.Property.Name
                };

                propertyLists.Add(seg);
            }
            else if (segment is NavigationPropertySegment navProp)
            {
                PropertySegmentInfo seg = new PropertySegmentInfo
                {
                    PropertyName = navProp.NavigationProperty.Name
                };

                propertyLists.Add(seg);
            }
            else if (segment is OperationSegment operationSegment)
            {
                BoundOperationSegmentInfo seg = new BoundOperationSegmentInfo
                {
                    OperationName = operationSegment.Operations.First().Name
                };

                propertyLists.Add(seg);
            }
            else
            {
                throw new Exception($"Not support segment {segment}");
            }
        }

        values["propertyLists"] = propertyLists;
    }
}
