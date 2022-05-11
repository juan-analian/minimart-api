using Minimart.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Domain.Logic
{
    public class DiscountContext
    {
        private IDiscountStrategy _strategy;

        public DiscountContext(IDiscountStrategy strategy) => this._strategy = strategy;

        public Cart CalculateDiscount(Cart cart, DateTime? dia)
        {
            if (this._strategy != null)
                return this._strategy.Apply (cart, dia);
            else
                throw new Exception("Undefined discountStrategy");
        }
    }
}
