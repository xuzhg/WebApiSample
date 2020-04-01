
// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace AspNetCore3xEndpointSample.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public Department Department { get; set; }

        public IList<Department> DepartList { get; set; }

        public ICollection<Department> DepartCollection { get; set; }

//        public IEnumerable<Department> DepartEnumer { get; set; }
    }

    public class Department
    {
        public int Id { get; set; }

        public string DepName { get; set; }
    }
}
