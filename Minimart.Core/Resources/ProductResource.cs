using Minimart.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Resources
{
    public class ProductResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }        
        public Category Category { get; set; }
    }
}
