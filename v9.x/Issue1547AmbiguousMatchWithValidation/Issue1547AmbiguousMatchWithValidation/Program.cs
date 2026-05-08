using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.Extensions.Validation;

var builder = WebApplication.CreateBuilder();

builder.Services.AddOData();

builder.Services.AddValidation();

var app = builder.Build();

app.MapGet("/", ([SkipValidation] ODataQueryOptions<TimeZoneInfo> options) =>
{
    var data = TimeZoneInfo.GetSystemTimeZones().AsQueryable();

    var result = new PageResult<TimeZoneInfo>(options.ApplyTo(data).Cast<TimeZoneInfo>(), null,
        options.Count?.GetEntityCount(options.Filter?.ApplyTo(data, new ODataQuerySettings()) ?? data));

    return result;
})
    //.DisableValidation();
    ;

app.Run();