using LaskutusApp.Models;
using LaskutusApp.Repositories;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for FindBillWindow.xaml
    /// </summary>
    public partial class FindBillWindow : Window
    {
        public BillRepository repo;
        public Bill? bill { get; set; }
        public FindBillWindow()
        {
            InitializeComponent();
            repo = new BillRepository();
            bill = new Bill();

            comLaskut.ItemsSource = repo.GetBills();
        }
        /// <summary>
        /// Tallentaa valitun laskunumeron, laskunumero haetaan MainWindow:ssa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FindButton(object sender, RoutedEventArgs e)
        {
            bill = (Bill)comLaskut.SelectedValue;
            if (bill != null)
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
            bill = null;
            Close();
        }
    }
}
