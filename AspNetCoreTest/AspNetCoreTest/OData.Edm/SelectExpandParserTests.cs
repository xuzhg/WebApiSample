using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using NS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Microsoft.AspNetCore.Test.OData.Edm
{
    public class SelectExpandParserTests
    {
        private readonly IEdmModel _model;
        private readonly IEdmEntityType _customer;
        private readonly IEdmEntityType _vipCustomer;
        private readonly IEdmComplexType _address;
        private readonly IEdmEntitySet _customers;

        public SelectExpandParserTests()
        {
            _model = EdmModelBuilder.GetEdmModel();
            _customer = _model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "Customer");
            _vipCustomer = _model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "VipCustomer");
            _address = _model.SchemaElements.OfType<IEdmComplexType>().First(c => c.Name == "Address");
            _customers = _model.EntityContainer.FindEntitySet("Customers");
        }

        [Fact]
        public void TestSelectAndExpand()
        {
            var selectExpandClause = new ODataQueryOptionParser(_model, this._customer, _customers,
                new Dictionary<string, string>
                {
                    { "$expand", "" },
                    { "$select", "Locations/NS.CnAddress/PostCode" }
                   // { "$select", "Locations/NS.CnAddress" } // this should be same as "$select=Locations"

                }).ParseSelectAndExpand();

            Assert.NotNull(selectExpandClause);
        }

        [Fact]
        public void TestSelectAndExpand2()
        {
            var selectExpandClause = new ODataQueryOptionParser(_model, this._customer, _customers,
                new Dictionary<string, string>
                {
                    { "$expand", "" },
                    { "$select", "Locations,Locations/NS.CnAddress/PostCode" }
                   // { "$select", "Locations/NS.CnAddress" } // this should be same as "$select=Locations"

                }).ParseSelectAndExpand();

            Assert.NotNull(selectExpandClause);
        }

        [Fact]
        public void TestSelectAndExpand3()
        {
            ODataSelectPath path = new ODataSelectPath();
            var selectExpandClause = new ODataQueryOptionParser(_model, this._customer, _customers,
                new Dictionary<string, string>
                {
                    { "$expand", "" },
                    { "$select", "Locations/Street,Locations/Region" }
                   // { "$select", "Locations/NS.CnAddress" } // this should be same as "$select=Locations"

                }).ParseSelectAndExpand();

            Assert.NotNull(selectExpandClause);
        }

        [Fact]
        public void TestSelectAndExpand4()
        {
            var selectExpandClause = new ODataQueryOptionParser(_model, this._customer, _customers,
                new Dictionary<string, string>
                {
                    { "$expand", "" },
                    { "$select", "HomeLocation/*" }
                   // { "$select", "Locations/NS.CnAddress" } // this should be same as "$select=Locations"

                }).ParseSelectAndExpand();

            Assert.NotNull(selectExpandClause);
        }

        [Fact]
        public void TestSelectAndExpand5()
        {
            var selectExpandClause = new ODataQueryOptionParser(_model, this._customer, _customers,
                new Dictionary<string, string>
                {
                    { "$expand", "" },
                    { "$select", "Locations/*,HomeLocation/*" }
                   // { "$select", "Locations/NS.CnAddress" } // this should be same as "$select=Locations"

                }).ParseSelectAndExpand();

            Assert.NotNull(selectExpandClause);
        }

        [Fact]
        public void TestSelectAndExpand1()
        {
            var selectExpandClause = new ODataQueryOptionParser(_model, this._customer, _customers,
                new Dictionary<string, string>
                {
                    { "$expand", "" },
                    { "$select", "NS.VipCustomer/VipName" }
                }).ParseSelectAndExpand();

            Assert.NotNull(selectExpandClause);
        }


    }
}
