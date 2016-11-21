using Garnet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

namespace WebApiBugFix.Controllers
{
    public class ContainersController : ODataController
    {
        [EnableQuery(MaxExpansionDepth = 9)]
        public IHttpActionResult Get()
        {
            return Ok(DataResource.Containers);
        }
    }

    public class DataResource
    {
        public static IList<ContainerEntity> Containers = new List<ContainerEntity>();

        static DataResource()
        {
            ContainerEntity container = new ContainerEntity();
            container.Id = 1;
            container.Name = "Products";
            container.Created = DateTimeOffset.Now;
            container.Items = new List<ItemEntity>();
            container.Properties = new List<PropertyEntity>();
            container.Relationships = new List<ContainerRelationshipEntity>();
            Containers.Add(container);

            PropertyEntity property = new PropertyEntity();
            property.Id = 11;
            property.Name = "Property";
            property.Type = Garnet.Data.Enums.TypeOfValue.Int;
            property.PrimaryRelationships = new List<ContainerRelationshipEntity>();
            property.ForeignRelationships = new List<ContainerRelationshipEntity>();
            property.Container = container;
            container.Properties.Add(property);

            ItemEntity item = new ItemEntity();
            item.Id = 21;
            item.ContainerId = 1;
            item.Container = container;
            item.Created = DateTimeOffset.Now;
            item.Values = new List<PropertyValueEntity>();
            container.Items.Add(item);

            ContainerRelationshipEntity relation = new ContainerRelationshipEntity();
            relation.Id = 31;
            relation.PrimaryId = 41;
            relation.Primary = new PropertyEntity();
            relation.ForeignId = 51;
            relation.Foreign = new PropertyEntity();
            container.Relationships.Add(relation);

            PropertyValueEntity value = new PropertyValueEntity();
            value.Id = 61;
            value.IntValue = new Garnet.Data.Entities.Values.IntValueEntity();
            value.IntValue.Id = 101;
            value.IntValueId = 101;
            value.IntValue.DataValue = 999;
            value.IntValue.Created = DateTimeOffset.Now;
            value.IntValue.PropertyValue = value;
            value.IntValue.PropertyValueId = 61;


            value.BoolValue = new Garnet.Data.Entities.Values.BoolValueEntity();
            value.BoolValue.Id = 201;
            value.BoolValueId = 201;
            value.BoolValue.DataValue = true;
            value.BoolValue.Created = DateTimeOffset.Now;
            value.BoolValue.PropertyValue = value;
            value.BoolValue.PropertyValueId = 61;

            value.DoubleValue = new Garnet.Data.Entities.Values.DoubleValueEntity();
            value.DoubleValue.Id = 301;
            value.DoubleValueId = 301;
            value.DoubleValue.DataValue = 9.9;
            value.DoubleValue.Created = DateTimeOffset.Now;
            value.DoubleValue.PropertyValue = value;
            value.DoubleValue.PropertyValueId = 61;

            value.StringValue = new Garnet.Data.Entities.Values.StringValueEntity();
            value.StringValue.Id = 401;
            value.StringValueId = 401;
            value.StringValue.DataValue = "StringValue";
            value.StringValue.Created = DateTimeOffset.Now;
            value.StringValue.PropertyValue = value;
            value.StringValue.PropertyValueId = 61;

            item.Values.Add(value);
        }
    }
}
