using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace InvestmentPortfolio
{
    class NavigationCommand : BindableObject, ICommand
    {
        public Page NavigationPage { get; set; }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        async public void Execute(object parameter)
        {
            if (NavigationPage != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(NavigationPage);
            }
        }
    }
}
