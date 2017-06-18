﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Task4
{
    class Serialization
    {
        public static void Run(Produkt[] products)
        {
            /* Task4*/
            //Serializing

            var test = new NetworkSwitch("NETGEAR FS728TP-100EUS", 24, 2, false, true, true, 280m, Currency.EUR); 
            string testjson = JsonConvert.SerializeObject(test);
            Console.WriteLine(testjson);

            var settings1 = new JsonSerializerSettings() { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.Auto };
            File.WriteAllText(@"G:\Studium\SS17\Objektorientierte Methoden\OOD\jsonfile.json", testjson);
            var destest = JsonConvert.DeserializeObject<NetworkSwitch>(File.ReadAllText(@"G:\Studium\SS17\Objektorientierte Methoden\OOD\jsonfile.json"), settings1);
            Console.WriteLine(destest);


            var settings2 = new JsonSerializerSettings() { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.Auto };
            Console.WriteLine(JsonConvert.SerializeObject(products, settings2));

            var text = JsonConvert.SerializeObject(products, settings2);
            string pathfile = @"G:\Studium\SS17\Objektorientierte Methoden\OOD";
            var filename = Path.Combine(pathfile, "products.json");
            File.WriteAllText(filename, text);


            var textFromFile = File.ReadAllText(filename);
            var productsFromFile = JsonConvert.DeserializeObject<Produkt[]>(textFromFile, settings2);
            var currency = Currency.EUR;
            foreach (var x in productsFromFile) Console.WriteLine($"{x.GetDescription} {x.Price}{currency}");

        }
    }
    
}
