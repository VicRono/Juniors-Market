using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Juniors_Market.Models
{
    public class MarketDetail
    {
        public string Address { get; set; }
        public string GoogleLink { get; set; }
        public string Products { get; set; }
        public string Schedule { get; set; }
    }

    public class MarketDetails
    {
        public MarketDetail marketdetails { get; set; }
    }
}