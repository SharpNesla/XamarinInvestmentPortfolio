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
    public partial class PortfolioAdd : ContentPage
    {
        public PortfolioAdd()
        {
            InitializeComponent();
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class PortfolioAddViewModel
    {
        public PortfolioService Service { get; }
        public Portfolio Portfolio { get; set; }
        public PortfolioAddViewModel(PortfolioService porfolioService)
        {
            Service = porfolioService;
            this.Init();
        }
        async void Init()
        {
            Portfolio = new Portfolio();
        }


    }
}