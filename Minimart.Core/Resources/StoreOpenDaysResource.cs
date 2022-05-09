using Minimart.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Resources
{
    public class StoreOpenDaysResource
    {
        public string WeekDay { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
