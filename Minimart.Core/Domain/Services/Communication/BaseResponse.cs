using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Domain.Services.Communication
{
    public abstract class BaseResponse<T>
    {
        public bool Success { get; protected set; }

        public string Message { get; protected set; }

        public T Resource { get; private set; }


        protected BaseResponse(string message)
        {
            Success = false;
            Message = message;
            Resource = default;
        }

        protected BaseResponse(T resource)
        {
            Success = true;
            Message = string.Empty;
            Resource = resource;
        }
    }
}
