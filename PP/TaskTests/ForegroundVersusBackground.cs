using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskTests
{
    public class ForegroundVersusBackground
    {
        public static void Run(string[] args)
        {
            Thread worker = new Thread(() => Console.ReadLine());
            worker.Name = "My Thread";
            if (args.Length > 0) worker.IsBackground = true;
            worker.Start();
        }
    }
}
