using InvestmentPortfolio.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InvestmentPortfolio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : ContentPage
    {
        public Main()
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