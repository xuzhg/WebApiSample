using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ODataByteSample
{
    public class Customer
    {
        public int Id { get; set; }

        public byte[] Content { get; set; }

        public IEnumerable<byte> Bytes { get; set; }

        public ICollection<byte?> Contents { get; set; }
    }
}
