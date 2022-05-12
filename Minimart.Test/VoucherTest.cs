using Minimart.Core.Domain.Logic;
using Minimart.Core.Domain.Models;
using Minimart.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Minimart.Test
{
    public class VoucherBusinessLogicTest
    {
        
        [Fact]
        public void Voucher_doesnot_apply_out_of_date()
        {
            //20% off on Wednesdays and Thursdays, on Cleaning products, from Jan 27th to Feb 13th
            var voucher = VoucherHelper.GetById("COCO1V1F8XOG1MZZ");
            var context = VoucherHelper.GetDiscountContext(voucher);

            // CategoryId = 2 (cleaning products)
            var cart = new Cart()
            {
                items = new List<CartItem>() {
                    new CartItem() { CategoryId = 2, ProductId = 10, Quantity = 5, Price = 1.50M }
            }};


            var finalCart = context.CalculateDiscount(cart, new DateTime(2022, 9, 10)); //out of range
            
            var TotalWithDiscount = finalCart.items.Sum(i => i.TotalWithDiscount);
            Assert.True(TotalWithDiscount == 0);             
        }

        [Fact]
        public void Voucher_doesnot_apply_different_weekdate()
        {
            //20% off on Wednesdays and Thursdays, on Cleaning products, from Jan 27th to Feb 13th
            var voucher = VoucherHelper.GetById("COCO1V1F8XOG1MZZ");
            var context = VoucherHelper.GetDiscountContext(voucher);

            // CategoryId = 2 (cleaning products)
            var cart = new Cart()
            {
                items = new List<CartItem>() {
                    new CartItem() { CategoryId = 2, ProductId = 10, Quantity = 5, Price = 1.50M }
            }};

            //test on wednesday, 30% off on Cleaning products from jan-27 to feb-13
            var finalCart = context.CalculateDiscount(cart, new DateTime(2022, 2, 6)); //simulate sunday

            var TotalWithDiscount = finalCart.items.Sum(i => i.TotalWithDiscount);
            Assert.True(TotalWithDiscount == 0);
        }

        [Fact]
        public void Voucher_COCO1V1F8XOG1MZZ_return_valid_totalWithDiscount()
        {
            //20% off on Wednesdays and Thursdays, on Cleaning products, from Jan 27th to Feb 13th
            var voucher = VoucherHelper.GetById("COCO1V1F8XOG1MZZ");
            var context = VoucherHelper.GetDiscountContext(voucher);

            // CategoryId = 2 (cleaning products)
            var cart = new Cart()
            {
                items = new List<CartItem>() {
                    new CartItem() { CategoryId = 2, ProductId = 10, Quantity = 5, Price = 1.50M }
            }};

             
            var finalCart = context.CalculateDiscount(cart, new DateTime(2022, 2, 2));

            var TotalWithDiscount = finalCart.items.Sum(i => i.TotalWithDiscount);
            Assert.True(TotalWithDiscount == 6.0M);
            
        }

        [Fact]
        public void Voucher_COCO1V1F8XOG1MZZ_doesnot_apply_by_category()
        {
            //20% off on Wednesdays and Thursdays, on Cleaning products, from Jan 27th to Feb 13th
            var voucher = VoucherHelper.GetById("COCO1V1F8XOG1MZZ");
            var context = VoucherHelper.GetDiscountContext(voucher);

            //CategoryId = 1 > sodas
            var cart = new Cart()
            {
                items = new List<CartItem>() {
                    new CartItem() { CategoryId = 1, ProductId = 10, Quantity = 5, Price = 1.50M }
            }
            };

                        
            var finalCart = context.CalculateDiscount(cart, new DateTime(2022, 2, 2)); //valid date

            var TotalWithDiscount = finalCart.items.Sum(i => i.TotalWithDiscount);
            Assert.True(TotalWithDiscount == 0);
        }

        [Fact]
        public void Voucher_COCOKCUD0Z9LUKBN_3x2_take_7_return_valid_amount()
        {
            var voucher = VoucherHelper.GetById("COCOKCUD0Z9LUKBN");
            var context = VoucherHelper.GetDiscountContext(voucher);

            var cart = new Cart() { items = new List<CartItem>() { 
                new CartItem() { ProductId = 12, Quantity = 7, Price = 20M } 
            }};

             
            //Pay 2 take 3 on "Windmill Cookies" (productId= 12) on up to 6 units, from Jan 24th to Feb 6th
            var finalCart = context.CalculateDiscount(cart, new DateTime(2022, 2, 2));

            //Assert
            var TotalWithDiscount = finalCart.items.Sum(i => i.TotalWithDiscount);
            Assert.True(TotalWithDiscount == 100);
        }

        [Fact]
        public void Voucher_COCOKCUD0Z9LUKBN_3x2_take_2_return_valid_amount()
        {
            var voucher = VoucherHelper.GetById("COCOKCUD0Z9LUKBN");
            var context = VoucherHelper.GetDiscountContext(voucher);

            var cart = new Cart()
            {
                items = new List<CartItem>() {
                new CartItem() { ProductId = 12, Quantity = 2, Price = 20M }
            }
            };


            //Pay 2 take 3 on "Windmill Cookies" (productId= 12) on up to 6 units, from Jan 24th to Feb 6th
            var finalCart = context.CalculateDiscount(cart, new DateTime(2022, 2, 2));

            //Assert
            var TotalWithDiscount = finalCart.items.Sum(i => i.TotalWithDiscount);
            Assert.True(TotalWithDiscount == 0);
        }


        [Fact]
        public void Voucher_COCO2O1USLC6QR22_discount_on_2nd_unit_return_valid_amount()
        {
            //30 % off on the second unit(of the same product), on "Nuka-Cola", "Slurm" and "Diet Slurm", for all February
            var voucher = VoucherHelper.GetById("COCO2O1USLC6QR22");
            var context = VoucherHelper.GetDiscountContext(voucher);

            //ProductId = 3 > "Nuka-Cola"
            var cart = new Cart()
            {
                items = new List<CartItem>() {
                new CartItem() { ProductId = 3, Quantity = 2, Price = 30M }
            }
            };
             
            var finalCart = context.CalculateDiscount(cart, new DateTime(2022, 2, 1));//valida date

            //Assert
            var TotalWithDiscount = finalCart.items.Sum(i => i.TotalWithDiscount);
            Assert.True(TotalWithDiscount == 51);
        }


        [Fact]
        public void Voucher_COCO2O1USLC6QR22_discount_on_2nd_unit_but_quantity_1()
        {
            //30 % off on the second unit(of the same product), on "Nuka-Cola", "Slurm" and "Diet Slurm", for all February
            var voucher = VoucherHelper.GetById("COCO2O1USLC6QR22");
            var context = VoucherHelper.GetDiscountContext(voucher);

            //ProductId = 3 > "Nuka-Cola"
            var cart = new Cart()
            {
                items = new List<CartItem>() {
                new CartItem() { ProductId = 3, Quantity = 1, Price = 30M }
            }
            };

            var finalCart = context.CalculateDiscount(cart, new DateTime(2022, 2, 1));//valida date

            //Assert
            var TotalWithDiscount = finalCart.items.Sum(i => i.TotalWithDiscount);
            Assert.True(TotalWithDiscount == 0);
            
        }

         

    }
}
