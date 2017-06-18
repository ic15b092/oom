using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Newtonsoft.Json;

namespace Task6
{
    public enum Currency { EUR, GBP, USD, YEN }

    
    class Program
    {
        public static void Main(string[] args)
        {
            Kamera X = new Kamera("Sony STC300IR", 3.0, 800m, Currency.EUR);
            Kamera Z = new Kamera("Geovision BL1300", 1.3, 670m, Currency.EUR);
            Console.WriteLine("Die Bezeichnung der X Kamera ist {0}.", X.GetDescription);
            Console.WriteLine("Die Bezeichnung der Z Kamera ist {0}.", Z.GetDescription);
            Console.WriteLine("Die Aufloesung der Z Kamera ist {0}MP.", Z.Aufloesung);
            X.UpdatePreis(790, Currency.EUR);
            var testX = X.GetDescription;
            var testZ = Z.GetPrice(Currency.USD);
            Console.WriteLine($"Interface Bezeichnung:{testX}");
            Console.WriteLine($"Interface Preis:{testZ}");
            Console.WriteLine("Der Preis der X Kamera ist EUR{0}.", X.Price);
            NetworkSwitch Cisco = new NetworkSwitch("Cisco SG 300-20", 20, 4, true, false, true, 243m, Currency.GBP);
            Console.WriteLine($" Feature = { Cisco.Gigabit}");

            /* Task3 */
            //array of products
            var products = new Produkt[]
            {
               new Kamera("Sony SNC-VM631", 2.3, 569m, Currency.EUR),
               new Kamera("Sony SNC-CX600", 3.0, 870m, Currency.EUR),
               new Kamera("Sony SNC-EP500", 5.0, 1200m, Currency.EUR),
               new Kamera("Sony SNC-TX800", 3.2, 775m, Currency.EUR),
               new Kamera("Sony SNC-EP550", 5.5, 1360m, Currency.EUR),
               new NetworkSwitch("Cisco SG 300-52", 52, 4, true, false, true, 599m, Currency.GBP),
               new NetworkSwitch("Cisco SG 300-26", 26, 2, false, false, true, 456m, Currency.GBP), 
               new NetworkSwitch("Cisco SG 400-24", 24, 3, false, false, false, 356m, Currency.GBP),
               new NetworkSwitch("NETGEAR FS728TP-100EUS", 24, 2, false, true, true, 456m, Currency.EUR),
               new NetworkSwitch("NETGEAR FS008TP-1000EUS", 8, 2, true, true, true, 256m, Currency.EUR)
            };
            Console.WriteLine("Preis Sony SNC-CX600: {0}", products[1].Price);
            foreach (var s in products)
            {
                Console.WriteLine("Task3: {0} {1}", s.GetDescription, s.Price);
            }


            /* Task4 */
            /* Serialization*/

            Serialization.Run(products);

            /* Task 6 */

            Pull_Push_Task.Run(products);

        }
    }

   

    

    
}

