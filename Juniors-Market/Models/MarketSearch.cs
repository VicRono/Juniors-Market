﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Juniors_Market.Models
{
    public class MarketSearch
    {
        [Key]
        public int Id { get; set; }

        public string SearchId { get; set; }


        
        [Display(Name = "Market Name")]
        public string Marketname { get; set; }

    }

    public class MarketSearchResult 
    {
        public List<MarketSearch> Results { get; set; }

    }
}