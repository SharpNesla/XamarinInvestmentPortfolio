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
        public PortfolioEditorViewModel PortfolioEditorViewModel => new PortfolioEditorViewModel(Portfolio);

        #endregion

        #region Services

        public PortfolioService Portfolio { get; } = new PortfolioService();
        
        #endregion
    }
}
