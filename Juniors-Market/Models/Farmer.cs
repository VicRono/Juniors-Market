using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Juniors_Market.Models
{
    public class Farmer
    {
        [Key]
        public int FarmerId { get; set; }

        [Display(Name = "Market Name")]
        public string MarketName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string AspUserId { get; set; }
    }
}