using InvestmentPortfolio.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
    class MainPageViewModel : IOnNavigateBack
    {
        public List<Portfolio> Portfolios { get; private set; }
        public Command<Portfolio> ViewCommand { get; set; }
        public Command<Portfolio> ShowDialogCommand { get; set; }
        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                Update();
            }
        }


        private PortfolioService _porfolioService;
        public MainPageViewModel(PortfolioService portfolioService)
        {
            this._porfolioService = portfolioService;
            this.ViewCommand = new Command<Portfolio>(this.OnView);
            this.ShowDialogCommand = new Command<Portfolio>(this.OnPopupShow);

            this.Update();
            var current = Connectivity.NetworkAccess;

            switch (current)
            {
                case NetworkAccess.Internet:
                case NetworkAccess.ConstrainedInternet:
                    break;
                default:
                    App.Current.MainPage.DisplayAlert("Внимание", "Отсутствует подключение к сети интернет. " +
                        "Приложение будет работать в автономном режиме.", "Принять");
                    break;
            }
        }

        private void OnView(Portfolio portfolio)
        {
            //TODO change to navigationService
            var command = new NavigationCommand { PageType = typeof(PortfolioInfo) };
            command.Execute(portfolio);
        }
        private void OnEdit(Portfolio portfolio)
        {
            //TODO change to navigationService
            var command = new NavigationCommand { PageType = typeof(PortfolioEditor) };
            command.Execute(portfolio);
        }
        private void OnDelete(Portfolio portfolio)
        {
            this._porfolioService.Delete(portfolio);
            this.Update();
        }
        private async void OnPopupShow(Portfolio portfolio)
        {
            if (portfolio is null) return;

            var viewAction = "Просмотреть";
            var editAction = "Изменить";
            var deleteAction = "Удалить";

            //TODO Inject App class as dependency
            string action = await App.Current.MainPage.
                DisplayActionSheet($"{portfolio.Name}", null, null, viewAction, editAction, deleteAction);

            if (action.Equals(viewAction))
            {
                this.OnView(portfolio);
            }
            if (action.Equals(editAction))
            {
                this.OnEdit(portfolio);
            }
            if (action.Equals(deleteAction))
            {
                this.OnDelete(portfolio);
            }
        }
        public async void Update()
        {
            this.Portfolios = await this._porfolioService.Get(SearchText);
        }

        public void OnNavigateBack()
        {
            this.Update();
        }
    }
}