using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Resources
{
    public class StoreResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<StoreOpenDaysResource> OpenDays { get; set; } = new List<StoreOpenDaysResource>(); 
    }
}
