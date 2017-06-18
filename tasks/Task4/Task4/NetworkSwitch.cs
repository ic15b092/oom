using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using Newtonsoft.Json;

namespace Task4
{
    public class NetworkSwitch : Produkt
    {

        /* Private fields */

        private decimal m_price;

        /* Constructors */

        /// <summary>
        /// Creates a new Network Switch object.
        /// </summary>
        /// <param name="modell">Modell must not be empty.</param>
        /// <param name="cupper_ports">Number of cupper ports.</param>
        /// <param name="SFP_ports">Number of SFP ports.</param>
        /// <param name="gigabit">Gigabit/Megabit Switch.</param>
        /// <param name="poe">PoE Feature.</param>
        /// <param name="managed">Managed Switch.</param>
        /// <param name="price">Price must not be negative.</param>
        [JsonConstructor]
        public NetworkSwitch(string modell, int cupper_ports, int SFP_ports, bool gigabit, bool poe, bool managed, decimal price, Currency currency)
        {
            if (string.IsNullOrWhiteSpace(modell)) throw new ArgumentException("Bezeichnung muss vergeben werden");
            if (cupper_ports < 0 || cupper_ports > 64) throw new ArgumentOutOfRangeException("Anzahl der Ports kann nicht negativ sein oder Anzahl der Ports nicht realistisch");
            if (cupper_ports < 0 && SFP_Ports > 0 && poe == true) throw new ArgumentOutOfRangeException("Kombination nicht moeglich");
            if (price < 0) throw new ArgumentOutOfRangeException("Der Preis darf nicht negativ sein");
            Modell = modell;
            Cupper_Ports = cupper_ports;
            SFP_Ports = SFP_ports;
            Gigabit = gigabit;
            PoE = poe;
            Managed = managed;
            Price = price;
            Currency = currency;
        }

        /* Properties */

        /// <summary>
        /// Gets the description of this item.
        /// </summary>
        public string Modell { get; }

        /// <summary>
        /// Gets the number of cupper ports of this item.
        /// </summary>
        public int Cupper_Ports { get; }

        /// <summary>
        /// Gets the number of SFP ports of this item.
        /// </summary>
        public int SFP_Ports { get; }

        /// <summary>
        /// Gets the Gigabit feature of this item.
        /// </summary>
        public bool Gigabit { get; }

        /// <summary>
        /// Gets the PoE feature of this item.
        /// </summary>
        public bool PoE { get; }

        /// <summary>
        /// Gets the Management feature of this item.
        /// </summary>
        public bool Managed { get; }


        public decimal Price { get; private set; }
        /// <summary>
        /// Gets and sets the currency of this item.
        /// </summary>
        public Currency Currency { get; private set; }

        /* Methods */

        /// <summary>
        /// Updates the price and the currency of this item.
        /// </summary>
        public void UpdatePrice(decimal newPrice, Currency currency)
        {
            if (newPrice < 0) throw new ArgumentOutOfRangeException("Der Preis darf nicht negativ sein");
            m_price = newPrice;
            Currency = currency;
        }

        /// <summary>
        /// Convert the price of this item in a specific currency.
        /// </summary>
        public decimal GetPrice(Currency currency)
        {
            // if the price is requested in it's own currency, then simply return the stored price
            if (currency == Currency) return m_price;

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
            return m_price * rate;
        }
        #region Produkt implementation

        /// <summary>
        /// Gets a textual description of this product.
        /// </summary>
        [JsonIgnore]
        public string GetDescription => Modell;

        /// <summary>
        /// Gets the price of this item.
        /// </summary>
        

        #endregion
    }
}
