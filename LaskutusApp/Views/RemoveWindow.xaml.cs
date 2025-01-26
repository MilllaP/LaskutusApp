using LaskutusApp.Models;
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
    /// Interaction logic for RemoveWindow.xaml
    /// </summary>
    public partial class RemoveWindow : Window
    {
        public Bill? bill { get; set; }                                 //Syöte tallennetaan tänne ja haetaan MainWindow:ssa
        public RemoveWindow()
        {
            InitializeComponent();
            bill = new Bill();
        }
        /// <summary>
        /// Annetun tiedon tallentaminen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveButton(object sender, RoutedEventArgs e)
        {
            bill = this.DataContext as Bill;                            //Haetaan annettu syöte
            if (bill != null)                                           //Hyväksytään, jos syöte ei ole tyhjä
            {
                Close();
            }
        }
        /// <summary>
        /// Ikkunan sulkeminen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Hyväksyy lukuja 0-9
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
