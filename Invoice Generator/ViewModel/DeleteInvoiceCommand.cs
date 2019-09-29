using Invoice_Generator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Invoice_Generator.ViewModel
{
    public class DeleteInvoiceCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private readonly InvoiceViewModel vm;

        public DeleteInvoiceCommand(InvoiceViewModel viewmodel)
        {
            this.vm = viewmodel ?? throw new ArgumentNullException("modelWidoku");
        }

        public bool CanExecute(object parameter)
        {
            if (this.vm.SelectedInvoice == null)
                return false;
            else
                return true;
        }

        public void Execute(object parameter)
        {
            int index = this.vm.Invoices.IndexOf(this.vm.SelectedInvoice);
            if (index < 0)
                throw new Exception("nie wybrano elementu");
            else
            {
                using (var db = new InvoicesContext())
                {                    
                    var obj = db.Invoices.SingleOrDefault(x => x.InvoiceId == this.vm.SelectedInvoice.InvoiceId);
                    db.Positions.RemoveRange(obj.Positions);                    
                    db.Invoices.Attach(obj);
                    db.Invoices.Remove(obj);
                    db.SaveChanges();

                    this.vm.Invoices = new System.Collections.ObjectModel.ObservableCollection<Invoice>(db.Invoices);                    
                }
            }                
        }
    }
}
