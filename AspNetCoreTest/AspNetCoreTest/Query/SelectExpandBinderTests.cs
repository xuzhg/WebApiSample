
using System.Linq;
using System.Linq.Expressions;
using NS;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using Microsoft.AspNet.OData.Query.Expressions;
using Xunit;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Test.Query
{
    public class SelectExpandBinderTests
    {
        private readonly IEdmModel _model;
        private readonly IEdmEntityType _customer;
        private readonly IEdmEntityType _vipCustomer;
        private readonly IEdmComplexType _address;
        private readonly IEdmEntitySet _customers;

        public SelectExpandBinderTests()
        {
            _model = EdmModelBuilder.GetEdmModel();
            _customer = _model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "Customer");
            _vipCustomer = _model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "VipCustomer");
            _address = _model.SchemaElements.OfType<IEdmComplexType>().First(c => c.Name == "Address");
            _customers = _model.EntityContainer.FindEntitySet("Customers");
        }

        [Theory]
        [InlineData(HandleNullPropagationOption.False)]
        [InlineData(HandleNullPropagationOption.True)]
        public void CreateNamedPropertyExpression_NonDerivedProperty_ReturnsMemberAccessExpression(HandleNullPropagationOption options)
        {
            SelectExpandBinder binder = GetBinder<Customer>(_model, options);

            Expression customer = Expression.Constant(new Customer());
            IEdmStructuralProperty homeAddressProperty = _customer.StructuralProperties().Single(c => c.Name == "HomeLocation");

            ODataSelectPath selectPath = new ODataSelectPath(new PropertySegment(homeAddressProperty));
            PathSelectItem pathSelectItem = new PathSelectItem(selectPath);

            NamedPropertyExpression namedProperty = binder.CreateNamedPropertyExpression(customer, _customer, pathSelectItem);

            Assert.NotNull(namedProperty);
            Assert.Equal("\"HomeLocation\"", namedProperty.Name.ToString());

            if (options != HandleNullPropagationOption.True)
            {
                Assert.Equal("value(NS.Customer).HomeLocation", namedProperty.Value.ToString());
            }
            else
            {
                Assert.Equal("IIF((value(NS.Customer) == null)," +
                    " null," +
                    " value(NS.Customer).HomeLocation)", namedProperty.Value.ToString());
            }
        }

        [Theory]
        [InlineData(HandleNullPropagationOption.False)]
        [InlineData(HandleNullPropagationOption.True)]
        public void CreateNamedPropertyExpression_WithDerivedProperty_ReturnsMemberAccessExpression(HandleNullPropagationOption options)
        {
            SelectExpandBinder binder = GetBinder<Customer>(_model, options);

            //Expression customer = Expression.Parameter(typeof(Customer)); output will be different.
            Expression customer = Expression.Constant(new Customer());
            IEdmStructuralProperty vipAddressProperty = _vipCustomer.StructuralProperties().Single(c => c.Name == "VipAddress");

            ODataSelectPath selectPath = new ODataSelectPath(new TypeSegment(_vipCustomer, _customer, null),
                new PropertySegment(vipAddressProperty));
            PathSelectItem pathSelectItem = new PathSelectItem(selectPath);

            NamedPropertyExpression namedProperty = binder.CreateNamedPropertyExpression(customer, _customer, pathSelectItem);

            Assert.NotNull(namedProperty);
            Assert.Equal("\"VipAddress\"", namedProperty.Name.ToString());

            if (options != HandleNullPropagationOption.True)
            {
                Assert.Equal("(value(NS.Customer) As VipCustomer).VipAddress", namedProperty.Value.ToString());
            }
            else
            {
                Assert.Equal("IIF(((value(NS.Customer) As VipCustomer) == null)," +
                    " null," +
                    " (value(NS.Customer) As VipCustomer).VipAddress)", namedProperty.Value.ToString());
            }
        }

        [Theory]
        [InlineData(HandleNullPropagationOption.False)]
        [InlineData(HandleNullPropagationOption.True)]
        public void CreateNamedPropertyExpression_WithMultipleProperty_ReturnsMemberAccessExpression(HandleNullPropagationOption options)
        {
            SelectExpandBinder binder = GetBinder<Customer>(_model, options);

            //Expression customer = Expression.Parameter(typeof(Customer)); output will be different.
            Expression customer = Expression.Constant(new Customer());
            IEdmStructuralProperty homeLocationProperty = _customer.StructuralProperties().Single(c => c.Name == "HomeLocation");

            IEdmStructuralProperty streetProperty = _address.StructuralProperties().Single(c => c.Name == "Street");

            ODataSelectPath selectPath = new ODataSelectPath(new PropertySegment(homeLocationProperty),
                new PropertySegment(streetProperty));

            PathSelectItem pathSelectItem = new PathSelectItem(selectPath);

            NamedPropertyExpression namedProperty = binder.CreateNamedPropertyExpression(customer, _customer, pathSelectItem);

            Assert.NotNull(namedProperty);
            /*
            Assert.NotNull(namedProperty);
            Assert.Equal("\"VipAddress\"", namedProperty.Name.ToString());

            if (options != HandleNullPropagationOption.True)
            {
                Assert.Equal("(value(NS.Customer) As VipCustomer).VipAddress", namedProperty.Value.ToString());
            }
            else
            {
                Assert.Equal("IIF(((value(NS.Customer) As VipCustomer) == null)," +
                    " null," +
                    " (value(NS.Customer) As VipCustomer).VipAddress)", namedProperty.Value.ToString());
            }*/
        }

        [Theory]
        [InlineData(HandleNullPropagationOption.False)]
        [InlineData(HandleNullPropagationOption.True)]
        public void CreateNamedPropertyExpression_WithMultiplePropertyAndType_ReturnsMemberAccessExpression(HandleNullPropagationOption options)
        {
            SelectExpandBinder binder = GetBinder<Customer>(_model, options);

            Expression customer = Expression.Parameter(typeof(Customer)); // output will be different.
            // Expression customer = Expression.Constant(new Customer());

            SelectExpandClause selectExpandClause = ParseSelectExpand("HomeLocation/NS.CnAddress/Street", null);

            PathSelectItem pathSelectItem = selectExpandClause.SelectedItems.First() as PathSelectItem;

            NamedPropertyExpression namedProperty = binder.CreateNamedPropertyExpression(customer, _customer, pathSelectItem);

            Assert.NotNull(namedProperty);
            /*
            Assert.NotNull(namedProperty);
            Assert.Equal("\"VipAddress\"", namedProperty.Name.ToString());

            if (options != HandleNullPropagationOption.True)
            {
                Assert.Equal("(value(NS.Customer) As VipCustomer).VipAddress", namedProperty.Value.ToString());
            }
            else
            {
                Assert.Equal("IIF(((value(NS.Customer) As VipCustomer) == null)," +
                    " null," +
                    " (value(NS.Customer) As VipCustomer).VipAddress)", namedProperty.Value.ToString());
            }*/
        }

        [Fact]
        public void GetPropertiesToIncludeInQuery_ReturnsMemberAccessExpression()
        {
            string select = "HomeLocation/Street,HomeLocation/Region,HomeLocation/Emails($top=2),HomeLocation($skip=3;$count=true)";
            string expand = "HomeLocation/RelatedCity,HomeLocation/Cities/$ref";
            SelectExpandClause selectExpandClause = ParseSelectExpand(select, expand);

            ISet<IEdmStructuralProperty> autoSelectedProperties;
            Dictionary<IEdmStructuralProperty, PathSelectItem> includedProperties
                = SelectExpandBinder.GetPropertiesToIncludeInQuery(selectExpandClause, _customer, _customers, _model, out autoSelectedProperties);

            Assert.NotNull(includedProperties);
        }

        private static SelectExpandBinder GetBinder<T>(IEdmModel model, HandleNullPropagationOption nullPropagation = HandleNullPropagationOption.False)
        {
            var settings = new ODataQuerySettings { HandleNullPropagation = nullPropagation };

            var context = new ODataQueryContext(model, typeof(T)) { /*RequestContainer = new MockContainer()*/ };

            return new SelectExpandBinder(settings, new SelectExpandQueryOption("*", "", context));
        }

        public SelectExpandClause ParseSelectExpand(string select, string expand)
        {
            return new ODataQueryOptionParser(_model, this._customer, _customers,
                new Dictionary<string, string>
                {
                    { "$expand", expand == null ? "" : expand },
                    { "$select", select == null ? "" : select }
                }).ParseSelectAndExpand();
        }
    }
}
