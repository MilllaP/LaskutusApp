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
    /// Interaction logic for AddBillWindow.xaml
    /// </summary>
    public partial class AddBillWindow : Window
    {
        public ObservableCollection<Bill> allbills;
        private Bill bill;
        private BillRepository repo;

        private bool newBill = true;                    //Uusi lasku, ei jo olemassa oleva BillID
        private bool billAdded = false;                 //Lasku lisätään ensin, sitten tuotteet
        public AddBillWindow()
        {
            InitializeComponent();
            repo = new BillRepository();
            bill = new Bill();
            allbills = repo.GetBills();

            comCustomers.ItemsSource = repo.GetCustomers();       
            comProducts.ItemsSource = repo.GetProducts();   
            productgrid.IsEnabled = false;
        }
        /// <summary>
        /// Laskun lisääminen ja päivittäminen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            bill = (Bill)this.DataContext;
            Customer customer = (Bill)comCustomers.SelectedValue;       //Kenelle lasku lisätään
            bool selected = comProducts.DisplayMemberPath != null;      //Jos tuote on valittu, true

            foreach (var item in allbills)
            {
                if (bill.BillID == item.BillID)                         //Laskunumero on jo käytössä
                {
                    newBill = false;
                    break;
                }
                else
                {
                    newBill = true;
                }
            }
            if (comCustomers.IsEnabled && bill.BillID == 0)             //Laskunumero ei saa olla 0
            {
                MessageBox.Show("Tarkista antamasi laskunumero");
            }
            else if (comCustomers.IsEnabled && newBill == false)
            {
                MessageBox.Show("Laskunumero on varattu");              //Kerrotaan, että laskunumero oli varattu
            }
            else if (customer == null)                                  //Asiakas tulee valita, ennen laskun luomista
            {
                MessageBox.Show("Valitse asiakas");
            }
            else if (bill != null && customer != null && newBill && billAdded == false)     //Laskunumero oli uusi ja asiakas oli valittu
            {
                bill.Id = customer.Id;
                repo.AddBill(bill);

                billAdded = true;
                comCustomers.IsEnabled = false;
                idbox.IsEnabled = false;
                productgrid.IsEnabled = true;
                CancelButton.IsEnabled = true;
                AddButton.Content = "Tallenna";
            }
            else if (billAdded && bill != null && selected)            //Lasku on lisätty tietokantaan ja tuote on valittu combobox:ssa (comProducts)
            {
                repo.UpdateBill(bill);
                foreach (InvoiceLine line in bill.InvoiceLines)
                {
                    if (line.Amount == 0)
                    {
                        repo.RemoveLine(line);                          //Jos tuotteen määrä muutettiin 0, tuoterivi poistetaan
                    }
                }
                allbills = repo.GetBills();
                foreach (var item in allbills)
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
        /// Estää kaikki muut merkit paitsi numerosarjat 0-9 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// Peruutus nappulalla poistetaan juuri luotu lasku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (bill != null)
            {
                var result = MessageBox.Show("Luotu lasku poistetaan", "Varoitus", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    repo.RemoveBill(bill);
                    Close();
                }
            }
        }
    }
}

