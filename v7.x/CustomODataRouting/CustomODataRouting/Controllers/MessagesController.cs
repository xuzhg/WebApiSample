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
            new Message { Id = 1, Name = "abc", Likes = new List<Like> { new Like { Id = 11, Reason = "Good" }, new Like { Id = 12, Reason = "Bad" } }},
            new Message { Id = 2, Name = "ijk", Likes = new List<Like> { new Like { Id = 21, Reason = "Soso" }, new Like { Id = 22, Reason = "Not good" } }},
            new Message { Id = 3, Name = "xyz", Likes = new List<Like> { new Like { Id = 31, Reason = "Fine" }, new Like { Id = 32, Reason = "Very good" } }}
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

        /*
        [HttpPut]
        public IHttpActionResult PutToLikes(int key, [FromODataRequestBody]IList<Like> likes)
        {
            return Ok(likes);
        }*/

        [HttpPut]
        public IHttpActionResult PutToLikes(int key, [FromODataRequestBody]IEnumerable<Like> likes)
        {
            return Ok(likes);
        }
    }
}