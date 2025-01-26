using LaskutusApp.Models;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for FindCustomerWindow.xaml
    /// </summary>
    public partial class FindCustomerWindow : Window
    {
        public Bill? customer { get; set; }
        public FindCustomerWindow()
        {
            InitializeComponent();
            customer = new Bill();
        }
        /// <summary>
        /// Tallentaa annetun asiakastunnuksen ja sulkee ikkunan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindButton(object sender, RoutedEventArgs e)
        {
            customer = this.DataContext as Bill;
            if (customer != null)
            {
                Close();
            }
        }
        /// <summary>
        /// Sulkee ikkunan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton(object sender, RoutedEventArgs e)
        {
            customer = null;
            Close();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
