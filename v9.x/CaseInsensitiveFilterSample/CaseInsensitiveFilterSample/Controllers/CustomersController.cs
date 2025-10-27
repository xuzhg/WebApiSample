using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace CaseInsensitiveFilterSample.Controllers
{
    public class  CustomersController : Controller
    {
        private readonly MyContext _db;

        public CustomersController(MyContext db)
        {
            _db = db;
            db.Customers.Where(c => EF.Functions.Collate(c.LastName, "NOCASE") == "dog").ToList();
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_db.Customers);
        }
    }
}
