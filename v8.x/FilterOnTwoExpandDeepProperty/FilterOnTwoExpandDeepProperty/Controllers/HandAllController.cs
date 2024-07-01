using FilterOnTwoExpandDeepProperty.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;

namespace FilterOnTwoExpandDeepProperty.Controllers
{
    public class HandAllController : Controller
    {
        [ODataAttributeRouting]
        [HttpGet("odata/Thing1Thing2RelationTable")]
        [EnableQuery]
        public IActionResult GetAllThings()
        {
            IList<Thing1Thing2RelationTable> table = new List<Thing1Thing2RelationTable>
            {
                new Thing1Thing2RelationTable
                {
                    Id = 1,
                    Thing2Id = 11,
                    Thing1 = new Thing1
                    {
                        Id = 111,
                        DisplayName = "Foo",
                        Attribute = new AttributeInfo
                        {
                            Type = "Float"
                        }
                    }
                },
                new Thing1Thing2RelationTable
                {
                    Id = 2,
                    Thing2Id = 22,
                    Thing1 = new Thing1
                    {
                        Id = 222,
                        DisplayName = "Zoo",
                        Attribute = new AttributeInfo
                        {
                            Type = "Decimal"
                        }
                    }
                },
                new Thing1Thing2RelationTable
                {
                    Id = 3,
                    Thing2Id = 33,
                    Thing1 = new Thing1
                    {
                        Id = 333,
                        DisplayName = "Bar",
                        Attribute = new AttributeInfo
                        {
                            Type = "Int"
                        }
                    }
                },
                new Thing1Thing2RelationTable
                {
                    Id = 4,
                    Thing2Id = 22,
                    Thing1 = new Thing1
                    {
                        Id = 333,
                        DisplayName = "Tik",
                        Attribute = new AttributeInfo
                        {
                            Type = "Money"
                        }
                    }
                }
            };

            return Ok(table);
        }
    }
}
