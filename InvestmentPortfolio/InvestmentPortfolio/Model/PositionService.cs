using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentPortfolio.Model
{
    class PositionService
    {
        SQLiteAsyncConnection Database;

        public PositionService(SQLiteAsyncConnection connection)
        {
            Database = connection;
            CreateTableResult result = Database.CreateTableAsync<PortfolioPosition>().Result;
        }
        public async Task<Portfolio> GetById(uint id)
        {
            var task = Database.Table<Portfolio>().Where(x => x.PortfolioID == id).FirstAsync();
            return await task;
        }
        public async Task<int> Add(PortfolioPosition position)
        {
            return await Database.InsertAsync(position, typeof(PortfolioPosition));
        }
        public async Task<int> Update(PortfolioPosition position)
        {
            return await Database.UpdateAsync(position, typeof(PortfolioPosition));
        }
        public async void Delete(PortfolioPosition position)
        {
            await Database.DeleteAsync(position);
        }
    }
}
