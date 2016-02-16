using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

namespace NestedFilterSample
{
    public class RegionsController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(Regions);
        }

        private static IList<Region> Regions;

        static RegionsController()
        {
            Regions = new List<Region>();

            Region r = new Region
            {
                Id = 1,
                Name = "Region1",
                Facilities = new List<Facility>
                {
                    new Facility
                    {
                        Id = 11,
                        Active = true,
                        Departments = new List<Department>
                        {
                            new Department {Id = 111, Active = true},
                            new Department {Id = 112, Active = false}
                        }
                    },
                    new Facility
                    {
                        Id = 12,
                        Active = false,
                        Departments = new List<Department>
                        {
                            new Department {Id = 121, Active = true},
                            new Department {Id = 122, Active = false},
                        }
                    }
                }
            };
            Regions.Add(r);

            r = new Region
            {
                Id = 2,
                Name = "Region2",
                Facilities = new List<Facility>
                {
                    new Facility
                    {
                        Id = 21,
                        Active = true,
                        Departments = new List<Department>
                        {
                            new Department {Id = 211, Active = true},
                            new Department {Id = 212, Active = false}
                        }
                    },
                    new Facility
                    {
                        Id = 22,
                        Active = false,
                        Departments = new List<Department>
                        {
                            new Department {Id = 221, Active = true},
                            new Department {Id = 222, Active = false},
                        }
                    }
                }
            };
            Regions.Add(r);
        }
    }
}
