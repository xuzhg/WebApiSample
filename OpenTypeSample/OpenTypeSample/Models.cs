using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTypeSample
{
    public class Book
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public Press Press { get; set; }
        public Dictionary<string, object> DynamicProperties { get; set; }
    }

    public class Press
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Category Category { get; set; }
        public IDictionary<string, object> DynamicProperties { get; set; }
    }

    public enum Category
    {
        Book,
        Magazine,
        EBook
    }

    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
    }
}
