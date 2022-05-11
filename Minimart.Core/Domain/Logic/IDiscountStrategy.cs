using Minimart.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Domain.Logic
{
    public interface IDiscountStrategy
    {
        Cart Apply(Cart cart, DateTime? date);
    }
}
