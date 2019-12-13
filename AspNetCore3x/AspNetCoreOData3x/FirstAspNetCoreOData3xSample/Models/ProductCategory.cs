// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace FirstAspNetCoreOData3xSample.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string[] EMails { get; set; }

        public byte ByteValue { get; set; }

        public byte[] Data { get; set; }

        public Address HomeAddress { get; set; }

        public Category Category { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
