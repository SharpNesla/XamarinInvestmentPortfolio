using InvestmentPortfolio.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts;
using SkiaSharp;

namespace InvestmentPortfolio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PortfolioInfo : TabbedPage
    {
        public PortfolioInfo()
        {
            InitializeComponent();
        }
    }
    public enum OptimizeBy
    {
        MaxGain,
        MinRisk
    }
    public class OptimizeByConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var en = (OptimizeBy)value;
            switch (en)
            {
                case OptimizeBy.MaxGain:
                    return "Максимизация доходности";
                case OptimizeBy.MinRisk:
                default:
                    return "Минимизация риска";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string)value;

            switch (str)
            {
                case "Максимизация доходности":
                    return OptimizeBy.MaxGain;
                default:
                    return OptimizeBy.MinRisk;
            }
        }
    }

    [AddINotifyPropertyChangedInterface]
    class PortfolioInfoViewModel : IOnNavigate<Portfolio>
    {
        public Command<PortfolioPosition> EditCommand { get; set; }
        public Command<PortfolioPosition> ShowDialogCommand { get; set; }
        public DonutChart DonutChart { get; set; }
        public BarChart BarChart { get; set; }
        public OptimizeBy OptimizeBy { get; set; }
        public List<OptimizeBy> OptimizeItems { get; }
        private readonly PortfolioService _portfolioService;
        public Portfolio Portfolio { get; set; }
        public List<PortfolioPosition> Positions { get; set; }
        public Command AddPositionCommand { get; set; }
        public Command DeletePositionCommand { get;}
        public Command<PortfolioPosition> EditPositionCommand { get;  }

        public PortfolioInfoViewModel(PortfolioService portfolioService)
        {
            this.OptimizeItems = new List<OptimizeBy> { OptimizeBy.MaxGain, OptimizeBy.MinRisk };
            this.OptimizeBy = OptimizeBy.MaxGain;
            this._portfolioService = portfolioService;

            this.EditCommand = new Command<PortfolioPosition>(this.OnEdit);
            this.ShowDialogCommand = new Command<PortfolioPosition>(this.OnPopupShow);
            this.AddPositionCommand = new Command(this.OnAddPosition);
            this.DeletePositionCommand = new Command<PortfolioPosition>(this.OnDelete);
            this.EditPositionCommand = new Command<PortfolioPosition>(this.OnEdit);
            var typeface = SKTypeface.FromFamilyName("Roboto");

            this.DonutChart = new DonutChart()
            {
                LabelTextSize = 36,
            };
            this.BarChart = new BarChart
            {
                LabelTextSize = 36
            };
            
            this.Update();
        }

        private void OnAddPosition()
        {
            var command = new NavigationCommand { PageType = typeof(PositionEditor) };
            command.Execute(new PositionEditorParams { Item1 = Portfolio});
        }

        private void OnEdit(PortfolioPosition position)
        {
            //TODO change to navigationService
            var command = new NavigationCommand { PageType = typeof(PositionEditor) };
            command.Execute(new PositionEditorParams { Item1 = Portfolio, Item2 = position });
        }
        private async void OnDelete(PortfolioPosition position)
        {            
            this.Portfolio.Positions.Remove(position);
            this._portfolioService.Update(Portfolio);
        }
        private async void OnPopupShow(PortfolioPosition position)
        {
            if (position is null) return;

            var editAction = "Изменить";
            var deleteAction = "Удалить";

            //TODO Inject App class as dependency
            string action = await App.Current.MainPage.
                DisplayActionSheet($"{position}", null, null, editAction, deleteAction);

            if (action.Equals(editAction))
            {
                this.OnEdit(position);
            }
            if (action.Equals(deleteAction))
            {
                this.OnDelete(position);
            }
        }
        public async Task Update()
        {
            this.Portfolio = await this._portfolioService.GetById(this.Portfolio.PortfolioID);
            var Entries = new[]
{
                new ChartEntry(212)
                {
                    Label = "APL",
                    ValueLabel = "112",
                    Color = SKColor.Parse("#2c3e50")
                },
                new ChartEntry(248)
                {
                    Label = "MKB",
                    ValueLabel = "648",
                    Color = SKColor.Parse("#77d065")
                },
                new ChartEntry(128)
                {
                    Label = "iOS",
                    ValueLabel = "428",
                    Color = SKColor.Parse("#b455b6")
                },
                new ChartEntry(514)
                {
                    Label = "DBO",
                    ValueLabel = "214",
                    Color = SKColor.Parse("#3498db")
                }
            };
            this.BarChart.Entries = Entries;
            this.DonutChart.Entries = Entries;
        }
        public async void OnNavigate(Portfolio param)
        {
            this.Portfolio = param;
            await this.Update();
        }
    }
}