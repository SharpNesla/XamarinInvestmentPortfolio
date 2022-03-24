using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLiteNetExtensionsAsync.Extensions;
using System.Threading.Tasks;

namespace InvestmentPortfolio.Model
{

    public class PortfolioService
    {
        SQLiteAsyncConnection Database;

        private readonly FinancialService finances;

        public PortfolioService(SQLiteAsyncConnection connection, FinancialService service)
        {
            Database = connection;
            CreateTableResult result = Database.CreateTableAsync<Portfolio>().Result;
            this.finances = service;
        }

        public async Task<List<Portfolio>> Get(string searchText = "")
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                return await Database
                    .Table<Portfolio>()
                    .Where(x =>
                        x.Name
                        .Contains(searchText)
                        )
                    .ToListAsync();
            }
            return await Database.Table<Portfolio>().ToListAsync();
        }
        public async Task<Portfolio> GetById(uint id)
        {
            var task = Database.GetWithChildrenAsync<Portfolio>(id);
            var tasksync = await task;


            foreach (var item in tasksync.Positions)
            {
                //TODO: rewrite currencies with dict
                var currency = this.finances.Currencies1.Where(x => x.CharCode.Equals(tasksync.Currency)).First();
                var usd = this.finances.Currencies1.Where(x => x.CharCode.Equals("USD")).First();
                item.USDPrice = await this.finances.GetPositionPrice(item);
                var itemLocalPrice = item.USDPrice * usd.Value / currency.Value; 
                item.Value = itemLocalPrice * currency.Value * item.Count;
            }

            tasksync.Cost = tasksync.Positions.Select(x => x.Value).Sum();
            foreach (var item in tasksync.Positions)
            {
                if (tasksync.Cost != 0)
                {
                    item.Ratio = Math.Round(item.Value / tasksync.Cost * 10000) / 100;
                }
            }
            return tasksync;
        }
        public async Task<int> Add(Portfolio portfolio)
        {
            return await Database.InsertAsync(portfolio, typeof(Portfolio));
        }
        public async Task Update(Portfolio portfolio)
        {
            await Database.UpdateWithChildrenAsync(portfolio);
        }
        public async void Delete(Portfolio portfolio)
        {
            await Database.DeleteAsync(portfolio);
        }
    }
}