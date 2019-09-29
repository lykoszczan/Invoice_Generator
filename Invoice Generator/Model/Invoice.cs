using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoice_Generator.Model
{
    public class InvoicesContext : DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<PositionDB> Positions { get; set; }
    }

    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }   
        public string SellerName { get; set; }
        public string SellerNip { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNip { get; set; }
        public string CustomerAdress { get; set; }
        public string AmountBrutto { get; set; }

        public virtual ICollection<PositionDB> Positions {get; set;}
    }

    public class PositionDB
    {
        [Key]
        public int PositionId { get; set; }        
        public int RowIndex { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public double PriceNetto { get; set; }
        public double PriceBrutto { get; set; }
        public double Quantity { get; set; }
        public double Vat { get; set; }
        public double AmountNetto { get; set; }
        public double AmountBrutto { get; set; }

        public virtual Invoice Invoices { get; set; }

    }

    public class Position
    {
        private int fRowIndex;
        private string fName;
        private double fPriceNetto;
        private double? fPriceBrutto;
        private double fAmountNetto;
        private double fQuantity;
        private double fVat;
        private string fUnit;
        private double fAmountBrutto;
        
        public int RowIndex
        {
            get { return fRowIndex; }
            set { fRowIndex = value; }
        }

        public string Name
        {
            get { return fName; }
            set { fName = value; }
        }

        public string Unit
        {
            get { return fUnit; }
            set { fUnit = value; }
        }

        public double Vat
        {
            get { return fVat; }
            set { fVat = value; }
        }

        public double PriceNetto
        {
            get { return fPriceNetto; }
            set { fPriceNetto = value; }
        }

        public double Quantity
        {
            get { return fQuantity; }
            set { fQuantity = value; }
        }

        public double? PriceBrutto
        {
            get { return fPriceBrutto; }
            set { fPriceBrutto = value; }
        }

        public double AmountNetto
        {
            get { return fAmountNetto; }
            set { fAmountNetto = value; }
        }

        public double AmountBrutto
        {
            get { return fAmountBrutto; }
            set { fAmountBrutto = value; }
        }

        public Position()
        {
            this.Vat = 23;            
        }

        public Position(Position position)
        {
            this.fRowIndex = position.RowIndex;
            this.fName = position.Name;
            this.fUnit = position.Unit;
            this.fPriceBrutto = position.PriceBrutto;
            this.fVat = position.Vat;
            this.fPriceNetto = position.fPriceNetto;
            this.fQuantity = position.Quantity;
            this.AmountNetto = position.AmountNetto;
            this.AmountBrutto = position.AmountBrutto;
        }

        public Position(string unit, string name, double vat, double price, double quantity, int count)
        {
            this.fRowIndex = count + 1;
            this.fName = name;
            this.fUnit = unit;
            this.fPriceBrutto = price;
            this.fVat = vat;
            this.fPriceNetto = this.fPriceBrutto.GetValueOrDefault() / (1 + this.fVat / 100);
            this.fPriceNetto = Math.Round(this.fPriceNetto, 2); 
            this.fQuantity = quantity;
            this.AmountNetto = this.fQuantity * this.fPriceNetto;
            this.AmountBrutto = this.fQuantity * this.fPriceBrutto.GetValueOrDefault();
        }

        public void Calculate(int count = 1)
        {
            this.fRowIndex = count + 1;
            this.fPriceNetto = this.fPriceBrutto.GetValueOrDefault() / (1 + this.fVat / 100);
            this.fPriceNetto = Math.Round(this.fPriceNetto, 2);            
            this.AmountNetto = Math.Round(this.fQuantity * this.fPriceNetto, 2);
            this.AmountBrutto = Math.Round(this.fQuantity * this.fPriceBrutto.GetValueOrDefault(),2);
        }

    }

    public class Company
    {
        private string fAdress;
        private string fName;
        private string fNip;
        private string fEmail;
        private string fPhone;


        public string Adress
        {
            get { return fAdress; }
            set { fAdress = value.Trim(); }
        }
        public string Name
        {
            get { return fName; }
            set { fName = value.Trim(); }
        }
        public string Nip
        {
            get { return fNip; }
            set { fNip = value.Trim(); }
        }
        public string Email
        {
            get { return fEmail; }
            set { fEmail = value; }
        }
        public string Phone
        {
            get { return fPhone; }
            set { fPhone = value; }
        }

        public Company()
        {
            
        }

        public Company(string adress, string name, string nip, string email, string phone)
        {
            this.fAdress = adress;
            this.fName = name;
            this.fNip = nip;
            this.fEmail = email;
            this.fPhone = phone;
        }
    }

}
