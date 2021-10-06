using InvestmentPortfolio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvestmentPortfolio
{
    class CompositionRoot
    {
        public PortfolioService Portfolio { get; } = new PortfolioService();
        public MainPageViewModel MainPageViewModel => new MainPageViewModel(Portfolio);
    }
}
