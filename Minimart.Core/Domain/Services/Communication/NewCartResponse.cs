using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Domain.Services.Communication
{
    public class NewCartResponse : BaseResponse<Guid>
    {
        public NewCartResponse(Guid guid) : base(guid) { }

        public NewCartResponse(string message) : base(message) { }
    }
}
