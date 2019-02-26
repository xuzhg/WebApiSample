using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomODataRouting.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<Like> Likes { get; set; }
    }

    public class Like
    {
        public int Id { get; set; }

        public string Reason { get; set; }
    }
}