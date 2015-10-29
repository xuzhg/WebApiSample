using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataWithEF6Sample.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Message Msg { get; set; }
    }
}