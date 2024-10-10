using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Question79070582FilterNullSubChild;

public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}

public static class EdmModelBuilder
{
    public static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();

        builder.EntitySet<Worker>("Workers");

        return builder.GetEdmModel();
    }
}


public class Worker
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public IList<AbsenceHeader>? AbsenceHeaders { get; set; }
}


public class AbsenceHeader
{
    public int AbsenceHeaderId { get; set; }

    public IList<AbsenceDailyDetail>? AbsenceDailyDetails { get; set; }
}


public class AbsenceDailyDetail
{
    public int AbsenceDailyDetailId { get; set; }

    public DateOnly Date { get; set; }

    public int AbsenceHeaderId { get; set; }

    public bool IsAbsentDay { get; set; }
}
