using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Domain.Services.Communication
{
    public abstract class BaseResponse
    {
        public bool Success { get; protected set; }
        
        //if the message is set in a catch block, we can response with a 500 status code
        public bool UnhandledException { get; protected set; }
        public string Message { get; protected set; }

    }
}
