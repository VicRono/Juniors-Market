using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Juniors_Market.Models
{
    public class Email
    {
        [Key]
        public int EmailId { get; set; }
        public string ToLine { get; set; }
        public string FromLine { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}