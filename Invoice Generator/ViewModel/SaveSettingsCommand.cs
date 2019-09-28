using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Invoice_Generator.ViewModel
{
    public class SaveSettingsCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly InvoiceViewModel vm;

        public SaveSettingsCommand(InvoiceViewModel viewmodel)
        {
            this.vm = viewmodel ?? throw new ArgumentNullException("modelWidoku");
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Properties.Settings.Default.Save();
        }
    }
}
