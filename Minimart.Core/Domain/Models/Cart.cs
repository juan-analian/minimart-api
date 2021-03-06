using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Domain.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
        public DateTime CreatedAt { get; set; }
        public string VoucherId { get; set; }
        public decimal Total { get; set; }
        public decimal TotalWithDiscount { get; set; }
        public List<CartItem> items { get; set; } = new List<CartItem>();
    }
}
