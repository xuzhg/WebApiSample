using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalDateTimeSample.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
