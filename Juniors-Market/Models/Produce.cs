using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Juniors_Market.Models
{
    public class Produce
    {
        [Key]
        public int ProduceId { get; set; }
        public string Fruit { get; set; }
        public string Vegetable { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("Farmer")]
        public int FarmerId { get; set; }
        public Farmer Farmer { get; set; }
    }
}