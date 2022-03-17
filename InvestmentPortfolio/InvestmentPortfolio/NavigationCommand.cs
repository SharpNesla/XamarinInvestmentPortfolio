using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InvestmentPortfolio
{
    interface IOnNavigate<TParam>
    {
        void OnNavigate(TParam param);
    }
    interface IOnNavigateBack
    {
        void OnNavigateBack();
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

        public async void Execute(object parameter)
        {
            //TODO Handle incorrect page type
            var page = Activator.CreateInstance(PageType) as Page;
            if (parameter != null)
            {
                var check = page.BindingContext.GetType().GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Where(i => i.GetGenericTypeDefinition() == typeof(IOnNavigate<>));

                if (check.Any() &&
                    check.First().GetGenericArguments()[0] == parameter.GetType())
                {
                    check.First().GetMethod(nameof(IOnNavigate<object>.OnNavigate))
                        .Invoke(page.BindingContext, new[] { parameter });
                }
            }
            await App.Current.MainPage.Navigation.PushAsync(page);
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public static async Task Back()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
            var page = Application.Current.MainPage.Navigation.NavigationStack.First();
            var ctx = page.BindingContext;
            if (ctx.GetType().GetInterfaces()
                    .Any(i => i == typeof(IOnNavigateBack)))
            {
                (ctx as IOnNavigateBack).OnNavigateBack();
            }
        }
    }
}
