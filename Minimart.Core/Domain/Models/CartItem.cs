using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Domain.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public Guid CartId { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public Product Product { get; set; }
        public decimal Price { get; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Total { get; set; }
        public decimal TotalWithDiscount { get; set; }
    }
}
