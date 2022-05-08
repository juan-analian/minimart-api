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
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
