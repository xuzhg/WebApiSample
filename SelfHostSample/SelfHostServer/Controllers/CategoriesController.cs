using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using SelfHostServer.Models;

namespace SelfHostServer.Controllers
{
    public class CategoriesController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(DataSource.Categories);
        }

        public IHttpActionResult Get(int key)
        {
            Category category = DataSource.Categories.FirstOrDefault(e => e.CategoryId == key);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
    }
}
