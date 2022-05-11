using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Domain.Models
{
    public class Voucher
    {
        public string Id { get; set; }
        public int StoreId { get; set; }
        public EnumVoucherType VoucherDiscountTypeId { get; set; }
         
        public int? ValidFromDay { get; set; }
        public int? ValidFromMonth { get; set; }
        public int? ValidFromYear { get; set; }
        public int? ValidToDay { get; set; }
        public int? ValidToMonth { get; set; }
        public int? ValidToYear { get; set; }

        public int? Percent { get; set; }
        public int? UpToUnit { get; set; }
        public int? UnitOrder { get; set; }
        public int? TakeUnits { get; set; }
        public int? PayUnits { get; set; }

        public List<VoucherIncludeCategory> IncludedCategories { get; set; } = new List<VoucherIncludeCategory>();
        public List<VoucherIncludeProduct> IncludedProducts { get; set; } = new List<VoucherIncludeProduct>();
        public List<VoucherExcludeProduct> ExcludedProducts { get; set; } = new List<VoucherExcludeProduct>();
        public List<VoucherWeekDay> WeekDays { get; set; } = new List<VoucherWeekDay>();
    }
}
