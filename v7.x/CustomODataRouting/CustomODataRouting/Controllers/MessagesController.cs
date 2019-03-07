using CustomODataRouting.Extensions;
using CustomODataRouting.Models;
using Microsoft.AspNet.OData;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CustomODataRouting.Controllers
{
    public class MessagesController : ODataController
    {
        private static IList<Message> _messages = new List<Message>
        {
            new Message
            {
                Id = 1, Name = "abc",
                Locations = new List<Address> { new Address { Street = "148th", City = "Redmond" }, new Address { Street = "156th", City = "Issaquah" }, },
                Likes = new List<Like> { new Like { Id = 11, Reason = "Good" }, new Like { Id = 12, Reason = "Bad" } }},
            new Message { Id = 2, Name = "ijk",
                Locations = new List<Address> { new Address { Street = "118th", City = "Redmond" }, new Address { Street = "140th", City = "Issaquah" }, },
                Likes = new List<Like> { new Like { Id = 21, Reason = "Soso" }, new Like { Id = 22, Reason = "Not good" } }},
            new Message { Id = 3, Name = "xyz",
                Locations = new List<Address> { new Address { Street = "80th", City = "Redmond" }, new Address { Street = "14th", City = "Issaquah" }, },
                Likes = new List<Like> { new Like { Id = 31, Reason = "Fine" }, new Like { Id = 32, Reason = "Very good" } }}
        };

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_messages);
        }

        [EnableQuery]
        public IHttpActionResult Get(int key)
        {
            return Ok(_messages.FirstOrDefault(m => m.Id == key));
        }

        [EnableQuery]
        public IHttpActionResult GetLikes(int key)
        {
            return Ok(_messages.FirstOrDefault(m => m.Id == key).Likes);
        }

        [EnableQuery]
        public IHttpActionResult GetLocations(int key)
        {
            return Ok(_messages.FirstOrDefault(m => m.Id == key).Locations);
        }

        /*
        [HttpPut]
        public IHttpActionResult PutToLikes(int key, [FromODataRequestBody]IList<Like> likes)
        {
            return Ok(likes);
        }*/

        [HttpPut]
        public IHttpActionResult PutToLocations(int key, [FromBody]IEnumerable<Address> locations)
        {
            if (locations == null)
            {
                return Ok("Null");
            }

            return Ok(locations);
        }

        [HttpPut]
        public IHttpActionResult PutToLikes(int key, [FromODataRequestBody]IEnumerable<Like> likes)
        {
            return Ok(likes);
        }
    }
}