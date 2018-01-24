using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;
using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.OData;
using System.Web.OData.Query;

namespace ODataOWin
{
    public class MyEnableQueryAttribute : EnableQueryAttribute
    {
        public override IQueryable ApplyQuery(IQueryable queryable, ODataQueryOptions queryOptions)
        {
            // Don't apply Skip and Top.
            var ignoreQueryOptions = AllowedQueryOptions.Select | AllowedQueryOptions.Expand;
            return queryOptions.ApplyTo(queryable, ignoreQueryOptions);
        }
    }

    public class ProductsController : ODataController
    {
        private IList<Product> _products = new List<Product>();

        public ProductsController()
        {
            _products.Add(new Product
            {
                Id = 1,
                Price = 1.1m,
                Name = "Sam",
                LastUpdate = new DateTimeOffset(2018, 1, 24, 1, 1, 1, TimeSpan.Zero)
            });

            _products.Add(new Product
            {
                Id = 2,
                Price = 2.2m,
                Name = "Peter",
                LastUpdate = new DateTimeOffset(1988, 11, 4, 1, 1, 1, TimeSpan.Zero)
            });
        }
        /*
        [EnableQuery]
        public IQueryable<Product> Get(ODataQueryOptions<Product> query)
        {
            return _products.AsQueryable();
        }

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_products);
        }
        */
        
        [EnableQuery]
        public IHttpActionResult Get(ODataQueryOptions<Product> query)
        {
            IEdmEntityType edmType = query.Context.ElementType as IEdmEntityType;
            IList<string> defaultSelects, defaultHiddens;
            Get(query.Context.Model, edmType, out defaultSelects, out defaultHiddens);

            var queryNameValuePairs = query.Request.GetQueryNameValuePairs().ToDictionary(p => p.Key, p => p.Value);
            string selectValue;
            if(queryNameValuePairs.TryGetValue("$select", out selectValue))
            {
                foreach (var prop in defaultSelects)
                {
                    if (!selectValue.Contains(prop))
                    {
                        selectValue += ("," + prop);
                    }
                }

            }
            else
            {
                selectValue = string.Join(",", defaultSelects);
            }

            string expandValue;
            if (queryNameValuePairs.TryGetValue("$expand", out expandValue))
            {

            }
            queryNameValuePairs["$select"] = selectValue;

            IDictionary<string, string> options = new Dictionary<string, string>();
            options["$select"] = selectValue;
            if (expandValue != null)
            {
                options["$expand"] = expandValue;
            }
            

            ODataQueryOptionParser parser = new ODataQueryOptionParser(query.Context.Model, query.Context.ElementType, query.Context.NavigationSource, options);
            SelectExpandQueryOption selectExpand = new SelectExpandQueryOption(selectValue, expandValue, query.Context, parser);
            var a = selectExpand.SelectExpandClause;

            ODataQuerySettings settings = new ODataQuerySettings();

            var ret = selectExpand.ApplyTo(_products.AsQueryable(), settings);
            Type type = ret.GetType();

            return Ok(ret, type);
            // return queryOptions.ApplyTo(_products.AsQueryable());
        }

        private IHttpActionResult Ok(object content, Type type)
        {
            var resultType = typeof(OkNegotiatedContentResult<>).MakeGenericType(type);
            return Activator.CreateInstance(resultType, content, this) as IHttpActionResult;
        }

        public static bool Get(IEdmModel model, IEdmEntityType target, out IList<string> defaultSelects, out IList<string> defaultHiddens)
        {
            defaultSelects = new List<string>();
            defaultHiddens = new List<string>();
            IEdmTerm term = model.FindTerm("NS.MyTerm");

            IEdmVocabularyAnnotation annotation = model.FindVocabularyAnnotations<IEdmVocabularyAnnotation>(target, term).FirstOrDefault();
            if (annotation == null)
            {
                return false;
            }

            IEdmRecordExpression record = (IEdmRecordExpression)annotation.Value;

            defaultSelects = GetCollectionPropertyPath(record, "DefaultSelect");
            defaultHiddens = GetCollectionPropertyPath(record, "DefaultHidden");
            return true;
        }

        public static IList<string> GetCollectionPropertyPath(IEdmRecordExpression record, string propertyName)
        {
            if (record.Properties != null)
            {
                IEdmPropertyConstructor property = record.Properties.FirstOrDefault(e => e.Name == propertyName);
                if (property != null)
                {
                    IEdmCollectionExpression value = property.Value as IEdmCollectionExpression;
                    if (value != null && value.Elements != null)
                    {
                        IList<string> properties = new List<string>();
                        foreach (var a in value.Elements.Select(e => e as IEdmPathExpression))
                        {
                            properties.Add(a.Path);
                        }

                        if (properties.Any())
                        {
                            return properties;
                        }
                    }
                }
            }

            return new List<string>();
        }
    }
}
