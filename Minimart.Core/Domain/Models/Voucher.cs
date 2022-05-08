using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Domain.Models
{
    public class Voucher
    {
        public string Id { get; set; }
        public int StoreId { get; set; }
        public byte VoucherDiscountTypeId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public int? Percent { get; set; }
        public int? UpToUnit { get; set; }
        public int? UnitOrder { get; set; }
        public int? TakeUnits { get; set; }
        public int? PayUnits { get; set; }

    }
}
