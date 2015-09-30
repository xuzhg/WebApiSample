using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using Microsoft.OData.Core;

namespace DollarLevelSample
{
    public class TreeItemsController : ODataController
    {
        // GET: odata/TreeItems
        [EnableQuery(MaxExpansionDepth = 5)]
        public IHttpActionResult GetTreeItems()
        {
            var treeItems = new List<TreeItem>();
            treeItems.Add(new TreeItem()
            {
                Id = 1,
                Name = "Geleiding",
                Children = new List<TreeItem>()
                {
                    new TreeItem()
                    {
                        Id = 4, Name = "Item1-1", Children = new List<TreeItem>()
                    }
                }
            });

            treeItems.Add(new TreeItem() {Id = 2, Name = "Item 2", Children = new List<TreeItem>()});
            treeItems.Add(new TreeItem() {Id = 3, Name = "Item 3", Children = new List<TreeItem>()});

            treeItems.Add(new TreeItem
            {
                Id = 5, 
                Name = "Item 5",
                Children = new List<TreeItem>
                {
                    new TreeItem()
                    {
                        Id = 51,
                        Name = "SubItem_1_Of_Item5",
                        Children = new List<TreeItem>()
                        {
                            new TreeItem()
                            {
                                Id = 511,
                                Name = "SubSubItem_1_Of_Item5",
                                Children = new List<TreeItem>()
                                {
                                    new TreeItem()
                                    {
                                        Id = 5111,
                                        Name = "SubSubSubItem_1_Of_Item5",
                                        Children = new List<TreeItem>()
                                    }
                                }
                            }
                        }
                    }
                }
            });

            return Ok(treeItems);
        }
    }
}
