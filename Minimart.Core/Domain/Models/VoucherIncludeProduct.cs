using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Domain.Models
{
    public class VoucherIncludeProduct
    {
        public string VoucherId { get; set; }
        public int ProductId { get; set; }
    }
}
