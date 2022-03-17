using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLiteNetExtensions.Attributes;

namespace InvestmentPortfolio.Model
{
    public class Portfolio
    {
        [PrimaryKey]
        [AutoIncrement]
        public uint PortfolioID { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public decimal Cost { get; set; }
        public string Comment { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]      // One to many relationship with Valuation
        public List<PortfolioPosition> Positions { get; set; }

    }
}