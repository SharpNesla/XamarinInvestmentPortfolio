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
using System.Linq;
namespace InvestmentPortfolio.Views
{
    /// <summary>
    /// Provides a range of tasteful random pastel colors
    /// </summary>
    public class RandomPastelColorGenerator
    {
        private readonly Random _random;

        public RandomPastelColorGenerator()
        {
            // seed the generator with 2 because
            // this gives a good sequence of colors
            const int RandomSeed = 384;
            _random = new Random(RandomSeed);
        }

        /// <summary>
        /// Returns a random pastel brush
        /// </summary>
        /// <returns></returns>
        public SolidColorBrush GetNextBrush()
        {
            SolidColorBrush brush = new SolidColorBrush(GetNext());

            return brush;
        }

        /// <summary>
        /// Returns a random pastel color
        /// </summary>
        /// <returns></returns>
        public Color GetNext()
        {
            // to create lighter colours:
            // take a random integer between 0 & 128 (rather than between 0 and 255)
            // and then add 127 to make the colour lighter
            byte[] colorBytes = new byte[3];
            colorBytes[0] = (byte)(_random.Next(128) + 127);
            colorBytes[1] = (byte)(_random.Next(128) + 127);
            colorBytes[2] = (byte)(_random.Next(128) + 127);

            Color color = new Color(
                (double)colorBytes[0] / 255,
                (double)colorBytes[1] / 255,
                (double)colorBytes[2] / 255);

            return color;
        }
    }

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
    class PortfolioInfoViewModel : IOnNavigate<Portfolio>, IOnNavigateBack
    {
        public Command<PortfolioPosition> EditCommand { get; set; }
        public Command<PortfolioPosition> ShowDialogCommand { get; set; }
        public DonutChart DonutChart { get; set; }
        public OptimizeBy OptimizeBy { get; set; }
        public List<OptimizeBy> OptimizeItems { get; }
        private readonly PortfolioService _portfolioService;
        private readonly OptimizerService _optimizerService;

        public Portfolio Portfolio { get; set; }
        public List<PortfolioPosition> Positions { get; set; }
        public Command AddPositionCommand { get; set; }
        public Command DeletePositionCommand { get; }
        public Command<PortfolioPosition> EditPositionCommand { get; }
        public Command EditPortfolioCommand { get; }
        public Command OptimizeCommand { get; }
        private bool _isOptimalChart;

        public bool IsOptimalChart
        {
            get { return _isOptimalChart; }
            set { _isOptimalChart = value; }
        }

        public PortfolioInfoViewModel(PortfolioService portfolioService, OptimizerService optimizer)
        {
            this.OptimizeItems = new List<OptimizeBy> { OptimizeBy.MaxGain, OptimizeBy.MinRisk };
            this.OptimizeBy = OptimizeBy.MaxGain;
            this._portfolioService = portfolioService;
            this._optimizerService = optimizer;


            this.EditCommand = new Command<PortfolioPosition>(this.OnEdit);
            this.ShowDialogCommand = new Command<PortfolioPosition>(this.OnPopupShow);
            this.AddPositionCommand = new Command(this.OnAddPosition);
            this.DeletePositionCommand = new Command<PortfolioPosition>(this.OnDelete);
            this.EditPositionCommand = new Command<PortfolioPosition>(this.OnEdit);
            this.OptimizeCommand = new Command(this.OnOptimize);
            this.EditPortfolioCommand = new Command(this.OnEditPortfolio);
            var typeface = SKTypeface.FromFamilyName("Roboto");

            this.DonutChart = new DonutChart()
            {
                LabelTextSize = 36,
            };

            this.Update();
        }

        private void OnEditPortfolio()
        {
            var command = new NavigationCommand { PageType = typeof(PortfolioEditor) };
            command.Execute(Portfolio);
        }

        private async void OnOptimize()
        {
            var tuple = await this._optimizerService.Optimize(Portfolio);
            double[] portf;
            if (OptimizeBy == OptimizeBy.MaxGain)
            {
                portf = tuple.Item2;
            }
            else
            {
                portf = tuple.Item1;
            }
            for (int i = 0; i < portf.Length; i++)
            {
                Portfolio.Positions[i].OptimizedRatio = (decimal) portf[i];
            }
        }

        private void OnAddPosition()
        {
            var command = new NavigationCommand { PageType = typeof(PositionEditor) };
            command.Execute(new PositionEditorParams { Item1 = Portfolio });
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
            await this._portfolioService.Update(Portfolio);
        }
        private async void OnPopupShow(PortfolioPosition position)
        {
            if (position is null) return;

            var editAction = "Изменить";
            var deleteAction = "Удалить";

            //TODO Inject App class as dependency
            string action = await App.Current.MainPage.
                DisplayActionSheet($"{position.Name}", null, null, editAction, deleteAction);

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
            var randomPastelGenerator = new RandomPastelColorGenerator();
            var entries = new List<ChartEntry>(Portfolio.Positions.Count);
            foreach (var x in Portfolio.Positions)
            {
                var color = SKColor.Parse(randomPastelGenerator.GetNext().ToHex());
                entries.Add(
                    new ChartEntry((float)x.Ratio)
                    {
                        Label = x.Name,
                        ValueLabel = $"{x.Ratio}%",
                        ValueLabelColor = color,
                        Color = color
                    }
                    );
            }
            this.DonutChart.Entries = entries;
        }
        public async void OnNavigate(Portfolio param)
        {
            this.Portfolio = param;
            await this.Update();
        }
        public async void OnNavigateBack()
        {
            await this.Update();
            await this._portfolioService.Update(Portfolio);
        }
    }
}