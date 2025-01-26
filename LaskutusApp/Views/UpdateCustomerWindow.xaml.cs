using LaskutusApp.Models;
using LaskutusApp.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
using System.Windows.Shapes;

namespace LaskutusApp
{
    /// <summary>
    /// Interaction logic for UpdateCustomerWindow.xaml
    /// </summary>
    public partial class UpdateCustomerWindow : Window
    {
        private BillRepository repo;
        public Customer customer { get; set; }
        private int counter = 0;

        public UpdateCustomerWindow()
        {
            InitializeComponent();
            repo = new BillRepository();

            comCustomers.ItemsSource = repo.GetCustomers();             //Asiakkaiden hakeminen
            customer = (Bill)comCustomers.SelectedValue;
        }

        private void Pick_Click(object sender, RoutedEventArgs e)
        {
            customer = (Bill)comCustomers.SelectedValue;                //Asiakastiedot Combobox (comCustomers) valittu asiakas
            if (counter == 0 && customer != null)                       //Hyväksytään vain, jos customer ei ole tyhjä
            {
                comCustomers.IsEnabled = false;
                customerboxes.IsEnabled = true;
                Pick_Button.Content = "Päivitä";
                counter++;
                this.DataContext = customer;
            }
            else if (counter > 0 && customer != null)                   //Vahvistaa, että tieto on aikaisemmin annettu ja varmistaa käyttäjältä, että uudet tiedot halutaan päivittää
            {
                var result = MessageBox.Show("Haluatko varmasti päivittää tiedot?", "Varoitus", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    repo.UpdateCustomer(customer);
                    Close();
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
