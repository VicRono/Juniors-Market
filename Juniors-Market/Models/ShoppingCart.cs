using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Juniors_Market.Models
{
    public class ShoppingCart
    {
        [Key]
        public int ShoppingCartId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Produce")]
        public int ProduceId { get; set; }
        public Produce Produce { get; set; }
    }
}