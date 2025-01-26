namespace LaskutusApp.Models
{
    public class InvoiceLine : Product
    {
        public int LineNumber { get; set; }
        public int Amount { get; set; }
        public float FullPrice { get; set; }
    }
}
