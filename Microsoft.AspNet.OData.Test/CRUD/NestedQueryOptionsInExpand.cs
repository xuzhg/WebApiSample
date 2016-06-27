using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using Xunit;

namespace Microsoft.AspNet.OData.Test.CRUD
{
    public class NestedQueryOptionsInExpand
    {
        private readonly HttpConfiguration _configuration;
        private HttpClient _client;

        public NestedQueryOptionsInExpand()
        {
            _configuration = new[] { typeof(MetadataController), typeof(MeasuresController) }.GetHttpConfiguration();
            _configuration.MapODataServiceRoute("odata", "odata", GetEdmModel());
            HttpServer server = new HttpServer(_configuration);
            _client = new HttpClient(server);
        }

        [Fact]
        public void MetadataTest()
        {
            string requestUri = "http://localhost/odata/$metadata";

            string result = @"<?xml version=""1.0"" encoding=""utf-8""?>
<edmx:Edmx Version=""4.0"" xmlns:edmx=""http://docs.oasis-open.org/odata/ns/edmx"">
  <edmx:DataServices>
    <Schema Namespace=""Microsoft.AspNet.OData.Test.CRUD"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityType Name=""ProtoMeasure"">
        <Key>
          <PropertyRef Name=""Id"" />
        </Key>
        <Property Name=""Id"" Type=""Edm.Int32"" Nullable=""false"" />
        <NavigationProperty Name=""Metadata"" Type=""Microsoft.AspNet.OData.Test.CRUD.MeasureMetadata"" />
      </EntityType>
      <EntityType Name=""MeasureMetadata"">
        <Key>
          <PropertyRef Name=""MeasureId"" />
        </Key>
        <Property Name=""MeasureId"" Type=""Edm.Int32"" Nullable=""false"" />
        <NavigationProperty Name=""PackageTypes"" Type=""Collection(Microsoft.AspNet.OData.Test.CRUD.PackageType)"" />
      </EntityType>
      <EntityType Name=""PackageType"">
        <Key>
          <PropertyRef Name=""Id"" />
        </Key>
        <Property Name=""Id"" Type=""Edm.Int32"" Nullable=""false"" />
        <Property Name=""Name"" Type=""Edm.String"" />
        <Property Name=""Version"" Type=""Edm.Binary"" />
        <Property Name=""InsertedTime"" Type=""Edm.DateTimeOffset"" Nullable=""false"" />
        <Property Name=""UpdatedTime"" Type=""Edm.DateTimeOffset"" Nullable=""false"" />
      </EntityType>
    </Schema>
    <Schema Namespace=""Default"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityContainer Name=""Container"">
        <EntitySet Name=""Measures"" EntityType=""Microsoft.AspNet.OData.Test.CRUD.ProtoMeasure"" />
      </EntityContainer>
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();

            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void CanQuerySingleEntity()
        {
            string requestUri = "http://localhost/odata/Measures(1)";

            string result = @"{
  ""@odata.context"":""http://localhost/odata/$metadata#Measures/$entity"",""Id"":1
}";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void CanExpand_1_Level_OnSingleEntity()
        {
            string requestUri = "http://localhost/odata/Measures(1)?$expand=Metadata";

            string result = @"{
  ""@odata.context"":""http://localhost/odata/$metadata#Measures/$entity"",""Id"":1,""Metadata"":{
    ""MeasureId"":11
  }
}";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void CanExpand_2_Level_OnSingleEntity()
        {
            string requestUri = "http://localhost/odata/Measures(1)?$expand=Metadata($expand=PackageTypes)";

            string result = @"{
  ""@odata.context"":""http://localhost/odata/$metadata#Measures/$entity"",""Id"":1,""Metadata"":{
    ""MeasureId"":11,""PackageTypes"":[
      {
        ""@odata.type"":""#Microsoft.AspNet.OData.Test.CRUD.PackageType"",""Id"":111,""Name"":""Tim"",""Version"":""AQID""
      },{
        ""@odata.type"":""#Microsoft.AspNet.OData.Test.CRUD.PackageType"",""Id"":112,""Name"":""Sam"",""Version"":""AQID""
      },{
        ""@odata.type"":""#Microsoft.AspNet.OData.Test.CRUD.PackageType"",""Id"":113,""Name"":""Tony"",""Version"":""AQID""
      }
    ]
  }
}";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void CanNestedQueryOptionsInExpand_2_Level_OnSingleEntity()
        {
            string requestUri = "http://localhost/odata/Measures(1)?$expand=Metadata($expand=PackageTypes($select=Name;$top=1;$skip=1))";

            string result = @"{
  ""@odata.context"":""http://localhost/odata/$metadata#Measures(Metadata(PackageTypes(Name)))/$entity"",""Id"":1,""Metadata"":{
    ""MeasureId"":11,""PackageTypes"":[
      {
        ""@odata.type"":""#Microsoft.AspNet.OData.Test.CRUD.PackageType"",""Name"":""Sam""
      }
    ]
  }
}";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = _client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();
            Assert.Equal(result, response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public void CanNestedFilterInExpand_2_Level_OnSingleEntity_Failed()
        {
            string requestUri = "http://localhost/odata/Measures(1)?$expand=Metadata($expand=PackageTypes($select=Name;$filter=Name eq 'Sam'))";

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("Get"), requestUri);
            HttpResponseMessage response = _client.SendAsync(request).Result;

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<ProtoMeasure>("Measures");
            return builder.GetEdmModel();
        }
    }

    public class MeasuresController : ODataController
    {
        private static IList<ProtoMeasure> _measures;
        static MeasuresController()
        {
            _measures = new List<ProtoMeasure>();

            ProtoMeasure measure = new ProtoMeasure
            {
                Id = 1,
                Metadata = new MeasureMetadata
                {
                    MeasureId = 11,
                    PackageTypes = Enumerable.Range(1, 3).Select(e =>
                        new PackageType
                        {
                            Id = 110 + e,
                            Name = new[] {"Tim", "Sam", "Tony" }[e-1],
                            Version = new byte[] { 1, 2, 3}
                        }).ToList()
                }
            };

            _measures.Add(measure);

            measure = new ProtoMeasure
            {
                Id = 2,
                Metadata = new MeasureMetadata
                {
                    MeasureId = 22,
                    PackageTypes = Enumerable.Range(1, 3).Select(e =>
                        new PackageType
                        {
                            Id = 220 + e,
                            Name = new[] { "Man", "Woman", "Kid" }[e - 1],
                            Version = new byte[] { 7, 8, 9 }
                        }).ToList()
                }
            };

            _measures.Add(measure);
        }

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_measures);
        }

        [EnableQuery]
        public IHttpActionResult Get(int key)
        {
            ProtoMeasure measure = _measures.FirstOrDefault(c => c.Id == key);
            if (measure == null)
            {
                return NotFound();
            }

            return Ok(measure);
        }
    }

    public class ProtoMeasure
    {
        public int Id { get; set; }

        public MeasureMetadata Metadata { get; set; }
    }

    public class MeasureMetadata
    {
        [Key]
        public int MeasureId { get; set; }

        public IList<PackageType> PackageTypes { get; set; }
    }

    public class PackageType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Version { get; set; }
    }
}
