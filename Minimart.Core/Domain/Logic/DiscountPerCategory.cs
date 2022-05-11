﻿using Minimart.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minimart.Core.Domain.Logic
{
    public class DiscountPerCategory : BaseDiscount, IDiscountStrategy
    {
        public DiscountPerCategory(Voucher voucher) : base(voucher) {
            if (_voucher.Percent == null)
                throw new ArgumentNullException("Voucher.Percent must be defined");

        }

        public Cart Apply(Cart cart, DateTime? date)
        {
            var today = date ?? DateTime.Now;

            if (!this.isValidDate(today))
                return cart;

            if (!this.isValidaWeekDay(today))
                return cart;


            foreach (var item in cart.items)
            {
                //if this item apply for this voucher discount
                if (this.categories.Contains(item.CategoryId) && 
                    !this.excludedProducts.Contains(item.ProductId))
                {
                    var amount = (item.Price * item.Quantity);
                    var discount = ((_voucher.Percent ?? 0) / 100); //if Percent = 20, then discount is = 0.2
                    item.TotalWithDiscount = amount - (amount * discount);
                }
            }

            //calculate total of the cart with corresponding discount
            cart.TotalWithDiscount = cart.items.Sum(i => i.TotalWithDiscount);

            return cart;
        }
    }
}
