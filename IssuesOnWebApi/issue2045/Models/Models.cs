using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace issue2045.Models
{
    public class InspectionDuty
    {
        public int Id { get; set; }
    }

    public class InspectionDutyChange
    {
        public Opeartions Operations { get; set; }

        public Guid Identity { get; set; }

        public bool IsDisabled {get;set;}

        public DateTimeOffset SeriesStart { get; set; }

        public Interval Interval { get; set; }
    }

    public enum Opeartions
    {
        Undefined,
        Create,
        Update
    }

    public class Interval
    {
        public int Step { get; set; }

        public IntervalUnits Size { get; set; }
    }

    public enum IntervalUnits
    {
        Undefined,
        Year,
        Month,
        Week,
        Day
    }
}
