using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using System.Collections.Generic;

namespace CustomizeSelectExpand
{
    public class WeatherForecast
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        public IList<Department> Departs { get; set; }
    }

    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
