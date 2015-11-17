using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiWithEFSample
{
    public class WebApiWithEfContext : DbContext
    {
        public IDbSet<Person> People { get; set; }
    }
}
