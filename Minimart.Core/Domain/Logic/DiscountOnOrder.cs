using Minimart.Core.Domain.Models;
using System;
using System.Linq;

namespace Minimart.Core.Domain.Logic
{

    public class DiscountOnOrder : BaseDiscount, IDiscountStrategy
    {
        public DiscountOnOrder(Voucher voucher) : base(voucher) {

            if (_voucher.Percent == null)
                throw new ArgumentNullException("Voucher.Percent must be defined");

            if (_voucher.UnitOrder == null)
                throw new ArgumentNullException("Voucher.UnitOrder must be defined");
             
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
                if (this.products.Contains(item.ProductId) && item.Quantity >= _voucher.UnitOrder.Value)
                {
                    var amount = (item.Price * item.Quantity);
                    var discount = ((_voucher.Percent ?? 0) / (decimal)100); //if Percent = 20, then discount is = 0.2
                    item.TotalWithDiscount = amount - (item.Price * discount); //discount on the Nth. unit (1 unit)
                }
            }

            //calculate total of the cart with corresponding discount
            cart.TotalWithDiscount = cart.items.Sum(i => i.TotalWithDiscount);

            return cart;
        }
    }
}
