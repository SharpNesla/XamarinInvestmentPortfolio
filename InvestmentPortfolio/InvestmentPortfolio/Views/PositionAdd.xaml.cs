﻿using PropertyChanged;
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
    public partial class PositionAdd : ContentPage
    {
        public PositionAdd()
        {
            InitializeComponent();
        }
    }

    [AddINotifyPropertyChangedInterface]
    class PositionAddViewModel
    {
        
    }
}