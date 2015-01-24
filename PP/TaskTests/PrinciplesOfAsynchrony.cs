using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskTests
{
    public class PrinciplesOfAsynchrony
    {
        public static int GetPrimesCount(int start, int count)
        {
            return
            ParallelEnumerable.Range(start, count).Count(n =>
            Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
            //return Enumerable.Range(2, 3000000).Count(n =>
            //        Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
        }

        public static Task<int> GetPrimesCountAsync(int start, int count)
        {
            return Task.Run(() =>
            ParallelEnumerable.Range(start, count).Count(n =>
            Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
            //return Enumerable.Range(2, 3000000).Count(n =>
            //        Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
        }

        public static void DisplayPrimeCounts()
        {
            for (int i = 0; i < 10; i++)
            {
                var awaiter = GetPrimesCountAsync(i * 1000000 + 2, 1000000).GetAwaiter();

                int i1 = i;
                awaiter.OnCompleted(() =>
                    Console.WriteLine(awaiter.GetResult() + " primes between " + (i1 * 1000000) + " and " + ((i1 + 1) * 1000000 - 1)));
            }
            Console.WriteLine("Done!");
        }

        //No senwce to use ASYNC in an interface
        public static async Task DisplayPrimeCountsAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                int i1 = i;
                Console.WriteLine(await GetPrimesCountAsync(i * 1000000 + 2, 1000000) + " primes between " + (i1 * 1000000) + " and " + ((i1 + 1) * 1000000 - 1) + "On thread: " + Thread.CurrentThread.ManagedThreadId);
            }
            Console.WriteLine("Done! " + "On thread: " + Thread.CurrentThread.ManagedThreadId);
        }

        public static async Task SimpleAwait()
        {
            await Task.Delay(5000);
            Console.WriteLine("SimpleAwait() await 5 sec.");
        }

        public static async Task SimpleAwait2()
        {
            await Task.Delay(2000);
            Console.WriteLine("SimpleAwait2()" +
                              " await 2 sec.");
        }

        public static async void CallTwoAwaits()
        {
            await SimpleAwait();
            await SimpleAwait2();
        }
    }
}
