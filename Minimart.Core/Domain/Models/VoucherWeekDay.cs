using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Domain.Models
{
    public class VoucherWeekDay
    {
        public string VoucherId { get; set; }
        public EWeekDays WeekDay { get; set; } 
    }
}
