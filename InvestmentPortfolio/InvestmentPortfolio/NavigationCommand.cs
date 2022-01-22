using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InvestmentPortfolio
{
    interface IOnNavigate<TParam>
    {
        void OnNavigate(TParam param);
    }
    [ContentProperty(nameof(PageType))]
    class NavigationCommand : ICommand, IMarkupExtension
    {
        public Type PageType { get; set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        async public void Execute(object parameter)
        {
            //TODO Handle incorrect page type
            var page = Activator.CreateInstance(PageType) as Page;
            if (parameter != null)
            {
                if (page.BindingContext is IOnNavigate<object>)
                {
                    var handler = page.BindingContext as IOnNavigate<object>;
                    handler.OnNavigate(parameter);
                }
            }
            await App.Current.MainPage.Navigation.PushAsync(page);
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
