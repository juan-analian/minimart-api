using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Resources
{
    public class CartResource
    {
        public Guid Id { get; set; }
         
        public CartStoreResource Store { get; set; }
        public DateTime CreatedAt { get; set; }
        public string VoucherId { get; set; }

        public List<CartItemResource> Items { get; set; } 

        public decimal Total { get; set; }
        public decimal TotalWithDiscount { get; set; }
    }
}
