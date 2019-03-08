using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Juniors_Market.Models
{
    public class MarketDetail
    {
        [Key]
        public int MarketDetailId { get; set; }

        [ForeignKey("MarketSearch")]
        public string MarketId { get; set; }
        public MarketSearch MarketSearch { get; set; }

        public string Address { get; set; }
        public string GoogleLink { get; set; }
        public string Products { get; set; }
        public string Schedule { get; set; }

    }

    public class MarketDetails
    {
        public MarketDetail marketdetail { get; set; }
    }
}