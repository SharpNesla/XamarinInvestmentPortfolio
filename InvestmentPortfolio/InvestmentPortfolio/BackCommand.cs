using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace InvestmentPortfolio
{
    class BackCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
