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
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public ObservableCollection<Product> allproducts;
        private BillRepository repo;
        private Product product;
        public AddProductWindow()
        {
            InitializeComponent();
            repo = new BillRepository();
            product = new Product();

            allproducts = repo.GetProducts();
            this.DataContext = product;
        }
        /// <summary>
        /// Lisää tuotteen tietokantaan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            product = (Product)this.DataContext;

            if (product.ProductID == 0)                             //Tuotenumeroksi ei kelpaa 0
            {
                MessageBox.Show("Tarkista tuotenumero.");
            }
            else
            {
                bool newProduct = true;                             //Onko tuotenumero jo olemassa tietokannassa
                foreach (var item in allproducts)
                {
                    if (item.ProductID == product.ProductID)
                    {
                        newProduct = false;
                    }
                }
                if (newProduct)                                     //Tuotenumero on uusi
                {
                    if (product.ProductName == null || product.Unit == null || product.Price <= 0)
                    {
                        MessageBox.Show("Tarkista tuotteen tiedot.");   //Tuotteen kaikki tiedot tulee antaa
                    }
                    else
                    {
                        var repo = new BillRepository();
                        repo.AddProduct(product);
                        this.DataContext = null;
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Tuotenumero on varattu.");    //Tuotenumero on varattu tietokannassa
                }
            }
        }
        /// <summary>
        /// Sallii vain numerot 0-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// Sallii vain desimaaliluvut
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FloatValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
