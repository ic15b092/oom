using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson6
{
    class Program
    {
        static IEnumerable<R> Map<T, R>(IEnumerable<T> ts, Func<T, R> mapping)
        {
            foreach (var t in ts)
            {
                yield return f(t);
            }
        }

        static void Main(string[] args)
        {
            var xs = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var ys = Map(xs, x => "Hallo" + x * 2);
            foreach (var x in ys)  Console.WriteLine(x);

            var e = ys.GetEnumerator;


        }
    }
}
