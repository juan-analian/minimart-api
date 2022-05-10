using Minimart.Core.Domain.Models;
using Minimart.Core.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Domain.Services.Communication
{
    public class CartResponse: BaseResponse<Cart>
    {
        public CartResponse(Cart cart) : base(cart) { }

        public CartResponse(string message) : base(message) { }
    }
}
