using Invoice_Generator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Invoice_Generator.ViewModel
{
    public class AddPositionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private readonly InvoiceViewModel vm;

        public AddPositionCommand(InvoiceViewModel viewmodel)
        {
            this.vm = viewmodel ?? throw new ArgumentNullException("modelWidoku");
        }

        public bool CanExecute(object parameter)
        {
            if (string.IsNullOrEmpty(this.vm.PositionName) || string.IsNullOrEmpty(this.vm.PositionUnit)
                || this.vm.PositionPrice == null || this.vm.Position.Vat >= 100 || this.vm.Position.Vat < 0
                || this.vm.Position.Quantity <= 0)
                return false;
            else if (this.vm.PositionPrice <= 0)
                return false;
            else
                return true;
        }

        public void Execute(object parameter)
        {        
            if(this.vm.Positions.Count == 0)
                this.vm.Position.Calculate(0);
            else
                this.vm.Position.Calculate(this.vm.Positions.Last().RowIndex);
            Position pos = new Position(this.vm.Position);
            this.vm.Positions.Add(pos);
            this.vm.PositionName = string.Empty;
            this.vm.PositionUnit = string.Empty;
            this.vm.PositionPrice = null;            
        }

    }
}
