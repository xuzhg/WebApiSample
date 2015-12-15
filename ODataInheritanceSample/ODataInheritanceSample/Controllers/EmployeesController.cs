using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using ModelsNS;
using Xunit;

namespace ODataInheritanceSample.Controllers
{
    public class EmployeesController : ODataController
    {
        private static List<Employee> _employees = new List<Employee>();

        static EmployeesController()
        {
            Employee employee = new Employee
            {
                Id = 1,
                Name = "KaKa",
                Address = new Address
                {
                    City = "Tokyo",
                    Street = "Tokyo way",
                }
            };
            _employees.Add(employee);

            employee = new Manager
            {
                Id = 2,
                Name = "Sam",
                Salary = 99.9m,
                Address = new CnAddress
                {
                    City = "Shanghai",
                    Street = "ZiXing Rd",
                    PostCode = "201104"
                }
            };
            _employees.Add(employee);

            employee = new Seller
            {
                Id = 3,
                Name = "Tony",
                Bonus = 91099.998,
                Address = new UsAddress
                {
                    City = "Redmond",
                    Street = "One Microsoft Way",
                    ZipCode = "1001"
                }
            };
            _employees.Add(employee);
        }

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_employees);
        }

        [EnableQuery]
        public IHttpActionResult Get(int key)
        {
            return Ok(_employees.FirstOrDefault(e => e.Id == key));
        }

        public IHttpActionResult Post(Employee employee)
        {
            employee.Id = _employees.Max(e => e.Id) + 1;
            _employees.Add(employee);

            Manager manager = employee as Manager;
            if (manager != null)
            {
                Assert.Equal("Peter", manager.Name);
                Assert.NotNull(manager.Address);
                UsAddress usAddress = Assert.IsType<UsAddress>(manager.Address);
                Assert.Equal("NewYork", usAddress.City);

                return Ok("Post() with Manager"); // a guard
            }

            Seller seller = employee as Seller;
            if (seller != null)
            {
                Assert.Equal("John", seller.Name);
                Assert.NotNull(seller.Address);
                CnAddress cnAddress = Assert.IsType<CnAddress>(seller.Address);
                Assert.Equal("Shanghai", cnAddress.City);

                return Ok("Post() with Seller"); // a guard
            }

            Assert.NotNull(employee.Address);
            Assert.Equal("London", employee.Address.City);

            return Ok("Post()");
        }

        [HttpPost]
        public IHttpActionResult PostFromManager(Manager manager)
        {
            manager.Id = _employees.Max(e => e.Id) + 1;
            _employees.Add(manager);

            return Ok("PostFromManager()");
        }

        public IHttpActionResult PostFromSeller(Seller saler)
        {
            saler.Id = _employees.Max(e => e.Id) + 1;
            _employees.Add(saler);

            return Ok("PostFromSeller()");
        }
    }
}
