using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskTests
{
    public class ExceptionHandling
    {
        public static void Run()
        {
            try
            {
                new Thread(Go).Start();
            }
            catch (Exception exception)
            {
                //We'll never get here!
                //Each thread has an independent execution path
                //The remedy is to move the exception handler to the Go() method.
                Console.WriteLine("Exception !!!");
                throw;
            }
        }

        public static void Go()
        {
            throw null;//Throws a null reference exception.
        }
    }
}
