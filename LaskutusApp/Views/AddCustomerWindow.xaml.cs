using LaskutusApp.Models;
using LaskutusApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        private ObservableCollection<Bill> allcustomers;
        private BillRepository repo;
        public AddCustomerWindow()
        {
            InitializeComponent();
            repo = new BillRepository();
            this.DataContext = new Customer();
            allcustomers = repo.GetCustomers();                     //Hakee olemassa olevat asiakkaat tietokannasta
        }
        /// <summary>
        /// Lisää asiakkaan tietokantaan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var customer = this.DataContext as Customer;
            bool newCustomer = true;                                //Asiakasta ei ole tietokannassa
                
            if (customer != null)
            {
                foreach (var item in allcustomers)
                {
                    if (customer.Id == item.Id)                     //Asiakas löytyi tietokannasta annetulla asiakasnumerolla
                    {
                        newCustomer = false;
                        break;
                    }
                    else
                    {
                        newCustomer = true;
                    }
                }
                if(customer.Id == 0)                                //Asiakasnumero ei voi olla 0
                {
                    MessageBox.Show("Tarkista antamasi laskunumero");
                }
                else if (newCustomer == false)                      //Kerrotaan, että asiakasnumero on varattu
                {
                    MessageBox.Show("Laskunumero on varattu");
                }
                else if (newCustomer)                               //Asiakasta ei löytynyt tietokannasta
                {
                    if (customer.Name == null || customer.StreetAddress == null || customer.PostalCode == null || customer.City == null)
                    {
                        MessageBox.Show("Tarkista tuotteen tiedot.");   //Asiakkaan kaikki tiedot tulee antaa
                    }
                    else
                    {
                        repo.AddCustomer(customer);
                        DialogResult = true;
                    }
                }
            }
        }
        /// <summary>
        /// Sallii numerot 0-9
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
