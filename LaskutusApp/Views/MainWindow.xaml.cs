using LaskutusApp.Models;
using LaskutusApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LaskutusApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Bill> allbills;         //Laskut haetaan tietokannasta laskutusdb
        public ObservableCollection<Product> allproducts;   //Kaikki tuotteet jotka ovat tietokannassa
        public ObservableCollection<Bill> allcustomers;     //Kaikki asiakkaat jotka ovat tietokannassa
        public BillRepository repo;
        private int index;
        public MainWindow()
        {
            repo = new BillRepository();
            InitializeComponent();

            if (allbills == null)                           //Huom! Tietokanta luodaan aina uudestaan, kun sovellus ajetaan uudestaan
            {
                repo.CreateDatabase();

                repo.CreateCustomerTable();
                repo.FillCustomersTable();

                repo.CreateProductTable();
                repo.FillProductTable();

                repo.CreateBillTable();
                repo.FillBillTable();

                repo.CreateInvoicelineTable();
                repo.FillInvoicelineTable();
            }

            allbills = repo.GetBills();                 //Hakee laskut tietokannasta
            this.DataContext = allbills[index];

            allproducts = repo.GetProducts();           //Hakee tuotteet tietokannasta
            ProductList.ItemsSource = allproducts;

            allcustomers = repo.GetCustomers();         //Hakee asiakkaat tietokannasta
            CustomerList.ItemsSource = allcustomers;
        }
        /// <summary>
        /// Siirtyy ensimmäiseen laskuun, joka on ObservableCollection<Bill> allbills:ssa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToFirstBill(object sender, RoutedEventArgs e)
        {
            index = 0;
            this.DataContext = allbills[index];
        }
        /// <summary>
        /// Siirtyy edelliseen laskuun, joka on ObservableCollection<Bill> allbills:ssa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousBill(object sender, RoutedEventArgs e)
        {
            if (index == 0)
            {
                index = 0;
            }
            else
            {
                index--;
            }
            this.DataContext = allbills[index];
        }
        /// <summary>
        /// Siirtyy seuraavaan laskuun, joka on ObservableCollection<Bill> allbills:ssa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextBill(object sender, RoutedEventArgs e)
        {

            if (index == allbills.Count - 1)
            {
                index = allbills.Count - 1;
            }
            else
            {
                index++;
            }
            this.DataContext = allbills[index];
        }
        /// <summary>
        /// Siirtyy viimeiseen laskuun, joka on ObservableCollection<Bill> allbills:ssa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToLastBill(object sender, RoutedEventArgs e)
        {
            index = allbills.Count - 1;
            this.DataContext = allbills[index];
        }
        /// <summary>
        /// Sovelluksen info-ikkuna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Info_Click(object sender, RoutedEventArgs e)
        {
            //var InfoView = new InfoWindow();
            //InfoView.ShowDialog();
            var testi = new testiwindow();
            testi.ShowDialog();
        }

        /// <summary>
        /// Laskun haku laskunumerolla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBill_Click(object sender, RoutedEventArgs e)
        {
            var SearchBillView = new FindBillWindow();
            SearchBillView.ShowDialog();

            if (SearchBillView.bill != null)                //Hakee valitun laskunumeron, ja hakee laskun sijainnin allbills listasta
            {
                this.DataContext = allbills.Where(x => x.BillID == SearchBillView.bill.BillID);
            }
        }
        /// <summary>
        /// Laskun poistaminen laskunumerolla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveBill_Click(object sender, RoutedEventArgs e)
        {
            var RemoveBillsView = new RemoveWindow();
            RemoveBillsView.ShowDialog();

            if (RemoveBillsView.bill != null && RemoveBillsView.bill.BillID != 0)
            {
                bool notfound = true;
                foreach (var item in allbills)
                {
                    if (item.BillID == RemoveBillsView.bill.BillID)
                    {
                        var result = MessageBox.Show("Haluatko varmasti poistaa laskun?", "Varoitus", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            repo.RemoveBill(RemoveBillsView.bill);
                            allbills = repo.GetBills();
                            this.DataContext = allbills[0];
                            notfound = false;
                        }
                    }
                }
                if (notfound)
                {
                    MessageBox.Show("Asiakasta ei löytynyt");
                }
            }
        }
        /// <summary>
        /// Uuden laskun lisääminen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBill_Click(object sender, RoutedEventArgs e)
        {
            var AddBillView = new AddBillWindow();
            AddBillView.ShowDialog();

            allbills = repo.GetBills();
            this.DataContext = allbills[0];
        }
        /// <summary>
        /// Laskun päivittäminen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateBill_Click(object sender, RoutedEventArgs e)
        {
            var UpdateBillView = new UpdateBillWindow();
            UpdateBillView.ShowDialog();

            allbills = repo.GetBills();
            this.DataContext = allbills[0];
        }
        /// <summary>
        /// Asiakkaan kaikkien laskujen hakeminen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetCustomer_Click(object sender, RoutedEventArgs e)
        {
            var SearchCustomerView = new FindCustomerWindow();
            SearchCustomerView.ShowDialog();
            if (SearchCustomerView.customer != null && SearchCustomerView.customer.Id != 0)               //Hakee valitun asiakkaan ja antaa asiakkaan tiedot laskuikkunalle (ShowBillWindow)
            {
                bool notfound = true;
                foreach (var item in allbills)
                {
                    if (item.Id == SearchCustomerView.customer.Id)
                    {
                        notfound = false;
                    }
                }
                if (notfound)
                {
                    MessageBox.Show("Asiakasta ei löytynyt");
                }
                else
                {
                    var SearchView = new ShowBillWindow(SearchCustomerView.customer);
                    SearchView.ShowDialog();
                }
            }
        }
        /// <summary>
        /// Tuotehintojen muuttaminen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateProducts_Click(object sender, RoutedEventArgs e)
        {
            var UpdateView = new UpdateProductWindow();
            UpdateView.ShowDialog();

            allbills = repo.GetBills();
            this.DataContext = allbills[0];

            allproducts = repo.GetProducts();
            ProductList.ItemsSource = allproducts;
        }
        /// <summary>
        /// Uuden asiakkaan lisääminen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var AddCustomerView = new AddCustomerWindow();
            AddCustomerView.ShowDialog();

            allcustomers = repo.GetCustomers();
            CustomerList.ItemsSource = allcustomers;
        }
        /// <summary>
        /// UUden tuotteen lisääminen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var AddProductView = new AddProductWindow();
            AddProductView.ShowDialog();

            allproducts = repo.GetProducts();
            ProductList.ItemsSource = allproducts;
        }
        /// <summary>
        /// Asiakastietojen muuttaminen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            var UpdateCustomerView = new UpdateCustomerWindow();
            UpdateCustomerView.ShowDialog();

            allbills = repo.GetBills();
            this.DataContext = allbills[0];

            allcustomers = repo.GetCustomers();
            CustomerList.ItemsSource = allcustomers;
        }
    }
}