using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.OData;

namespace ODataByteSample
{
    public class CustomersController : ODataController
    {
        [EnableQuery]
        public IQueryable<Customer> Get()
        {
            var customers = Enumerable.Range(1, 5).Select(e => new Customer
            {
                Id = e,
                Content = new byte[] {(byte) e, (byte) (10 + e)},
                Bytes = Enumerable.Range(1, 5).Select(f => (byte) f),
                Contents = Enumerable.Range(1, 3).Select(g => g%2 == 0 ? (byte?) null : (byte) g).ToList()
            });

            return customers.AsQueryable();
        }
    }
}
