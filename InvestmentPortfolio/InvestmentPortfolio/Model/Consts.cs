using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

        public const string YAHOO_API_CODER = "PASTE YOUR TOKEN HERE";
        public const string YAHOO_API_ROOT = "https://yfapi.net";
        public const string CBR_API_DAILY_URI = "https://www.cbr.ru/scripts/XML_daily.asp";
        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }

        public const string FINNHUB_TOKEN = "PASTE YOUR TOKEN HERE";
    }
}
