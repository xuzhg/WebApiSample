using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.OData.Serializer;

namespace Test
{
    public class Item
    {
        public int Id { get; set; }
        public int IntProp { get; set; }
    }

    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }

    }

    public class MyTest
    {
        public int Id { get; set; }

        public int IntProp { get; set; }

        public double doubleProp { get; set; }

        public string stringProp { get; set; }

        public Address NullAddress { get; set; }

        public Address Location { get; set; }

        public IList<Address> Addresses { get; set; }

        public IList<MyTest> RelatedTests { get; set; }

    }

    class Program
    {
        static void Main(string[] args)
        {
            MyTest test = new MyTest
            {
                Id = 1,
                IntProp = 9,
                doubleProp = 19.9,
                stringProp = "abc",
                NullAddress = null,
                Location = new Address
                {
                    City = "Redmond",
                    Street = "One Microsoft Way"
                },
                Addresses = new List<Address>
                {
                    new Address
                    {
                        City = "Shanghai",
                        Street = "One Shanghai Way"
                    },
                    null,
                    new Address
                    {
                        City = "Beijing",
                        Street = "One Beijing Way"
                    }
                },
                RelatedTests = new List<MyTest>
                {
                    new MyTest
                    {
                        Id = 1,
                        IntProp = 9,
                        doubleProp = 19.9,
                        stringProp = "abc",
                        NullAddress = null,
                        Addresses = new List<Address>
                        {
                            new Address
                            {
                                City = "Shanghai",
                                Street = "One Shanghai Way"
                            },
                            null,
                            new Address
                            {
                                City = "Beijing",
                                Street = "One Beijing Way"
                            }
                        }
                    }
                }
            };

            string json = ODataConvert.ConvertToOData(test);

            Console.WriteLine(JObject.Parse(json));
        }
    }
}
