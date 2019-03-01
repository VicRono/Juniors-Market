using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Juniors_Market.Models
{
    public class MarketSearch
    {
        public string Id { get; set; }
        public string Marketname { get; set; }


    }

    public class MarketSearchResult 
    {
        public List<MarketSearch> Results { get; set; }

    }
}