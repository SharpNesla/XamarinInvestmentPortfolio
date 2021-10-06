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
    public partial class PortfolioEditor : ContentPage
    {
        public PortfolioEditor()
        {
            InitializeComponent();
        }
    }
    [AddINotifyPropertyChangedInterface]
    public class PortfolioEditorViewModel
    {
        private PortfolioService _portfolioService;

        public Portfolio Portfolio { get; set; }
        public ICommand Add { get; }
        public PortfolioEditorViewModel(PortfolioService portfolio)
        {
            this._portfolioService = portfolio;
            this.Add = new Command(async () =>
            {
                await _portfolioService.Add(this.Portfolio);

                var navigateCommand = new NavigationCommand { NavigationPage = new MainPage()};
                navigateCommand.Execute(null);
            });
            Init();
        }
        
        public async void Init()
        {
            if (true)
            {
                Portfolio = new Portfolio();
            }
        }
    }
}