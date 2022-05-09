using Dapper;
using Minimart.Core.Domain.Models;
using Minimart.Core.Domain.Repositories;
using Minimart.Core.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimart.Core.Persistence.Repositories
{
    public class StoreRepository: IStoreRepository
    {
        private readonly DapperContext _context;

        public StoreRepository(DapperContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Store>> GetStores(int? hour, byte? weekday)
        {
            var query = @"select s.Id, s.[Name], s.[address], o.Id, o.StoreId, o.[WeekDay], 
                        cast(o.[From] as datetime) [From], cast(o.[To] as datetime) [To] 
                        from Store s join StoreOpenDay o on s.Id = o.StoreId ";

            if (hour.HasValue && !weekday.HasValue)
                query += "where  datepart(HOUR,o.[From]) <= @hour and datepart(HOUR,o.[To]) > @hour ";

            if (!hour.HasValue && weekday.HasValue)
                query += "where  o.WeekDay = @weekday";

            if (hour.HasValue && weekday.HasValue)
                query += "where  datepart(HOUR,o.[From]) <= @hour and datepart(HOUR,o.[To]) > @hour and  o.WeekDay = @weekday";

            using (var connection = _context.CreateConnection())
            {

                var storeDict = new Dictionary<int, Store>();

                var stores = await connection.QueryAsync<Store, StoreOpenDay, Store>(
                    query, (store, storeOpenDays) =>
                    {
                        if (!storeDict.TryGetValue(store.Id, out var currentStore))
                        {
                            currentStore = store;
                            storeDict.Add(currentStore.Id, currentStore);
                        }
                        currentStore.OpenDays.Add(storeOpenDays);
                        return currentStore;
                    }, new { hour, weekday}
                );


                return stores.Distinct().ToList();
            }
        }

        public async Task<IEnumerable<Store>> GetStoresOnly()
        {
            var query = "SELECT * FROM Store";
            using (var connection = _context.CreateConnection())
            {
                var stores = await connection.QueryAsync<Store>(query);
                return stores.ToList();
            }
        }


        public async Task<IEnumerable<Store>> GetStores()
        {
            var query = @"select s.Id, s.[Name], s.[address], o.Id, o.StoreId, o.[WeekDay], 
                        cast(o.[From] as datetime) [From], cast(o.[To] as datetime) [To] 
                        from Store s join StoreOpenDay o on s.Id = o.StoreId ";
            using (var connection = _context.CreateConnection())
            {

                var storeDict = new Dictionary<int, Store>();

                var stores = await connection.QueryAsync<Store, StoreOpenDay, Store>(
                    query, (store, storeOpenDays) =>
                    {
                        if(!storeDict.TryGetValue(store.Id, out var currentStore))
                        {
                            currentStore = store;
                            storeDict.Add(currentStore.Id, currentStore);
                        }
                        currentStore.OpenDays.Add(storeOpenDays);
                        return currentStore;
                    }
                );


                return stores.Distinct().ToList();
            }
        }
    }
}
