using LaskutusApp.Models;
using LaskutusApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for UpdateProductWindow.xaml
    /// </summary>
    public partial class UpdateProductWindow : Window
    {
        public BillRepository repo;
        private Product product;
        public UpdateProductWindow()
        {
            InitializeComponent();
            repo = new BillRepository();
            product = new Product();

            comProducts.ItemsSource = repo.GetProducts();                       //Hakee kaikki tuotteet tietokannasta
        }
        /// <summary>
        /// Valitsee tuotteen ja mahdollistaa sen päivittämisen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Select_Button(object sender, RoutedEventArgs e)
        {
            if (comProducts.SelectedValue == null)                              //Tuote tulee olla valittuna
            {
                MessageBox.Show("Valitse tuote");
            }
            else
            {
                comProducts.IsEnabled = false;                                  //Antaa muut valinnat käyttöön, kun tuote on valittu combobox:ssa (comProducts)
                SelectButton.IsEnabled = false;
                ProductRow.Visibility = Visibility.Visible;
                UpdateButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
                CancelButton.IsEnabled = true;

                product = (Product)comProducts.SelectedValue;
                this.DataContext = product;
            }
        }
        /// <summary>
        /// Päivitä tuotteen hintaa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Button(object sender, RoutedEventArgs e)
        {
            product = (Product)this.DataContext;
            if (product.Price > 0)                                          //Tuotteen hinta tulee olla enemmän kuin 0
            {
                repo.UpdateProduct(product);
                comProducts.ItemsSource = repo.GetProducts();

                ButtonControl();
            }
            else
            {
                MessageBox.Show("Tarkista summa");
            }
        }
        /// <summary>
        /// Valitun tuotteen poistaminen tietokannasta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Button(object sender, RoutedEventArgs e)
        {
            product = (Product)this.DataContext;
            if (product != null)
            {
                var result = MessageBox.Show("Haluatko varmasti poistaa tuotteen? HUOM! Tuote poistuu olemassa olevista laskuista.", "Varoitus", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    repo.RemoveProduct(product);
                }
            }
            comProducts.ItemsSource = repo.GetProducts();

            ButtonControl();
        }
        /// <summary>
        /// Peruuttaa, jos tuote on valittu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Button(object sender, RoutedEventArgs e)
        {
            comProducts.ItemsSource = repo.GetProducts();
            ButtonControl();
        }
        /// <summary>
        /// Näppäimien pois (IsEnabled) sulkeminen ja palauttaminen
        /// </summary>
        private void ButtonControl()
        {
            this.DataContext = null;
            comProducts.IsEnabled = true;
            SelectButton.IsEnabled = true;
            UpdateButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            CancelButton.IsEnabled = false;
        }
        /// <summary>
        /// Hyväksyy desimaaleja
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
