using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTests
{
    public class TaskTheme
    {
        public static void RunSimpleTask()
        {
            Task.Run(() => { Console.WriteLine("Foo"); });
        }

        public static void RunSimpleTaskWithWait()
        {
            var task = Task.Run(() => { Console.WriteLine("Foo"); });
            Console.WriteLine("Task is complited:{0}", task.IsCompleted);
            //calling Wait on a task blocks until it completes(equivalent of calling Join on a thread)
            task.Wait();
            Console.WriteLine("");
            Console.WriteLine("Task is complited:{0}", task.IsCompleted);
        }

        /// <summary>
        /// Returns count of prime numbers range of 2 , 3000000
        /// </summary>
        public static void ReturningValues()
        {
            Task<int> primeNumberTask = Task.Run(() =>
                Enumerable.Range(2, 3000000)
                .Count(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));

            Console.WriteLine("Task running...");
            Console.WriteLine("The answer is " + primeNumberTask.Result); //Bloks if not already finished
        }
    }
}
