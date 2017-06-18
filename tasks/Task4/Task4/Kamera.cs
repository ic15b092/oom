using System;
using System.Globalization;
using System.Net;
using Newtonsoft.Json;

namespace Task4
{
    public class Kamera : Produkt
    {
        /*Private Fields*/
        
        private decimal preis;

        /*Konstruktor*/
        public Kamera(string bezeichnung, double aufloesung, decimal preis, Currency currency)
        {
            if (string.IsNullOrWhiteSpace(bezeichnung)) throw new ArgumentException("Bezeichnung muss vergeben werden");
            if (aufloesung < 0) throw new ArgumentOutOfRangeException("Aufloesung kann nicht negativ sein");
            if (aufloesung < 1) throw new ArgumentOutOfRangeException("Die Kamera ist keine Megapixel Kamera");
            if (preis < 0) throw new ArgumentOutOfRangeException("Der Preis darf nicht negativ sein");
            Bezeichnung = bezeichnung;
            Aufloesung = aufloesung;
            Preis = preis;
        }

        /*Properties*/
        public string Bezeichnung { get; private set; }

        public double Aufloesung { get; private set; }

 
        public Currency Currency { get; private set; }

        /*Methode*/
        public void UpdatePreis(decimal newPreis, Currency currency)
        {
            if (newPreis < 0) throw new ArgumentOutOfRangeException("Der Preis darf nicht negativ sein");
            preis = newPreis;
            Currency = currency;
        }

        public decimal Preis { get; private set; }

        public decimal GetPrice(Currency currency)
        {
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

        #region Produkt implementation
        /// <summary>
        /// Gets a textual description of this product.
        /// </summary>
        /// 
        [JsonIgnore]
        public string GetDescription => Bezeichnung;

        /// <summary>
        /// Gets the price of this product.
        /// </summary>
        public decimal Price => preis;

        #endregion
    }
}
