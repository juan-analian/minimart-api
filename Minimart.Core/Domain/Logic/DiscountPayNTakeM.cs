using Minimart.Core.Domain.Models;
using System;
using System.Linq;

namespace Minimart.Core.Domain.Logic
{
    public class DiscountPayNTakeM : BaseDiscount, IDiscountStrategy
    {
        public DiscountPayNTakeM(Voucher voucher) : base(voucher) {
            if (_voucher.TakeUnits == null)
                throw new ArgumentNullException("Voucher.TakeUnits must be defined");
            if (_voucher.PayUnits == null)
                throw new ArgumentNullException("Voucher.PayUnits must be defined");
            if (_voucher.UpToUnit == null)
                throw new ArgumentNullException("Voucher.UpToUnit must be defined");
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
                
                if (this.products.Contains(item.ProductId) && item.Quantity > _voucher.TakeUnits)
                {
                    //quantity of products discounted! 
                    //if we have 6 items and the promo is take 3 pay 2, then count = 2
                    //if we have 4 items and the promo is take 3 pay 2, then count = 1
                    var take = _voucher.TakeUnits;
                    var pay = _voucher.PayUnits;
                    var limit = _voucher.UpToUnit;

                    var outOfPromo = item.Quantity > limit ? item.Quantity - limit: 0;
                    var inPromo = item.Quantity - outOfPromo;

                    var packs = inPromo / take;

                    var subTotalInPromo = packs * (item.Price * pay);
                    var subTotalOutOfPromo = outOfPromo * item.Price;
 
                    item.TotalWithDiscount = (decimal)(subTotalInPromo + subTotalOutOfPromo); 
                }
            }

            //calculate total of the cart with corresponding discount
            cart.TotalWithDiscount = cart.items.Sum(i => i.TotalWithDiscount);
            return cart;
        }
    }
}
