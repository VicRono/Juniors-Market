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
        public string Address { get; set; }
        public string GoogleLink { get; set; }
        public string Products { get; set; }
        public string Schedule { get; set; }

        [ForeignKey("MarketSearch")]
        public string marketId { get; set; }
        public MarketSearch MarketSearch { get; set; }

    }

    public class MarketDetails
    {
        public List<MarketDetail> marketdetails { get; set; }
    }
}