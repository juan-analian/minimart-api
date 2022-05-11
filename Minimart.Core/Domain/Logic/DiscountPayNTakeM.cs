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
                
                if (this.products.Contains(item.ProductId) && item.Quantity >= _voucher.TakeUnits)
                {
                     
                    //TAKE 3 pay 2 
                    var take = _voucher.TakeUnits;
                    //take 3 PAY 2
                    var pay = _voucher.PayUnits;
                    //take 3 pay 2 up to LIMIT products
                    var limit = _voucher.UpToUnit;

                    //if quantity = 100 and limit = 18 => outOfPromo = 82
                    var outOfPromo = item.Quantity > limit ? item.Quantity - limit: 0;
                    //if quantity = 100 and limit = 18 => inPromo = 18
                    var inPromo = item.Quantity - outOfPromo;

                    //groups
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
