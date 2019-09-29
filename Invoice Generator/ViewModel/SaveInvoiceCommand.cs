using Invoice_Generator.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xceed.Words.NET;

namespace Invoice_Generator.ViewModel
{
    public class SaveInvoiceCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private readonly InvoiceViewModel vm;

        public SaveInvoiceCommand(InvoiceViewModel viewmodel)
        {
            this.vm = viewmodel ?? throw new ArgumentNullException("modelWidoku");
        }

        public bool CanExecute(object parameter)
        {
            if (string.IsNullOrEmpty(this.vm.Customer.Name) || string.IsNullOrEmpty(this.vm.Customer.Adress) 
                || string.IsNullOrEmpty(this.vm.Customer.Nip) || this.vm.Positions.Count <= 0)
                return false;
            else
                return true;
        }
        public void Execute(object parameter)
        {
            string invoiceName;
            int maxId;
            using (var db = new InvoicesContext())
            {                
                if (!db.Invoices.Any())
                {
                    maxId = 0;
                }
                else
                    maxId = db.Invoices.Where(u => u.DateTime.Year == DateTime.Now.Year).OrderByDescending(u => u.InvoiceId).FirstOrDefault().InvoiceId;            
                invoiceName = $"FV_{++maxId}_{DateTime.Now.Year}";
            }

            string fileName = string.Concat(AppDomain.CurrentDomain.BaseDirectory, invoiceName, ".docx");

            var doc = DocX.Create(fileName);
            
            string docDate = DateTime.Now.ToString("dd/MM/yyyy").ToString();
            int paymentDays = 14;
            string paymentType = "przelew";

            doc.InsertParagraph($"Faktura nr {invoiceName.Replace("_", "/")}").Alignment = Alignment.right;
            doc.InsertParagraph($"Data wystawienia {docDate}").Alignment = Alignment.right;
            doc.InsertParagraph($"Termin płatności: {paymentDays.ToString()} dni").Alignment = Alignment.right;
            doc.InsertParagraph($"Metoda płatności: {paymentType}").Alignment = Alignment.right;

            doc.InsertParagraph("Sprzedawca:").Bold();
            doc.InsertParagraph(Properties.Settings.Default.Name);
            doc.InsertParagraph($"{Properties.Settings.Default.Street} {Properties.Settings.Default.Number}");
            doc.InsertParagraph($"{Properties.Settings.Default.PostalCode} {Properties.Settings.Default.City}");
            doc.InsertParagraph($"NIP: {Properties.Settings.Default.NIP}");
            doc.InsertParagraph($"Nr rachunku do wpłat: {Properties.Settings.Default.BankAccount}");

            doc.InsertParagraph("Nabywca:").Bold().Alignment = Alignment.right;                  
            doc.InsertParagraph(this.vm.Customer.Name).Alignment = Alignment.right;
            doc.InsertParagraph(this.vm.Customer.Adress).Alignment = Alignment.right;
            doc.InsertParagraph($"NIP {this.vm.Customer.Nip}").Alignment = Alignment.right;

            Table posTable = doc.AddTable(this.vm.Positions.Count + 1, 8);
            posTable.Rows[0].Cells[0].Paragraphs.First().Append("Lp.");
            posTable.Rows[0].Cells[1].Paragraphs.First().Append("Nazwa");
            posTable.Rows[0].Cells[2].Paragraphs.First().Append("Jedn.");
            posTable.Rows[0].Cells[3].Paragraphs.First().Append("Ilość");
            posTable.Rows[0].Cells[4].Paragraphs.First().Append("Cena netto");
            posTable.Rows[0].Cells[5].Paragraphs.First().Append("Stawka VAT");
            posTable.Rows[0].Cells[6].Paragraphs.First().Append("Wartość netto");
            posTable.Rows[0].Cells[7].Paragraphs.First().Append("Wartość brutto");
            
            foreach(Position item in this.vm.Positions)
            {
                int lp = this.vm.Positions.IndexOf(item) + 1;
                posTable.Rows[lp].Cells[0].Paragraphs.First().Append(lp.ToString());
                posTable.Rows[lp].Cells[1].Paragraphs.First().Append(item.Name);
                posTable.Rows[lp].Cells[2].Paragraphs.First().Append(item.Unit);
                posTable.Rows[lp].Cells[3].Paragraphs.First().Append(item.Quantity.ToString());
                posTable.Rows[lp].Cells[4].Paragraphs.First().Append(item.PriceNetto.ToString());
                posTable.Rows[lp].Cells[5].Paragraphs.First().Append($"{item.Vat.ToString()}%");
                posTable.Rows[lp].Cells[6].Paragraphs.First().Append(item.AmountNetto.ToString());
                posTable.Rows[lp].Cells[7].Paragraphs.First().Append(item.AmountBrutto.ToString());                
            }

            doc.InsertTable(posTable);
            
            doc.InsertParagraph("Zapłacono: 0.00 PLN").Alignment = Alignment.right;

            double totalAmount = this.vm.Positions.Sum(item => item.AmountBrutto);

            doc.InsertParagraph($"Do zapłaty: {totalAmount.ToString()} PLN").Alignment = Alignment.right;
            doc.InsertParagraph($"Razem: {totalAmount.ToString()} PLN").Alignment = Alignment.right;
            doc.InsertParagraph("Uwagi: W tytule przelewu proszę podać nr zamówienia.");

            doc.Save();

            Process.Start("WINWORD.EXE", "\"" + fileName + "\"");

            using (var db = new InvoicesContext())
            {                
                var invoice = new Invoice
                {
                    Name = invoiceName,    
                    DateTime = DateTime.Now,
                    CustomerName = this.vm.Customer.Name,
                    CustomerAdress = this.vm.Customer.Adress,
                    CustomerNip = this.vm.Customer.Nip,
                    AmountBrutto = totalAmount.ToString()
                };                

                db.Invoices.Add(invoice);

                foreach(Position item in this.vm.Positions)
                {
                    PositionDB posDB = new PositionDB
                    {
                        RowIndex = item.RowIndex,
                        Name = item.Name,
                        Unit = item.Unit,
                        PriceNetto = item.PriceNetto,
                        PriceBrutto = item.PriceBrutto.GetValueOrDefault(),
                        Quantity = item.Quantity,
                        Vat = item.Vat,
                        AmountNetto = item.AmountNetto,
                        AmountBrutto = item.AmountBrutto,
                        Invoices = invoice
                    };

                    db.Positions.Add(posDB);

                }
                db.SaveChanges();

                this.vm.Invoices = new System.Collections.ObjectModel.ObservableCollection<Invoice>(db.Invoices);
            }

            this.vm.Positions.Clear();
        }
    }
}
