using System;
using System.Collections.Generic;
using System.Text;

namespace Minimart.Core.Domain.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address{ get; set; }
        public IList<StoreOpenDay> OpenDays { get; set; } = new List<StoreOpenDay>();
    }
}
