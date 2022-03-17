using InvestmentPortfolio.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InvestmentPortfolio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PortfolioAdd : ContentPage
    {
        public PortfolioAdd()
        {
            InitializeComponent();
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class PortfolioEditorViewModel : IOnNavigate<Portfolio>
    {
        public PortfolioService Service { get; }
        public Portfolio Portfolio { get; set; }
        public IEnumerable<string> Currencies { get; }
        public Command AddCommand { get; }
        public bool IsNew { get; private set; }
        public PortfolioEditorViewModel(PortfolioService porfolioService, FinancialService financialService)
        {
            this.Service = porfolioService;
            this.AddCommand = new Command(this.Add);
            this.Currencies = financialService.Currencies;
            this.Portfolio = new Portfolio();
        }

        public void OnNavigate(Portfolio param)
        {
            this.IsNew = false;
            this.Portfolio = param;
        }

        private async void Add()
        {
            if (IsNew)
            {
                await this.Service.Add(Portfolio);

            }
            else
            {
                await this.Service.Update(Portfolio);
            }


            //TODO change to navigationService
            await NavigationCommand.Back();
        }
    }
}