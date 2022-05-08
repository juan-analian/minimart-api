using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Domain.Models
{
    public class StoreOpenDay
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public EWeekDays WeekDay { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

    }
}
