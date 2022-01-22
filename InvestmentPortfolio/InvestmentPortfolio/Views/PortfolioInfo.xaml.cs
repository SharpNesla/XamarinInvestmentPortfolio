using InvestmentPortfolio.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
    class PortfolioInfoViewModel
    {
        public OptimizeBy OptimizeBy { get; set; }
        public List<OptimizeBy> OptimizeItems { get; }
        private readonly PortfolioService _portfolioService;
        public Portfolio Portfolio { get; set; }
        public List<PortfolioPosition> Positions { get; set; }
        public PortfolioInfoViewModel(PortfolioService portfolioService)
        {
            this.OptimizeItems = new List<OptimizeBy> { OptimizeBy.MaxGain, OptimizeBy.MinRisk };
            this.OptimizeBy = OptimizeBy.MaxGain;
            this._portfolioService = portfolioService;
            this.Portfolio = new Portfolio()
            {
                Name = "Тест",
                Cost = 3000000,
                Currency = "RUB"

            };
            this.Positions = new List<PortfolioPosition>()
            {
                new PortfolioPosition{  },
                new PortfolioPosition{  },
new PortfolioPosition{  },
new PortfolioPosition{  },
new PortfolioPosition{  },
new PortfolioPosition{  },
new PortfolioPosition{  },
new PortfolioPosition{  },
new PortfolioPosition{  },
new PortfolioPosition{  },
new PortfolioPosition{  },
new PortfolioPosition{  },
new PortfolioPosition{  },
new PortfolioPosition{  },
new PortfolioPosition{  },
new PortfolioPosition{  },
new PortfolioPosition{  },
new PortfolioPosition{  },
new PortfolioPosition{  },

            };
        }
    }
}