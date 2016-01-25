using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateTimeOffsetWithEfTest
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime Birthday { get; set; }

        //[Column(TypeName = "smalldatetime")] // doesn't work
        public DateTimeOffset ExpirationDate { get; set; }
    }

    public class CustomerContext : DbContext
    {
        public CustomerContext() : base("DateTimeOffsetWithEfTest")
        { }

        public IDbSet<Customer> Customers { get; set; }

        /* doesn't work for datetimeoffset
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var customer = modelBuilder.Entity<Customer>();
            customer.Property(f => f.ExpirationDate).HasColumnType("smalldatetime");
            base.OnModelCreating(modelBuilder);
        }
         * */
    }
}
