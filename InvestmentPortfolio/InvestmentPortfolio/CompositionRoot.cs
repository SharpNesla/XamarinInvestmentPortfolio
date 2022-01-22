using InvestmentPortfolio.Model;
using InvestmentPortfolio.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvestmentPortfolio
{
    class CompositionRoot
    {
        #region ViewModels

        public MainPageViewModel MainPageViewModel => new MainPageViewModel(Portfolio);
        public PortfolioAddViewModel PortfolioAddViewModel => new PortfolioAddViewModel(Portfolio);
        public PortfolioInfoViewModel PortfolioInfoViewModel => new PortfolioInfoViewModel(Portfolio);
        public PositionAddViewModel PositionAddViewModel => new PositionAddViewModel();
        #endregion

        #region Services

        public PortfolioService Portfolio { get; } = new PortfolioService();
        
        #endregion
    }
}
