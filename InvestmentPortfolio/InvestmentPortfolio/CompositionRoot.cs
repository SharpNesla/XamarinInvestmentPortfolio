using InvestmentPortfolio.Model;
using InvestmentPortfolio.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvestmentPortfolio
{
    class CompositionRoot
    {
        #region ViewModels

        public MainPageViewModel MainPageViewModel => new MainPageViewModel(Portfolio);
        public PortfolioEditorViewModel PortfolioAddViewModel => new PortfolioEditorViewModel(Portfolio, FinancialService);
        public PortfolioInfoViewModel PortfolioInfoViewModel => new PortfolioInfoViewModel(Portfolio);
        public PositionEditorViewModel PositionEditorViewModel => new PositionEditorViewModel(PositionService);
        #endregion

        #region Services

        public PortfolioService Portfolio { get; }
        public PositionService PositionService { get; }
        public FinancialService FinancialService { get; } = new FinancialService();

        #endregion

        public CompositionRoot()
        {
            var sqlite = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            this.Portfolio = new PortfolioService(sqlite);
            this.PositionService = new PositionService(sqlite);
        }
    }
}
