using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Task6
{
    class Pull_Push_Task
    {
        public static void Run(Produkt[] products)
        {


            /* IObservable tests */
            var testsubj = new Subject<Produkt>();
            var subscription = testsubj.Subscribe();
            Console.WriteLine($"\n");
            foreach (var s in products)
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                testsubj.OnNext(s);
                Console.WriteLine($"Bezeichnung:{s.GetDescription}");
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            testsubj.OnCompleted();
            Console.WriteLine("Sequence completed.");
            subscription.Dispose();



            var testobs = new Subject<Produkt>();
            var test = from p in testobs
                       where p.Price == 456m
                       select p;

            Console.WriteLine($"\n");

            test.Subscribe(p => {
                Console.WriteLine("Modell:{0} Price:{1}", p.GetDescription, p.Price); 
            });
            Console.WriteLine($"\n");
            
            foreach (var t in products)
            {
                Type type = t.GetType();
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                if (type.Equals(typeof(Kamera)))
                   Console.WriteLine("{0}", t.GetDescription);            
                   testobs.OnNext(t);          
            }
            testobs.OnCompleted();
            Console.WriteLine("Sequence completed.");
            testobs.Dispose();


            /* Tasks tests */
           
            var tasks = new List<Task<decimal>>();
            foreach (var x in products)
            {
                var task = Task.Run(() =>
                {
                    Console.WriteLine($"computing price for {x.GetDescription}");
                    //Task.Delay(TimeSpan.FromSeconds(5.0)).Wait();
                    Console.WriteLine($"done computing price for {x.GetDescription}");
                    return x.Price;
                });

                tasks.Add(task);
            }
            Console.WriteLine("Doing something else");

            var tasks2 = new List<Task<decimal>>();

            foreach (var task in tasks.ToArray())
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
                tasks2.Add(
                    task.ContinueWith(t => { Console.WriteLine($"Price is {t.Result} {Currency.EUR}"); return t.Result; })
                );
            }

        }
    }
}
