using Minimart.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minimart.Core.Domain.Logic
{
    public abstract class BaseDiscount
    {
        internal Voucher _voucher;

        internal DateTime validFromDate;
        internal DateTime validToDate;

        internal List<int> products;
        internal List<int> categories;
        internal List<int> excludedProducts;

        public BaseDiscount(Voucher voucher)
        {
            this._voucher = voucher;
            this.SetValidDateRange();
            this.LoadCollections();
        }

        private void LoadCollections()
        {
            products = this._voucher.IncludedProducts.Select(p => p.ProductId).ToList();
            categories = this._voucher.IncludedCategories.Select(c => c.CategoryId).ToList();
            excludedProducts = this._voucher.ExcludedProducts.Select(p => p.ProductId).ToList();
        }

        private void SetValidDateRange()
        {           
            int fromDay = _voucher.ValidFromDay ?? 1;
            int fromMonth = _voucher.ValidFromMonth ??  1;
            int fromYear = _voucher.ValidFromYear ?? 1;

            validFromDate = new DateTime(fromYear, fromMonth, fromDay);

            int toMonth = _voucher.ValidToMonth ?? 12;
            int toYear = _voucher.ValidToYear ?? 9999;
            int toDay = _voucher.ValidToDay ?? (new DateTime(toYear, toMonth, 1).AddMonths(1).AddDays(-1)).Day;

            validToDate = new DateTime(toYear, toMonth, toDay);
        }

        internal bool isValidDate(DateTime date)
        {
            return (validFromDate <= date && validToDate >= date);
        }

        internal bool isValidaWeekDay(DateTime date)
        {
            //.Net start with: Sunday = 0 to Saturday = 6
            //We are storing in our SQL: Monday as 1 to Sunday as 7
            var weekDay = date.DayOfWeek == 0 ? 7 : (int)date.DayOfWeek;

            return _voucher.WeekDays.Any(w => (int)w.WeekDay == weekDay);            
        }


    }
}
