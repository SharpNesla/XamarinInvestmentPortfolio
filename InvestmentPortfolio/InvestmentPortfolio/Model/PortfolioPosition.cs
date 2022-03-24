using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using SQLiteNetExtensions.Attributes;

namespace InvestmentPortfolio.Model
{
    public class PortfolioPosition
    {
        [PrimaryKey]
        [AutoIncrement]
        public uint PositionID { get; set; }
        public uint PortfolioID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string Comment { get; set; }
        [Ignore]
        public decimal OptimizedCount { get; set; }
        [Ignore]
        public decimal Value { get; set; }
        public decimal USDPrice { get; set; }
        [Ignore]
        public decimal OptimizedValue { get; set; }
        public decimal OptimizedUSDValue { get; set; }
        [Ignore]
        public decimal Ratio { get; set; }
        [Ignore]
        public decimal OptimizedRatio { get; set; }
        [ManyToOne]
        public Portfolio Portfolio { get; set; }
    }
}
