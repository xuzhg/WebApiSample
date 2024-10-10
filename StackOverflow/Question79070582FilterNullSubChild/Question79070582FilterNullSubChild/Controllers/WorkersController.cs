using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Question79070582FilterNullSubChild.Controllers
{
    public class WorkersController : ControllerBase
    {
        private static IList<Worker> _workers = new List<Worker>
        {
            new Worker
            {
                Id =1,
                Name = "Sam",
                AbsenceHeaders = new List<AbsenceHeader>
                {
                    new AbsenceHeader
                    {
                        AbsenceHeaderId = 111,
                        AbsenceDailyDetails = new List<AbsenceDailyDetail>
                        {
                            new AbsenceDailyDetail { AbsenceHeaderId = 111, AbsenceDailyDetailId = 1111, Date = new DateOnly(2024, 10, 1), IsAbsentDay = true },
                            new AbsenceDailyDetail { AbsenceHeaderId = 111, AbsenceDailyDetailId = 1112, Date = new DateOnly(2024, 10, 2), IsAbsentDay = false }
                        }
                    },
                    new AbsenceHeader
                    {
                        AbsenceHeaderId = 122,
                        AbsenceDailyDetails = new List<AbsenceDailyDetail>()
                    },
                    new AbsenceHeader
                    {
                        AbsenceHeaderId = 133,
                        AbsenceDailyDetails = null
                    }
                }
            },
            new Worker
            {
                Id  = 2,
                Name = "Wu",
                AbsenceHeaders = new List<AbsenceHeader>
                {
                    new AbsenceHeader
                    {
                        AbsenceHeaderId = 222,
                        AbsenceDailyDetails = new List<AbsenceDailyDetail>
                        {
                            new AbsenceDailyDetail { AbsenceHeaderId = 11, AbsenceDailyDetailId = 111, Date = new DateOnly(2025, 10, 1), IsAbsentDay = true },
                            new AbsenceDailyDetail { AbsenceHeaderId = 11, AbsenceDailyDetailId = 112, Date = new DateOnly(2026, 10, 2), IsAbsentDay = false }
                        }
                    },
                    new AbsenceHeader
                    {
                        AbsenceHeaderId = 222,
                        AbsenceDailyDetails = new List<AbsenceDailyDetail>()
                    },
                    new AbsenceHeader
                    {
                        AbsenceHeaderId = 233,
                        AbsenceDailyDetails = null
                    }
                }
            }
        };

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_workers);
        }
    }
}
