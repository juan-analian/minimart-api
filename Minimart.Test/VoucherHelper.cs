using Minimart.Core.Domain.Logic;
using Minimart.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Test
{
    public static class VoucherHelper
    {
        public static Voucher GetById(string id)
        {
            var voucher = new Voucher();
            switch (id)
            {
                case "COCO1V1F8XOG1MZZ":
                    voucher.Id = id;
                    voucher.VoucherDiscountTypeId = EnumVoucherType.DiscountOnCategories;
                    voucher.ValidFromDay = 27;
                    voucher.ValidFromMonth = 1;
                    voucher.ValidToDay = 13;
                    voucher.ValidToMonth = 2;
                    voucher.Percent = 20;
                    voucher.WeekDays.Add(new VoucherWeekDay() { VoucherId = id, WeekDay = EWeekDays.Wednesday });
                    voucher.WeekDays.Add(new VoucherWeekDay() { VoucherId = id, WeekDay = EWeekDays.Thursday });
                    voucher.IncludedCategories.Add(new VoucherIncludeCategory() { VoucherId = id, CategoryId = 2 });
                    break;

                case "COCOKCUD0Z9LUKBN":
                    voucher.Id = id;
                    voucher.VoucherDiscountTypeId = EnumVoucherType.DiscountPayNTakeM;
                    voucher.ValidFromDay = 24;
                    voucher.ValidFromMonth = 1;
                    voucher.ValidToDay = 6;
                    voucher.ValidToMonth = 2;
                    voucher.UpToUnit = 6;
                    voucher.TakeUnits = 3;
                    voucher.PayUnits = 2;
                    voucher.IncludedProducts.Add(new VoucherIncludeProduct() { VoucherId = id, ProductId = 12 });
                    break;

                case "COCOG730CNSG8ZVX":
                    voucher.Id = id;
                    voucher.VoucherDiscountTypeId = EnumVoucherType.DiscountOnCategories;
                    voucher.ValidFromDay = 31;
                    voucher.ValidFromMonth = 1;
                    voucher.ValidToDay = 9;
                    voucher.ValidToMonth = 2;
                    voucher.Percent = 10;
                    voucher.IncludedCategories.Add(new VoucherIncludeCategory() { VoucherId = id, CategoryId = 1});
                    voucher.IncludedCategories.Add(new VoucherIncludeCategory() { VoucherId = id, CategoryId = 4});
                    break;
                case "COCO2O1USLC6QR22":
                    voucher.Id = id;
                    voucher.VoucherDiscountTypeId = EnumVoucherType.DiscountOnNthOrderUnit;                    
                    voucher.ValidFromMonth = 2;                    
                    voucher.ValidToMonth = 2;
                    voucher.Percent = 30;
                    voucher.UnitOrder = 2;
                    voucher.IncludedProducts.Add(new VoucherIncludeProduct() { VoucherId = id, ProductId = 3 });
                    voucher.IncludedProducts.Add(new VoucherIncludeProduct() { VoucherId = id, ProductId = 5 });
                    voucher.IncludedProducts.Add(new VoucherIncludeProduct() { VoucherId = id, ProductId = 6 });
                    break;
                case "COCO0FLEQ287CC05":
                    voucher.Id = id;
                    voucher.VoucherDiscountTypeId = EnumVoucherType.DiscountOnNthOrderUnit;
                    voucher.ValidFromDay = 1;
                    voucher.ValidFromMonth = 2;
                    voucher.ValidToDay = 15;
                    voucher.ValidToMonth = 2;
                    voucher.Percent = 50;
                    voucher.UnitOrder = 2;
                    voucher.WeekDays.Add(new VoucherWeekDay() { VoucherId = id, WeekDay = EWeekDays.Monday });
                    voucher.IncludedProducts.Add(new VoucherIncludeProduct() { VoucherId = id, ProductId = 12 });
                    break;
                default:
                    return null;

            }

            return voucher;
        } 

        public static DiscountContext GetDiscountContext(Voucher voucher)
        {
            IDiscountStrategy strategy;
            switch (voucher.VoucherDiscountTypeId)
            {
                case EnumVoucherType.DiscountOnProducts:
                    strategy = new DiscountPerProduct(voucher);
                    break;
                case EnumVoucherType.DiscountOnCategories:
                    strategy = new DiscountPerCategory(voucher);
                    break;
                case EnumVoucherType.DiscountOnNthOrderUnit:
                    strategy = new DiscountOnOrder(voucher);
                    break;
                case EnumVoucherType.DiscountPayNTakeM:
                    strategy = new DiscountPayNTakeM(voucher);
                    break;
                default:
                    strategy = null;
                    break;
            }

            //assign the right logic
            return new DiscountContext(strategy);
        }
    }
}
