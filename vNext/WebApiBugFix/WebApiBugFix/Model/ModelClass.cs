using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garnet.Data.Entities.Values;
using Garnet.Data.Enums;

namespace Garnet.Data.Entities
{
    public class ContainerEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Created { get; set; }
        public IList<PropertyEntity> Properties { get; set; }
        public IList<ItemEntity> Items { get; set; }
        public IList<ContainerRelationshipEntity> Relationships { get; set; }
    }

    public class ItemEntity
    {
        public int Id { get; set; }
        public int ContainerId { get; set; }
        public DateTimeOffset Created { get; set; }

        public ContainerEntity Container { get; set; }
        public IList<PropertyValueEntity> Values { get; set; }
    }

    public class PropertyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TypeOfValue Type { get; set; }
        public int ContainerId { get; set; }

        public DateTimeOffset Created { get; set; }
        public ContainerEntity Container { get; set; }

        public IList<ContainerRelationshipEntity> ForeignRelationships { get; set; }
        public IList<ContainerRelationshipEntity> PrimaryRelationships { get; set; }
    }

    public class PropertyValueEntity
    {
        public int Id { get; set; }
        public string DataValue { get; set; }

        public int IntValueId { get; set; }
        public int StringValueId { get; set; }
        public int BoolValueId { get; set; }
        public int DoubleValueId { get; set; }
        public int PropertyId { get; set; }
        public int ItemId { get; set; }

        public DateTimeOffset Created { get; set; }

        public IntValueEntity IntValue { get; set; }

        public BoolValueEntity BoolValue { get; set; }

        public DoubleValueEntity DoubleValue { get; set; }

        public StringValueEntity StringValue { get; set; }

        public PropertyEntity Property { get; set; }

        public ItemEntity Item { get; set; }
    }

    public class ContainerRelationshipEntity
    {
        public int Id { get; set; }
        public int PrimaryId { get; set; }
        public int ForeignId { get; set; }
        public DateTimeOffset Created { get; set; }
        public PropertyEntity Primary { get; set; }
        public PropertyEntity Foreign { get; set; }
    }
}

namespace Garnet.Data.Entities.Values
{
    public class BoolValueEntity
    {
        public int Id { get; set; }
        public bool DataValue { get; set; }
        public DateTimeOffset Created { get; set; }
        public int PropertyValueId { get; set; }
        public PropertyValueEntity PropertyValue { get; set; }
    }

    public class DoubleValueEntity
    {
        public int Id { get; set; }
        public Double DataValue { get; set; }
        public DateTimeOffset Created { get; set; }
        public int PropertyValueId { get; set; }
        public PropertyValueEntity PropertyValue { get; set; }
    }

    public class IntValueEntity
    {
        public int Id { get; set; }
        public int DataValue { get; set; }
        public DateTimeOffset Created { get; set; }
        public int PropertyValueId { get; set; }
        public PropertyValueEntity PropertyValue { get; set; }
    }

    public class StringValueEntity
    {
        public int Id { get; set; }
        public string DataValue { get; set; }
        public DateTimeOffset Created { get; set; }
        public int PropertyValueId { get; set; }
        public PropertyValueEntity PropertyValue { get; set; }
    }
}


namespace Garnet.Data.Enums
{
    public enum TypeOfValue
    {
        Int,
        String,
        Double,
        Bool,
        Null
    }
}
