using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskTests
{
    internal class Program
    {
        private static bool _done;
        private static readonly object _locker = new object();

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            TaskTheme.RunSimpleTaskWithWait();

            Console.ReadLine();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("*********************************");
            Console.WriteLine(sender);
            Console.WriteLine(e.ExceptionObject);
            Console.WriteLine("*********************************");
        }

        private static void ThreadStart()
        {
            lock (_locker)
            {

                if (!_done)
                {
                    Console.WriteLine("DONE");
                    _done = true;
                }
            }
        }
    }

}
