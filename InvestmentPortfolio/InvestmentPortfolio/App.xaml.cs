using InvestmentPortfolio.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InvestmentPortfolio
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            App.Current.MainPage = new MainPage();
            App.Current.MainPage.Navigation.PushAsync(new Main());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
