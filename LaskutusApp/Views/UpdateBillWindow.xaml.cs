using LaskutusApp.Models;
using LaskutusApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LaskutusApp
{
    /// <summary>
    /// Interaction logic for UpdateBillWindow.xaml
    /// </summary>
    public partial class UpdateBillWindow : Window
    {
        public ObservableCollection<Bill> allbills;
        public BillRepository repo;
        public UpdateBillWindow()
        {
            InitializeComponent();
            repo = new BillRepository();

            allbills = repo.GetBills();
            comProducts.ItemsSource = repo.GetProducts();
        }
        /// <summary>
        /// Hakee annetun laskutunnuksen ja vertaa sitä jo olemassa oleviin laskuihin.
        /// Painike muuttuu tallenna nappulaksi, jonka jälkeen laskuun voi tehdä muutoksia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Get_Click(object sender, RoutedEventArgs e)
        {
            var bill = (Bill)idbox.DataContext;

            if ((bill != null && idbox.IsEnabled) || bill.BillID == 0)              //Laskutunnus ei ole tyhjä tai 0
            {
                foreach (var item in allbills)
                {
                    if (item.BillID == bill.BillID)                                 //Laskutunnus on olemassa
                    {
                        this.DataContext = item;
                        idbox.IsEnabled = false;
                        datebox.IsEnabled = true;
                        duebox.IsEnabled = true;
                        productgrid.IsEnabled = true;
                        AddBill.Content = "Tallenna";
                        break;
                    }
                }
            }
            else if (idbox.IsEnabled == false)                                      //Laskutunnus on annettu ja hyväksytty
            {
                repo.UpdateBill(bill);              
                foreach (InvoiceLine line in bill.InvoiceLines)                     //Tarkistaa jos laskun tuotteen määärä on 0, tuoterivi poistetaan
                {
                    if (line.Amount == 0)
                    {
                        repo.RemoveLine(line);
                    }
                }
                allbills = repo.GetBills();
                foreach (var item in allbills)                                      //Ladataan päivitetty versio laskusta
                {
                    if (item.BillID == bill.BillID)
                    {
                        this.DataContext = item;
                    }
                }
                MessageBox.Show("Lasku tallennettu");
            }
        }
        /// <summary>
        /// Hyväksytään vain lukuja 0-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
