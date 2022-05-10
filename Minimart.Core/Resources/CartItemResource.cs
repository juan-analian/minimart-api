using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Resources
{
    public class CartItemResource
    {
        public ProductItemResource Product { get; set; }

        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }

        public decimal Total { get; set; }
        public decimal TotalWithDiscount { get; set; }

    }
}
