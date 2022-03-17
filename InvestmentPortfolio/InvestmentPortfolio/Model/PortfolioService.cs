using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using SQLiteNetExtensionsAsync.Extensions;
using System.Threading.Tasks;

namespace InvestmentPortfolio.Model
{

    public class PortfolioService
    {
        SQLiteAsyncConnection Database;

        public PortfolioService(SQLiteAsyncConnection connection)
        {
            Database = connection;
            CreateTableResult result = Database.CreateTableAsync<Portfolio>().Result;
        }

        public async Task<List<Portfolio>> Get()
        {
            var task = Database.Table<Portfolio>().ToListAsync();
            return await task;
        }
        public async Task<Portfolio> GetById(uint id)
        {
            var task = Database.GetWithChildrenAsync<Portfolio>(id);
            return await task;
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