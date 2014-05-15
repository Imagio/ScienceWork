using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ru.Imagio.Science.Math.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var w = Solver.Solve();
            Console.Write(w);
            Console.ReadKey();
        }
    }
}
