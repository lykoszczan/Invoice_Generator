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
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private readonly InvoiceViewModel vm;

        public SaveSettingsCommand(InvoiceViewModel viewmodel)
        {
            this.vm = viewmodel ?? throw new ArgumentNullException("modelWidoku");
        }

        public bool CanExecute(object parameter)
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.Name) ||
                string.IsNullOrEmpty(Properties.Settings.Default.NIP) ||
                string.IsNullOrEmpty(Properties.Settings.Default.Street) ||
                string.IsNullOrEmpty(Properties.Settings.Default.Number) ||
                string.IsNullOrEmpty(Properties.Settings.Default.PostalCode) ||
                string.IsNullOrEmpty(Properties.Settings.Default.City))
                return false;
            else
                return true;
        }

        public void Execute(object parameter)
        {
            Properties.Settings.Default.Save();
        }
    }
}
