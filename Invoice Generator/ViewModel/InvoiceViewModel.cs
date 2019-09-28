using Invoice_Generator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Invoice_Generator.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string caller = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(caller));
            }            
        }
    }

    public class InvoiceViewModel : ViewModelBase
    {
        private Invoice InvoiceInfo = new Invoice();

        private ICommand saveInvoiceCommand;
        private ICommand addPositionCommand;
        private ICommand deletePositionCommand;
        private ICommand saveSettingsCommand;

        private Position fSelectedPosition;
        private Position fPosition;
        private Company fCustomer;

        private ObservableCollection<Position> fpositions;

        public ObservableCollection<Position> Positions
        {
            get { return this.fpositions; }
            set
            {
                this.fpositions = value;
                OnPropertyChanged();
            }
        }

        public Position SelectedPosition
        {
            get { return this.fSelectedPosition; }
            set
            {
                this.fSelectedPosition = value;
                OnPropertyChanged();
            }
        }

        public Company Customer
        {
            get { return this.fCustomer; }
            set
            {
                this.fCustomer = value;
                OnPropertyChanged();
            }
        }        

        public Position Position
        {
            get { return this.fPosition; }
            set
            {
                this.fPosition = value;
                OnPropertyChanged();
            }
        }

        public string PositionName
        {
            get { return this.fPosition.Name; }
            set
            {
                this.fPosition.Name = value;
                OnPropertyChanged();
            }
        }

        public double? PositionPrice
        {
            get { return this.fPosition.PriceBrutto; }
            set
            {
                this.fPosition.PriceBrutto = value;
                OnPropertyChanged();
            }
        }

        public double PositionQuantity
        {
            get { return this.fPosition.Quantity; }
            set
            {
                this.fPosition.Quantity = value;
                OnPropertyChanged();
            }
        }

        public string PositionUnit
        {
            get { return this.fPosition.Unit; }
            set
            {
                this.fPosition.Unit = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveInvoice
        {
            get
            {                
                if (saveInvoiceCommand == null) saveInvoiceCommand = new SaveInvoiceCommand(this);
                return saveInvoiceCommand;
            }
        }

        public ICommand AddPosition
        {
            get
            {
                if (addPositionCommand == null) addPositionCommand = new AddPositionCommand(this);
                return addPositionCommand;
            }
        }

        public ICommand DeletePosition
        {
            get
            {
                if (deletePositionCommand == null) deletePositionCommand = new DeletePositionCommand(this);
                return deletePositionCommand;
            }
        }

        public ICommand SaveSettings
        {
            get
            {
                if (saveSettingsCommand == null) saveSettingsCommand = new SaveSettingsCommand(this);
                return saveSettingsCommand;
            }
        }

        public InvoiceViewModel()
        {
            fpositions = new ObservableCollection<Position>();
            fCustomer = new Company();
            fPosition = new Position();
        }


    }
}
