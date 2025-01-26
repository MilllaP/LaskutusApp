using LaskutusApp.Models;
using LaskutusApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ShowBillWindow.xaml
    /// </summary>
    public partial class ShowBillWindow : Window
    {
        public ObservableCollection<Bill> allbills;             //Kaikki laskut, jotka ovat tietokannnassa
        public ObservableCollection<Bill> customersbills;       //Tietyn asiakkaan kaikki laskut
        public BillRepository repo;
        private int index;
        public ShowBillWindow(Bill customer)
        {
            InitializeComponent();

            repo = new BillRepository();
            customersbills = new ObservableCollection<Bill>();  //Talletetaan asiakkaan kaikki laskut, jotta niitä voidaan selata

            allbills = repo.GetBills();

            foreach (Bill item in allbills)                     //Valitaan laskut, jotka vastaavat annettua asiakastunnusta
            {
                if (item.Id == customer.Id) 
                {
                    customersbills.Add(item);
                }
            }
            this.DataContext = customersbills[index];           //Ikkunan tiedot ovat kaikki laskut, jotka ovat customersbills listassa
        }
        /// <summary>
        /// Siirtyy listan ensimmäiseen laskuun
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToFirstBill(object sender, RoutedEventArgs e)
        {
            index = 0;
            this.DataContext = customersbills[index];
        }
        /// <summary>
        /// Siirtyy listassa edelliseen laskuun
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
            this.DataContext = customersbills[index];
        }
        /// <summary>
        /// Siirtyy listassa seuraavaan laskuun
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextBill(object sender, RoutedEventArgs e)
        {

            if (index == customersbills.Count - 1)
            {
                index = customersbills.Count - 1;
            }
            else
            {
                index++;
            }
            this.DataContext = customersbills[index];
        }
        /// <summary>
        /// Siirtyy listan viimeiseen laskuun
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToLastBill(object sender, RoutedEventArgs e)
        {
            index = customersbills.Count - 1;
            this.DataContext = customersbills[index];
        }
    }
}
