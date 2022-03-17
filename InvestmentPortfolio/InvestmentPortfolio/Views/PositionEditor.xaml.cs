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
    public partial class PositionEditor : ContentPage
    {
        public PositionEditor()
        {
            InitializeComponent();
        }
    }
    class PositionEditorParams
    {
        public Portfolio Item1 { get; set; }
        public PortfolioPosition Item2 { get; set; }
    }
    [AddINotifyPropertyChangedInterface]
    class PositionEditorViewModel : IOnNavigate<PositionEditorParams>
    {
        public PositionService Service { get; }
        public PortfolioPosition Position { get; set; }
        public Command AddCommand { get; }
        public bool IsNew { get; private set; }
        public PositionEditorViewModel(PositionService positionService)
        {
            this.Service = positionService;
            this.AddCommand = new Command(this.Add);
            this.Position = new PortfolioPosition();
            this.IsNew = true;
        }

        public void OnNavigate(PositionEditorParams parameters)
        {
            if (parameters.Item2 != null)
            {
                this.Position = parameters.Item2;
                this.IsNew = false;
            }
            this.Position.Portfolio = parameters.Item1;
            this.Position.PortfolioID = parameters.Item1.PortfolioID;
        }

        private async void Add()
        {
            await this.Service.Add(Position);
        }
    }
}