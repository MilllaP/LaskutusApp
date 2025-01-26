using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaskutusApp.Models
{
    public class Company
    {
        public string CompanyName { get; }
        public string ComStreetAddress { get; }
        public string ComPostalCode { get; }
        public string ComCity { get; }
        public Company()
        {
            CompanyName = "Rakennus Oy";
            ComStreetAddress = "Karjalankatu 3";
            ComCity = "JOENSUU";
            ComPostalCode = "80200";
        }
    }
}