using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Juniors_Market.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string Body { get; set; }

        [ForeignKey("Customer")]
        public string CustomerName { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("MarketSearch")]
        public string MarketName { get; set; }
        public MarketSearch MarketSearch { get; set; }
    }
}