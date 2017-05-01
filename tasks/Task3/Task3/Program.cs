using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;


namespace Task3
{
    public enum Currency { EUR, GBP, USD, YEN }

    class Program
    {
        static void Main(string[] args)
        {
            Kamera X = new Kamera("Sony STC300IR", 3.0, 800m, Currency.EUR);
            Kamera Z = new Kamera("Geovision BL1300", 1.3, 670m, Currency.EUR);
            Console.WriteLine("Die Bezeichnung der X Kamera ist {0}.", X.Bezeichnung);
            Console.WriteLine("Die Bezeichnung der Z Kamera ist {0}.", Z.Bezeichnung);
            Console.WriteLine("Die Aufloesung der Z Kamera ist {0}MP.", Z.Aufloesung);
            X.UpdatePreis(790, Currency.EUR);
            var testX = X.GetBezeichnung;
            var testZ = Z.GetPrice(Currency.USD);
            Console.WriteLine($"Interface Bezeichnung:{testX}");
            Console.WriteLine($"Interface Preis:{testZ}");
            Console.WriteLine("Der Preis der X Kamera ist EUR{0}.", X.Preis);
            Bestellung NeuBest = new Bestellung(X, 3, Currency.USD);
            Bestellung testbestellung = new Bestellung(Z, 5, Currency.EUR);
            Console.WriteLine($"Betrag der Bestellung Z = { testbestellung.Betrag}");


            var objekts = new Produkt[]
            {
                new Kamera("Sony SNC-VM631", 2.3, 569m, Currency.EUR),
                new Kamera("Sony SNC-CX600", 3.0, 870m, Currency.EUR),
                new Kamera("Sony SNC-EP550", 5.0, 1200m, Currency.EUR),
                new Bestellung(X, 3, Currency.EUR),
                new Bestellung(Z, 5, Currency.EUR),
            };
            foreach (var s in objekts)
            {
                Console.WriteLine($" {s.GetBezeichnung} {s.GetPrice(Currency.USD)}USD");
            }
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

        public string Bezeichnung
        {
            get
            {
                return bezeichnung;
            }
            set
            {
                bezeichnung = value;
            }
        }

        public double Aufloesung
        {
            get
            {
                return aufloesung;
            }
            set
            {
                if (value < 1) throw new Exception("Die Kamera ist keine Megapixel Kamera");
                aufloesung = value;
            }
        }

        public decimal Preis
        {
            get
            {
                return preis;
            }
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
        public string GetBezeichnung
        {
            get
            {
                return bezeichnung;
            }
        }
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

    public class Bestellung : Produkt
    {
        private int Anzahl;
        Kamera Type;
        private string Modell;
        private decimal Preis;
        private decimal Summe;
        
        public Bestellung(Kamera produkt, int anzahl, Currency currency)
        {
            if (anzahl <= 0) throw new ArgumentOutOfRangeException("Menge darf nicht negativ oder gleich NULL sein");
            Anzahl = anzahl;
            Type = produkt; 
            Preis = Type.Preis;
            Currency = currency;
        }

        /// <summary>
        /// Value of this gift card.
        /// </summary>
        public decimal Betrag
        {
            get
            {
                Summe = Preis * Anzahl;
                return Summe;
            }
        }

        public Currency Currency { get; }

        public string GetBezeichnung
        {
            get
            {
                Modell = Type.Bezeichnung;
                return Modell;
            }
        }

        public decimal GetPrice(Currency currency)
        {
            // if the price is requested in it's own currency, then simply return the stored price
            if (currency == Currency) return Summe;

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
            return Summe * rate;
        }
    }
}

