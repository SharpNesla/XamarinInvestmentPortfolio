using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvestmentPortfolio.Model
{
    public class Portfolio
    {
        [PrimaryKey]
        [AutoIncrement]
        public uint PortfolioID { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public string Comment { get; set; }
    }
}