using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace LaskutusApp.Models
{
    public class Bill : Customer
    {
        public DateTime Date { get; set; }
        public int BillID { get; set; }
        public DateTime DueDate { get; set; }
        public string Information { get; set; }
        public float InTotal { get; set; }
        public ObservableCollection<InvoiceLine> InvoiceLines { get; set; }
        public Bill()
        {
            Date = DateTime.Now.Date;
            BillID = 0;
            DueDate = DateTime.Now.Date;
            Information = string.Empty;
            InTotal = 0;

            InvoiceLines = new ObservableCollection<InvoiceLine>();
        }
    }
}
