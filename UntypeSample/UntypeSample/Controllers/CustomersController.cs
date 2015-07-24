using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.OData;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Library;

namespace UntypeSample.Controllers
{
    public class CustomersController : ODataController
    {
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(GetCustomers());
        }

        [HttpGet]
        public IHttpActionResult GetName(int key)
        {
            EdmEntityObjectCollection customers = GetCustomers();
            EdmEntityObject customer = customers.FirstOrDefault(e =>
            {
                object customerId;
                if (e.TryGetPropertyValue("CustomerId", out customerId))
                {
                    return (int) customerId == key;
                }

                return false;
            }) as EdmEntityObject;

            if (customer == null)
            {
                return NotFound();
            }

            object name;
            customer.TryGetPropertyValue("Name", out name);
            if (name == null)
            {
                return NotFound();
            }

            return Ok(name, name.GetType());
        }

        public IHttpActionResult Patch(int key, IEdmEntityObject entity)
        {
            object value;
            if (entity.TryGetPropertyValue("Name", out value))
            {
                string name = value as string;
                if (name != null)
                {
                    if (name == "Sam")
                    {
                        return Ok(name);
                    }
                }
            }

            return BadRequest("Not My input.");
        }

        public HttpResponseMessage Put(int key, IEdmEntityObject entity)
        {
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [HttpGet]
        public IHttpActionResult GetColor(int key)
        {
            EdmEntityObjectCollection customers = GetCustomers();
            EdmEntityObject customer = customers.FirstOrDefault(e =>
            {
                object customerId;
                if (e.TryGetPropertyValue("CustomerId", out customerId))
                {
                    return (int)customerId == key;
                }

                return false;
            }) as EdmEntityObject;

            if (customer == null)
            {
                return NotFound();
            }

            object color;
            customer.TryGetPropertyValue("Color", out color);
            if (color == null)
            {
                return NotFound();
            }

            // return Ok(color, color.GetType());

            EdmEnumObject enumColor = color as EdmEnumObject;
            if (enumColor == null)
            {
                return NotFound();
            }

            return Ok(enumColor);
        }

        private EdmEntityObjectCollection GetCustomers()
        {
            IEdmModel edmModel = Request.ODataProperties().Model;

            IEdmEntityType customerType = edmModel.SchemaElements.OfType<IEdmEntityType>().Single(e => e.Name == "Customer");
            IEdmEnumType colorType = edmModel.SchemaElements.OfType<IEdmEnumType>().Single(e => e.Name == "Color");
            IEdmComplexType addressType = edmModel.SchemaElements.OfType<IEdmComplexType>().Single(e => e.Name == "Address");

            // an enum object
            EdmEnumObject color = new EdmEnumObject(colorType, "Red");

            // a complex object
            EdmComplexObject address1 = new EdmComplexObject(addressType);
            address1.TrySetPropertyValue("Street", "ZiXing Rd"); // Declared property
            address1.TrySetPropertyValue("StringProperty", "CN"); // a string dynamic property
            address1.TrySetPropertyValue("GuidProperty", new Guid("181D3A20-B41A-489F-9F15-F91F0F6C9ECA")); // a guid dynamic property

            // another complex object with complex dynamic property
            EdmComplexObject address2 = new EdmComplexObject(addressType);
            address2.TrySetPropertyValue("Street", "ZiXing Rd"); // Declared property
            address2.TrySetPropertyValue("AddressProperty", address1); // a complex dynamic property

            // an entity object
            EdmEntityObject customer = new EdmEntityObject(customerType);
            customer.TrySetPropertyValue("CustomerId", 1); // Declared property
            customer.TrySetPropertyValue("Name", "Mike"); // Declared property
            customer.TrySetPropertyValue("Color", color); // an enum dynamic property
            customer.TrySetPropertyValue("Address1", address1); // a complex dynamic property
            customer.TrySetPropertyValue("Address2", address2); // another complex dynamic property

            // a collection with one object
            EdmEntityObjectCollection collection =
                new EdmEntityObjectCollection(
                    new EdmCollectionTypeReference(
                        new EdmCollectionType(new EdmEntityTypeReference(customerType, isNullable: true))));

            collection.Add(customer);

            return collection;
        }

        private IHttpActionResult Ok(object content, Type type)
        {
            var resultType = typeof(OkNegotiatedContentResult<>).MakeGenericType(type);
            return Activator.CreateInstance(resultType, content, this) as IHttpActionResult;
        }

    }
}
