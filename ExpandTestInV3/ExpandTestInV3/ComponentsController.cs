using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using HostingDb.Pandora;

namespace ExpandTestInV3
{
    public class ComponentsController : ODataController
    {
        private ComponentContext db = new ComponentContext();

        // ~/odata/Components,  add support all query options
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(db.Components);
        }

        // ~/odata/Components('xxx')
        [EnableQuery]
        public IHttpActionResult Get([FromODataUri]string key)
        {
            return Ok(db.Components.First(c => c.Id == key));
        }

        // ~/odata/Components('xxx')/Childs
        [EnableQuery]
        public IQueryable<Component> GetChilds([FromODataUri]string key)
        {
            var id = db.Components.First(c => c.Id == key).Identity;
            return db.ChildComponents.Where(cc => cc.Identity == id).AsQueryable();//.Select(cc => cc.Component);
        }
    }
}
