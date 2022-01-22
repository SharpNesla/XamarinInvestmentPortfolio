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
        public Command<ItemTappedEventArgs> EditCommand { get; set; }
        private PortfolioService _porfolioService;
        public MainPageViewModel(PortfolioService portfolioService)
        {
            this._porfolioService = portfolioService;
            this.EditCommand = new Command<ItemTappedEventArgs>(x => this.OnEdit(x.Item as Portfolio));
            Init();
        }

        private void OnEdit(Portfolio portfolio)
        {
            //TODO change to navigationService
            var command = new NavigationCommand { PageType = typeof(PortfolioInfo) };
            command.Execute(portfolio);
        }

        public async void Init()
        {
            var portfolios = await this._porfolioService.Get();
            this.Portfolios = new List<Portfolio> {
                new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
                    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},
    new Portfolio() {Name = "Тест", Cost = 3000000, Currency = "RUB"},

            };
        }
    }
}