using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InvestmentPortfolio.Model
{
    public static class Constants
    {
        public const string DatabaseFilename = "TodoSQLite.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
    }
    public class PortfolioService
    {
        SQLiteAsyncConnection Database;
        
        public PortfolioService()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            CreateTableResult result = Database.CreateTableAsync<Portfolio>().Result;
        }
        
        public async Task<List<Portfolio>> Get()
        {
            var task = Database.Table<Portfolio>().ToListAsync();
            return await task;
        }

        public async Task<int> Add(Portfolio portfolio)
        {
           return await Database.InsertAsync(portfolio, typeof(Portfolio));
        }
    }
}