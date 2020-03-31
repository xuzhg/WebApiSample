
// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore3xEndpointSample.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string Depid { get; set; }

        public Department Department { get; set; }
    }

    public class Department
    {
        [Key]
        public string Depid { get; set; }

        public string DepName { get; set; }
    }
}
