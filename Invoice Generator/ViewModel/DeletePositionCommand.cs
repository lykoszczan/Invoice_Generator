using Invoice_Generator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Invoice_Generator.ViewModel
{
    public class DeletePositionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private readonly InvoiceViewModel vm;

        public DeletePositionCommand(InvoiceViewModel viewmodel)
        {
            this.vm = viewmodel ?? throw new ArgumentNullException("modelWidoku");
        }


        public bool CanExecute(object parameter)    
        {
            if (this.vm.SelectedPosition == null)
                return false;
            else
                return true;
        }

        public void Execute(object parameter)
        {
            int index = this.vm.Positions.IndexOf(this.vm.SelectedPosition);
            if (index < 0)
                throw new Exception("nie wybrano elementu");
            else
            {
                this.vm.Positions.RemoveAt(index);
                foreach(Position item in this.vm.Positions)
                {
                    item.RowIndex = this.vm.Positions.IndexOf(item) + 1;
                }                
                this.vm.Positions.OrderBy(x => x.RowIndex);
                List<Position> temp = this.vm.Positions.ToList();
                this.vm.Positions = new System.Collections.ObjectModel.ObservableCollection<Position>(temp);
            }
        }
    }
}
