using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InvestmentPortfolio
{
    class NavigationCommand : BindableObject, ICommand, IMarkupExtension
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        async public void Execute(object parameter)
        {
            await Shell.Current.GoToAsync(parameter as string);
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return new NavigationCommand();
        }
    }
}
