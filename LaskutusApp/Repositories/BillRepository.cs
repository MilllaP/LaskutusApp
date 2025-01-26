using LaskutusApp.Models;
using Microsoft.VisualBasic;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Shapes;


namespace LaskutusApp.Repositories
{
    /// <summary>
    /// Tietokannan luominen ja tietokannasta tiedon haku
    /// </summary>
    public class BillRepository
    {

        private const string local = @"Server=127.0.0.1; Port=3306; User ID=<username>; Pwd=<password>;";                                // Tietokannan yhteys; lisää tähän oma tietokantasi käyttäjätunnus ja salasana 
        private const string localWithDb = @"Server=127.0.0.1; Port=3306; User ID=<username>; Pwd=<password>; Database=LaskutusDb;";     // Tietokannan yhteys; lisää tähän oma tietokantasi käyttäjätunnus ja salasana 

        /// <summary>
        /// HUOM! Luo tietokannan, poistaa edellisen jos saman niminen on jo olemassa.
        /// </summary>
        public void CreateDatabase()
        {
            using (MySqlConnection conn = new MySqlConnection(local))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("DROP DATABASE IF EXISTS LaskutusDb", conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand("CREATE DATABASE LaskutusDb", conn);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Asiakas taulun luonti
        /// </summary>
        public void CreateCustomerTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                string createTable = "CREATE TABLE Customers (numberID INT(3) NOT NULL, " +
                                                            "fullname VARCHAR(30) NOT NULL, " +
                                                            "streetaddress VARCHAR(30) NOT NULL, " +
                                                            "postalcode VARCHAR(10) NOT NULL, " +
                                                            "city VARCHAR(30) NOT NULL, " +
                                                            "PRIMARY KEY (numberID));";
                MySqlCommand cmd = new MySqlCommand(createTable, conn);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Tuote taulun luonti
        /// </summary>
        public void CreateProductTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                string createTable = "CREATE TABLE Products (productid INT(5) NOT NULL, " +
                                                            "productname VARCHAR(30) NOT NULL, " +
                                                            "unit VARCHAR(10) NOT NULL, " +
                                                            "price FLOAT NOT NULL, " +
                                                            "PRIMARY KEY (productid))";
                MySqlCommand cmd = new MySqlCommand(createTable, conn);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Lasku taulun luonti
        /// </summary>
        public void CreateBillTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                string createTable = "CREATE TABLE Bills (billid INT NOT NULL, " +
                                                            "billdate DATE NOT NULL, " +
                                                            "duedate DATE NOT NULL, " +
                                                            "information VARCHAR(40) NOT NULL, " +
                                                            "numberID INT NOT NULL, " +
                                                            "PRIMARY KEY (billid), " +
                                                            "FOREIGN KEY (numberID) REFERENCES Customers(numberID))";
                MySqlCommand cmd = new MySqlCommand(createTable, conn);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Laskurivin luonti
        /// </summary>
        public void CreateInvoicelineTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                string createTable = "CREATE TABLE Invoicelines (linenumber INT NOT NULL AUTO_INCREMENT, " +
                                                            "amount INT NOT NULL, " +
                                                            "billid INT NOT NULL, " +
                                                            "productid INT(5) NOT NULL, " +
                                                            "PRIMARY KEY (linenumber, billid, productid), " +
                                                            "FOREIGN KEY (billid) REFERENCES Bills(billid), " +
                                                            "FOREIGN KEY (productid) REFERENCES Products(productid))";
                MySqlCommand cmd = new MySqlCommand(createTable, conn);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Asiakastietojen lisääminen tauluun
        /// </summary>
        public void FillCustomersTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string cust1 = "INSERT INTO Customers(numberID, fullname, streetaddress, postalcode, city) VALUES(777, 'Mari Mäkinen', 'Lönnrotinkatu 2', '00180', 'Helsinki')";
                string cust2 = "INSERT INTO Customers(numberID, fullname, streetaddress, postalcode, city) VALUES(998, 'Kai Saari', 'Lounaantie 4', '33800', 'Tampere')";
                string cust3 = "INSERT INTO Customers(numberID, fullname, streetaddress, postalcode, city) VALUES(445, 'Leila Laakso', 'Kirkkopuistikko 16', '65100', 'Vaasa')";
                string cust4 = "INSERT INTO Customers(numberID, fullname, streetaddress, postalcode, city) VALUES(665, 'Paavo Eloranta', 'Kasvihuoneenkatu 8', '28130', 'Pori')";
                string cust5 = "INSERT INTO Customers(numberID, fullname, streetaddress, postalcode, city) VALUES(122, 'Erkki Keskinen', 'Koulukatu 32', '90100', 'Oulu')";

                MySqlCommand cmd = new MySqlCommand(cust1, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(cust2, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(cust3, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(cust4, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(cust5, conn);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Tuotetietojen lisääminen tauluun
        /// </summary>
        public void FillProductTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string prod1 = "INSERT INTO Products(productid, productname, unit, price) VALUES(10,'Työ','h', 60)";
                string prod2 = "INSERT INTO Products(productid, productname, unit, price) VALUES(11,'Silikoni','kpl', 5.95)";
                string prod3 = "INSERT INTO Products(productid, productname, unit, price) VALUES(12,'Tiiviste','kpl', 1)";
                string prod4 = "INSERT INTO Products(productid, productname, unit, price) VALUES(13,'Kattopaneeli','m', 1.95)";
                string prod5 = "INSERT INTO Products(productid, productname, unit, price) VALUES(14,'Peitelista','m', 4.45)";
                string prod6 = "INSERT INTO Products(productid, productname, unit, price) VALUES(15,'Laudelauta','m', 9.30)";
                string prod7 = "INSERT INTO Products(productid, productname, unit, price) VALUES(16,'Saunapaneeli','pkt', 89.15)";
                string prod8 = "INSERT INTO Products(productid, productname, unit, price) VALUES(17,'Sokkelilista','kpl', 25.80)";
                string prod9 = "INSERT INTO Products(productid, productname, unit, price) VALUES(18,'Sisustuslevy','m2', 30.90)";
                string prod10 = "INSERT INTO Products(productid, productname, unit, price) VALUES(19,'Kiinnityslaasti','kpl', 42.95)";

                MySqlCommand cmd = new MySqlCommand(prod1, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(prod2, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(prod3, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(prod4, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(prod5, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(prod6, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(prod7, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(prod8, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(prod9, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(prod10, conn);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Laskun tietojen lisääminen tauluun
        /// </summary>
        public void FillBillTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string bill1 = "INSERT INTO Bills(billid, billdate, duedate, information, numberID) VALUES(123, '2023-01-01', '2023-02-01', 'Pesuhuoneremontti', 777)";
                string bill2 = "INSERT INTO Bills(billid, billdate, duedate, information, numberID) VALUES(456, '2022-12-15', '2023-01-15', 'Kylpyhuoneen saneeraus', 998)";
                string bill3 = "INSERT INTO Bills(billid, billdate, duedate, information, numberID) VALUES(789, '2023-02-12', '2023-03-15', 'Kodinhoitohuoneen remontti', 777)";
                string bill4 = "INSERT INTO Bills(billid, billdate, duedate, information, numberID) VALUES(147, '2023-02-28', '2023-03-28', 'Pesuhuoneremontti', 445)";
                string bill5 = "INSERT INTO Bills(billid, billdate, duedate, information, numberID) VALUES(258, '2022-11-28', '2022-12-28', 'Kylpyhuoneen vesieristys', 122)";
                string bill6 = "INSERT INTO Bills(billid, billdate, duedate, information, numberID) VALUES(369, '2023-01-01', '2023-02-01', 'Pesuhuoneremontti', 665)";
                string bill7 = "INSERT INTO Bills(billid, billdate, duedate, information, numberID) VALUES(987, '2022-12-15', '2023-01-15', 'Saunaremontti', 998)";
                string bill8 = "INSERT INTO Bills(billid, billdate, duedate, information, numberID) VALUES(654, '2023-02-12', '2023-03-15', 'Kodinhoitohuoneen saneeraus', 445)";
                string bill9 = "INSERT INTO Bills(billid, billdate, duedate, information, numberID) VALUES(321, '2023-02-28', '2023-03-28', 'Kylpyhuonekalusteiden vaihto', 122)";
                string bill10 = "INSERT INTO Bills(billid, billdate, duedate, information, numberID) VALUES(159, '2022-11-28', '2022-12-28', 'Kylpyhuoneen laajennus', 122)";

                MySqlCommand cmd = new MySqlCommand(bill1, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(bill2, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(bill3, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(bill4, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(bill5, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(bill6, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(bill7, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(bill8, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(bill9, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(bill10, conn);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Laskurivien lisääminen tauluun
        /// </summary>
        public void FillInvoicelineTable()
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                string invoice1 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(5, 123, 10)";
                string invoice2 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(1, 123, 11)";
                string invoice3 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(2, 123, 12)";
                string invoice4 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(80, 123, 13)";

                string invoice5 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(8, 456, 10)";
                string invoice6 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(3, 456, 11)";
                string invoice7 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(4, 456, 12)";

                string invoice8 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(8, 789, 10)";
                string invoice9 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(50, 789, 13)";
                string invoice10 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(8, 789, 14)";
                string invoice11 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(4, 789, 18)";

                string invoice12 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(12, 147, 10)";
                string invoice13 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(3, 147, 11)";
                string invoice14 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(5, 147, 12)";
                string invoice15 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(60, 147, 13)";

                string invoice16 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(5, 258, 10)";
                string invoice17 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(6, 258, 11)";
                string invoice18 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(4, 258, 12)";

                string invoice19 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(20, 369, 10)";
                string invoice20 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(6, 369, 11)";
                string invoice21 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(8, 369, 12)";
                string invoice22 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(90, 369, 13)";
                string invoice23 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(10, 369, 18)";
                string invoice24 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(8, 369, 14)";

                string invoice25 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(26, 987, 10)";
                string invoice26 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(100, 987, 15)";
                string invoice27 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(3, 987, 16)";
                string invoice28 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(80, 987, 13)";

                string invoice29 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(6, 654, 10)";
                string invoice30 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(2, 654, 11)";
                string invoice31 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(2, 654, 12)";
                string invoice32 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(5, 654, 14)";

                string invoice33 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(8, 321, 10)";
                string invoice34 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(2, 321, 11)";
                string invoice35 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(8, 321, 14)";
                string invoice36 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(2, 321, 19)";

                string invoice37 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(8, 159, 10)";
                string invoice38 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(5, 159, 17)";
                string invoice39 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(3, 159, 19)";
                string invoice40 = "INSERT INTO Invoicelines(amount, billid, productid) VALUES(5, 159, 13)";

                MySqlCommand cmd = new MySqlCommand(invoice1, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice2, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice3, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice4, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice5, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice6, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice7, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice8, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice9, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice10, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice11, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice12, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice13, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice14, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice15, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice16, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice17, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice18, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice19, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice20, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice21, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice22, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice23, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice24, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice25, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice26, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice27, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice28, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice29, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice30, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice31, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice32, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice33, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice34, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice35, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice36, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice37, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice38, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice39, conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(invoice40, conn);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Poistaa tietyn laskun annetun BillID:n avulla
        /// </summary>
        /// <param name="bill"></param>
        public void RemoveBill(Bill bill)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE " +
                                                    "FROM invoicelines " +
                                                    "WHERE billid=@billid ", conn);
                cmd.Parameters.AddWithValue("@billid", bill.BillID);
                cmd.ExecuteNonQuery();

                MySqlCommand cmd2 = new MySqlCommand("DELETE " +
                                                    "FROM bills " +
                                                    "WHERE billid=@billid ", conn);
                cmd2.Parameters.AddWithValue("@billid", bill.BillID);
                cmd2.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Poistaa laskurivin laskulta, jossa tuotteen määrä on 0
        /// </summary>
        /// <param name="invoice"></param>
        public void RemoveLine(InvoiceLine invoice)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE " +
                                                    "FROM invoicelines " +
                                                    "WHERE amount=@amount ", conn);
                cmd.Parameters.AddWithValue("@amount", invoice.Amount);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Poistaa tuotteen valikoimasta
        /// </summary>
        /// <param name="product"></param>
        public void RemoveProduct(Product product)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE " +
                                                    "FROM invoicelines " +
                                                    "WHERE productid=@productid ", conn);
                cmd.Parameters.AddWithValue("@productid", product.ProductID);
                cmd.ExecuteNonQuery();

                MySqlCommand cmd2 = new MySqlCommand("DELETE " +
                                                    "FROM products " +
                                                    "WHERE productid=@productid ", conn);
                cmd2.Parameters.AddWithValue("@productid", product.ProductID);
                cmd2.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Hakee laskulle tiedot ja täyttää ne ikkunaan
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Bill> GetBills()
        {
            var bills = new ObservableCollection<Bill>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT bills.billid, bills.billdate, bills.duedate, bills.information, customers.numberid, customers.fullname, customers.streetaddress, customers.postalcode, customers.city, IFNULL(SUM(ROUND(invoicelines.amount*products.price, 2)), 0) AS intotal " +
                                                    "FROM bills " +
                                                    "LEFT JOIN customers ON customers.numberID = bills.numberID " +
                                                    "LEFT JOIN invoicelines ON invoicelines.billid = bills.billid " +
                                                    "LEFT JOIN products ON products.productid = invoicelines.productid " +
                                                    "GROUP BY billid; ", conn);


                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var newBill = new Bill
                    {
                        BillID = dr.GetInt32("billid"),
                        Date = dr.GetDateTime("billdate"),
                        DueDate = dr.GetDateTime("duedate"),
                        Information = dr.GetString("information"),
                        Id = dr.GetInt32("numberid"),
                        Name = dr.GetString("fullname"),
                        StreetAddress = dr.GetString("streetaddress"),
                        PostalCode = dr.GetString("postalcode"),
                        City = dr.GetString("city"),
                        InTotal = dr.GetFloat("intotal"),     // IFNULL ja LEFT JOIN mahdollistaa laskun luomisen, vaikka laskulla ei olisi yhtään tuotetta 
                    };
                    GetLines(newBill);
                    bills.Add(newBill);
                }
            }
            return bills;
        }
        /// <summary>
        /// Täyttää tuotteet luokkaan ja ikkunan taulukkoon
        /// </summary>
        /// <param name="newProduct"></param>
        public void GetLines(Bill newProduct)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT invoicelines.linenumber, invoicelines.productid, products.productname, invoicelines.billid, invoicelines.amount, products.unit, products.price, ROUND(invoicelines.amount*products.price, 2) 'fullprice' " +
                                                    "FROM invoicelines " +
                                                    "INNER JOIN products ON products.productid = invoicelines.productid " +
                                                    "WHERE billid=@productid; ", conn);
                cmd.Parameters.AddWithValue("@productid", newProduct.BillID);

                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    newProduct.InvoiceLines.Add(new InvoiceLine
                    {
                        LineNumber = dr.GetInt32("linenumber"),
                        ProductID = dr.GetInt32("productid"),
                        ProductName = dr.GetString("productname"),
                        Amount = dr.GetInt32("amount"),
                        Unit = dr.GetString("unit"),
                        Price = dr.GetFloat("price"),
                        FullPrice = dr.GetFloat("fullprice"),
                    });
                }
            }
        }
        /// <summary>
        /// Palauttaa kaikki asiakkaat 
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Bill> GetCustomers()
        {
            var customers = new ObservableCollection<Bill>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT customers.numberid, customers.fullname, customers.streetaddress, customers.postalcode, customers.city " +
                                                    "FROM customers " +
                                                    "GROUP BY customers.fullname; ", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var newBill = new Bill
                    {
                        Id = dr.GetInt32("numberID"),
                        Name = dr.GetString("fullname"),
                        StreetAddress = dr.GetString("streetaddress"),
                        PostalCode = dr.GetString("postalcode"),
                        City = dr.GetString("city"),
                    };
                    customers.Add(newBill);
                }
            }
            return customers;
        }
        /// <summary>
        /// Palauttaa kaikki tuotteet
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Product> GetProducts()
        {
            var products = new ObservableCollection<Product>();

            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT productid, productname, unit, price " +
                                                    "FROM products; ", conn);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var newProduct = new Product
                    {
                        ProductID = dr.GetInt32("productid"),
                        ProductName = dr.GetString("productname"),
                        Unit = dr.GetString("unit"),
                        Price = dr.GetFloat("price"),
                    };
                    //newProduct.Productlist.Add(newProduct);
                    products.Add(newProduct);
                }
            }
            return products;
        }
        /// <summary>
        /// Lisää uuden laskun tietokantaan
        /// </summary>
        /// <param name="bill"></param>
        public void AddBill(Bill bill)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO Bills(billid, billdate, duedate, information, numberID) VALUES(@billid, @billdate, @duedate, @information, @numberID)", conn);
                cmd.Parameters.AddWithValue("@billid", bill.BillID);
                cmd.Parameters.AddWithValue("@billdate", bill.Date);
                cmd.Parameters.AddWithValue("@duedate", bill.DueDate);
                cmd.Parameters.AddWithValue("@information", bill.Information);
                cmd.Parameters.AddWithValue("@numberID", bill.Id);

                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Lisää uuden tuoterivin laskulle, jos tuoterivi on jo olemassa rivi päivitetään
        /// </summary>
        /// <param name="bill"></param>
        public void AddLines(Bill bill)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                foreach (var line in bill.InvoiceLines)
                {
                    if (line.ProductID == 0 && line.Amount != 0 || line.ProductID == 0 && line.Amount == 0)
                    {
                        //Ettei ohjelma kaadu, jos tuoteriville annettiin määrä, mutta ei tuotetta
                    }
                    else if (line.LineNumber == 0)
                    {
                        MySqlCommand cmdIns = new MySqlCommand("INSERT INTO invoicelines (amount, billid, productid) VALUES(@amount, @billid, @productid)", conn);
                        cmdIns.Parameters.AddWithValue("@billid", bill.BillID);
                        cmdIns.Parameters.AddWithValue("@amount", line.Amount);
                        cmdIns.Parameters.AddWithValue("@productid", line.ProductID);
                        cmdIns.ExecuteNonQuery();
                    }
                    else
                    {
                        MySqlCommand cmdUpd = new MySqlCommand("UPDATE invoicelines SET amount=@amount WHERE linenumber=@linenumber", conn);
                        cmdUpd.Parameters.AddWithValue("@amount", line.Amount);
                        cmdUpd.Parameters.AddWithValue("@linenumber", line.LineNumber);
                        cmdUpd.ExecuteNonQuery();
                    }
                }
            }
        }
        /// <summary>
        /// Lisää uuden asiakkaan tietokantaan
        /// </summary>
        /// <param name="customer"></param>
        public void AddCustomer(Customer customer)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO Customers(numberid, fullname, streetaddress, postalcode, city) VALUES(@numberid, @fullname, @streetaddress, @postalcode, @city)", conn);
                cmd.Parameters.AddWithValue("@numberid", customer.Id);
                cmd.Parameters.AddWithValue("@fullname", customer.Name);
                cmd.Parameters.AddWithValue("@streetaddress", customer.StreetAddress);
                cmd.Parameters.AddWithValue("@postalcode", customer.PostalCode);
                cmd.Parameters.AddWithValue("@city", customer.City);

                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Lisää uuden tuotteen tietokantaan
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Product product)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO Products(productid, productname, unit, price) VALUES(@productid, @productname, @unit, @price)", conn);
                cmd.Parameters.AddWithValue("@productid", product.ProductID);
                cmd.Parameters.AddWithValue("@productname", product.ProductName);
                cmd.Parameters.AddWithValue("@unit", product.Unit);
                cmd.Parameters.AddWithValue("@price", product.Price);

                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Päivittää tuotteen hintaa
        /// </summary>
        /// <param name="product"></param>
        public void UpdateProduct(Product product)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE Products SET price=@price WHERE productid=@productid", conn);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@productid", product.ProductID);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Päivittää asiakkaan tietoja
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomer(Customer customer)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE Customers SET fullname=@fullname, streetaddress=@streetaddress, postalcode=@postalcode, city=@city WHERE numberID=@numberID ", conn);
                cmd.Parameters.AddWithValue("@numberID", customer.Id);
                cmd.Parameters.AddWithValue("@fullname", customer.Name);
                cmd.Parameters.AddWithValue("@streetaddress", customer.StreetAddress);
                cmd.Parameters.AddWithValue("@postalcode", customer.PostalCode);
                cmd.Parameters.AddWithValue("@city", customer.City);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Päivittää laskun tietoja ja kutsuu AddLines metodia laskurivien lisäämiseen/päivittämiseen
        /// </summary>
        /// <param name="bill"></param>
        public void UpdateBill(Bill bill)
        {
            using (MySqlConnection conn = new MySqlConnection(localWithDb))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE Bills SET billdate=@billdate, duedate=@duedate, information=@information WHERE billid=@billid ", conn);
                cmd.Parameters.AddWithValue("@billid", bill.BillID);
                cmd.Parameters.AddWithValue("@billdate", bill.Date);
                cmd.Parameters.AddWithValue("@duedate", bill.DueDate);
                cmd.Parameters.AddWithValue("@information", bill.Information);
                cmd.ExecuteNonQuery();
            }
            AddLines(bill);
        }
    }
}
