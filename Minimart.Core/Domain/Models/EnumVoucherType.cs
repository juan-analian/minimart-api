using System.ComponentModel;

namespace Minimart.Core.Domain.Models
{
    public enum EnumVoucherType: byte
    {
        [Description("Discount Percent for products")]
        DiscountOnProducts = 1,

        [Description("Discount Percent for categories")]
        DiscountOnCategories = 2,
        
        [Description("Discount on N order unit")]
        DiscountOnNthOrderUnit = 3,

        [Description("Discount Pay n take m")]
        DiscountPayNTakeM = 4
    }
}
