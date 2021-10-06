using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PropertyChanged;
using InvestmentPortfolio.Model;

namespace InvestmentPortfolio
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
    }
    [AddINotifyPropertyChangedInterface]
    class MainPageViewModel
    {
        public List<Portfolio> Portfolios { get; private set; }
        private PortfolioService _porfolioService;
        public MainPageViewModel(PortfolioService portfolioService)
        {
            this._porfolioService = portfolioService;
            Init();
        }
        public async void Init()
        {
            var portfolios = await this._porfolioService.Get();
            this.Portfolios = portfolios;
        }
    }
}
