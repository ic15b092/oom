using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace Task3
{
    public enum Currency { EUR, GBP, USD, YEN }

    class Program
    {
        static void Main(string[] args)
        {
            Kamera X = new Kamera("Sony STC300IR", 3.0, 800m, Currency.EUR);
            Kamera Z = new Kamera("Geovision BL1300", 1.3, 670m, Currency.EUR);
            Console.WriteLine("Die Bezeichnung der X Kamera ist {0}.", X.GetBezeichnung);
            Console.WriteLine("Die Bezeichnung der Z Kamera ist {0}.", Z.GetBezeichnung);
            Console.WriteLine("Die Aufloesung der Z Kamera ist {0}MP.", Z.Aufloesung);
            X.UpdatePreis(790, Currency.EUR);
            var testX = X.GetBezeichnung;
            var testZ = Z.GetPrice(Currency.USD);
            Console.WriteLine($"Interface Bezeichnung:{testX}");
            Console.WriteLine($"Interface Preis:{testZ}");
            Console.WriteLine("Der Preis der X Kamera ist EUR{0}.", X.Preis);
            NetworkSwitch Cisco = new NetworkSwitch("Cisco SG 300-20", 20, 4, true, false, true, 243m, Currency.GBP);
            Console.WriteLine($" Feature = { Cisco.GetFeatureGigabit}");

            /* Task3 */
            var objekts = new Produkt[]
            {
                new Kamera("Sony SNC-VM631", 2.3, 569m, Currency.EUR),
                new Kamera("Sony SNC-CX600", 3.0, 870m, Currency.EUR),
                new Kamera("Sony SNC-EP550", 5.0, 1200m, Currency.EUR),
                new NetworkSwitch("Cisco SG 300-52", 52, 4, true, false, true, 599m, Currency.GBP),
                new NetworkSwitch("NETGEAR FS728TP-100EUS", 24, 2, false, true, true, 280m, Currency.EUR)
            };
            foreach (var s in objekts)
            {
                Console.WriteLine($" {s.GetBezeichnung} {s.GetPrice(Currency.USD)}USD");
            }

            /* Task4*/
            //Serializing
            var test = new NetworkSwitch("NETGEAR FS728TP-100EUS", 24, 2, false, true, true, 280m, Currency.EUR);
            string testjson = JsonConvert.SerializeObject(test);
            Console.WriteLine(testjson);

            
           
            using (StreamWriter file = File.CreateText(@"G:\Studium\SS17\Objektorientierte Methoden\OOD\test2.json"))
            {
                  foreach (var s in objekts)
                  {
                      JsonSerializer serializer = new JsonSerializer();
                      serializer.Serialize(file, s);
                  }
            }
            using (StreamWriter file = File.CreateText(@"G:\Studium\SS17\Objektorientierte Methoden\OOD\test1.json"))
            {
                
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, objekts[1]);
            }

            //Deserializing and printing to console

            Kamera testkam = JsonConvert.DeserializeObject<Kamera>(File.ReadAllText(@"G:\Studium\SS17\Objektorientierte Methoden\OOD\test1.json"));
            Console.WriteLine($" Modell = { testkam.GetBezeichnung}");
            /*using (StreamReader file = File.OpenText(@"G:\Studium\SS17\Objektorientierte Methoden\OOD\test1.json"))
            {

                JsonSerializer serializer = new JsonSerializer();
                NetworkSwitch testkam = (NetworkSwitch)serializer.Deserialize(file, typeof(NetworkSwitch));
                Console.WriteLine($" Feature = { testkam.GetFeatureGigabit}");
            }
            */
        }
    }

    /*Interface*/
    public interface Produkt
    {
        /// <summary>
        /// Gets a modell of this item.
        /// </summary>
        string GetBezeichnung { get; }

        /// <summary>
        /// Gets the item's price in the specified currency.
        /// </summary>
        decimal GetPrice(Currency currency);
    }

    public class Kamera : Produkt
    {
        /*Private Fields*/
        private string bezeichnung;
        private double aufloesung;
        private decimal preis;

        /*Konstruktor*/
        public Kamera(string newBezeichnung, double newAufloesung, decimal newPreis, Currency currency)
        {
            if (string.Compare(newBezeichnung, bezeichnung) == 0) throw new ArgumentOutOfRangeException("Bezeichnung muss vergeben werden");
            if (newAufloesung < 0) throw new ArgumentOutOfRangeException("Aufloesung kann nicht negativ sein");
            if (newAufloesung < 1) throw new ArgumentOutOfRangeException("Die Kamera ist keine Megapixel Kamera");
            if (newPreis < 0) throw new ArgumentOutOfRangeException("Der Preis darf nicht negativ sein");
            bezeichnung = newBezeichnung;
            aufloesung = newAufloesung;
            preis = newPreis;
        }

        /*Properties*/
        public double Aufloesung => aufloesung;

        public decimal Preis
        {
            get { return preis; }
            set
            {
                if (value < 0) throw new Exception("Preis darf nicht negativ sein");
                preis = value;
            }
        }

        public Currency Currency { get; private set; }

        /*Methode*/
        public void UpdatePreis(decimal newPreis, Currency currency)
        {
            if (newPreis < 0) throw new ArgumentOutOfRangeException("Der Preis darf nicht negativ sein");
            preis = newPreis;
            Currency = currency;
        }

        #region Produkt implementation
        //get description
        public string GetBezeichnung => bezeichnung;
        
        //get Price
        public decimal GetPrice(Currency currency)
        {
            if (currency == Currency) return Preis;

            // use web service to query current exchange rate
            // request : http://download.finance.yahoo.com/d/quotes.csv?s=EURUSD=X&f=sl1d1t1c1ohgv&e=.csv
            // response: "EURUSD=X",1.0930,"12/29/2015","6:06pm",-0.0043,1.0971,1.0995,1.0899,0
            var key = string.Format("{0}{1}", Currency, currency); // e.g. EURUSD means "How much is 1 EUR in USD?".

            // create the request URL, ...
            var url = string.Format(@"http://download.finance.yahoo.com/d/quotes.csv?s={0}=X&f=sl1d1t1c1ohgv&e=.csv", key);
            // download the response as string
            var data = new WebClient().DownloadString(url);
            // split the string at ','
            var parts = data.Split(',');
            // convert the exchange rate part to a decimal 
            var rate = decimal.Parse(parts[1], CultureInfo.InvariantCulture);

            // and finally perform the currency conversion
            return Preis * rate;
        }
        #endregion
    }

    public class NetworkSwitch : Produkt
    {

        /* Private fields */

        private string modell;
        private int anzahlKupferPorts;
        private int anzahlFiberPorts;
        private bool gigabit;
        private bool poefaehig;
        private bool managedSwitch;
        private decimal preis;

        

        /* Konstruktor */
        public NetworkSwitch(string newModell, int newCupperPorts, int newFiberPorts, bool newFeatureGigabit, bool newFeaturePoE, bool newManagementFeature, decimal newPrice, Currency currency)
        {
            if (string.Compare(newModell, modell) == 0) throw new ArgumentOutOfRangeException("Bezeichnung muss vergeben werden");
            if (newCupperPorts < 0 || newCupperPorts > 64) throw new ArgumentOutOfRangeException("Anzahl der Ports kann nicht negativ sein oder Anzahl der Ports nicht realistisch");
            if (newCupperPorts < 0 && newFiberPorts > 0 && newFeaturePoE == true) throw new ArgumentOutOfRangeException("Kombination nicht moeglich");
            if (newPrice < 0) throw new ArgumentOutOfRangeException("Der Preis darf nicht negativ sein");
            modell = newModell;
            anzahlKupferPorts = newCupperPorts;
            anzahlFiberPorts = newFiberPorts;
            gigabit = newFeatureGigabit;
            poefaehig = newFeaturePoE;
            managedSwitch = newManagementFeature;
            preis = newPrice;
            Currency = currency;
        }

        /* Properties */
        public string GetBezeichnung => modell;
        public int AnzahlKupferPorts => anzahlKupferPorts;
        public int AnzahlFiberPorts => anzahlFiberPorts;
        public string GetFeatureGigabit
        {
            get
            {
                if (gigabit == true)
                    return "Gigabit Switch";
                else
                    return "Megabit Switch";
            }
        }
        public string GetFeaturePoE
        {
            get
            {
                if (poefaehig == true)
                    return "PoE Switch";
                else
                    return "Feature wird nicht unterstuetzt";
            }
        }
        public string GetFeatureManaged
        {
            get
            {
                if (managedSwitch == true)
                    return "Managed Switch";
                else
                    return "Feature wird nicht unterstuetzt";
            }
        }
        public decimal GetPreis => preis;

        public Currency Currency { get; private set; }

        /* Methode */
        public void UpdatePreis(decimal newPreis, Currency currency)
        {
            if (newPreis < 0) throw new ArgumentOutOfRangeException("Der Preis darf nicht negativ sein");
            preis = newPreis;
            Currency = currency;
        }

        public decimal GetPrice(Currency currency)
        {
            // if the price is requested in it's own currency, then simply return the stored price
            if (currency == Currency) return preis;

            // use web service to query current exchange rate
            // request : http://download.finance.yahoo.com/d/quotes.csv?s=EURUSD=X&f=sl1d1t1c1ohgv&e=.csv
            // response: "EURUSD=X",1.0930,"12/29/2015","6:06pm",-0.0043,1.0971,1.0995,1.0899,0
            var key = string.Format("{0}{1}", Currency, currency); // e.g. EURUSD means "How much is 1 EUR in USD?".

            // create the request URL, ...
            var url = string.Format(@"http://download.finance.yahoo.com/d/quotes.csv?s={0}=X&f=sl1d1t1c1ohgv&e=.csv", key);
            // download the response as string
            var data = new WebClient().DownloadString(url);
            // split the string at ','
            var parts = data.Split(',');
            // convert the exchange rate part to a decimal 
            var rate = decimal.Parse(parts[1], CultureInfo.InvariantCulture);

            // and finally perform the currency conversion
            return preis * rate;
        }
    }
}

