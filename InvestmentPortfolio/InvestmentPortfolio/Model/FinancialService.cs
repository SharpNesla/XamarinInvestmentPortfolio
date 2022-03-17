using System;
using System.Collections.Generic;
using System.Text;

namespace InvestmentPortfolio.Model
{
    public class FinancialService
    {
        private readonly IEnumerable<string> CURRENCIES = new [] { "USD", "EUR", "GBP" , "RUB", "CNY" };
        public IEnumerable<string> Currencies => CURRENCIES;

    }
}
