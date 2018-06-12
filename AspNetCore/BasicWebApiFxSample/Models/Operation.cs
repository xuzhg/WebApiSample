using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BasicWebApiFxSample.Models
{
    public class Operation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? OperationDate { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public TimeSpan? Duration { get; set; }
    }

    public class OperationContext : DbContext
    {
        public virtual DbSet<Operation> Operations { get; set; }
    }

}