﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

        public static void HandledExceptions()
        {
            Task task = Task.Run(() => { throw null; });
            try
            {
                task.Wait();
            }
            catch (AggregateException aex)//The CLR wraps the exception in an AggregateException
            {
                if (aex.InnerException is NullReferenceException)
                    Console.WriteLine("Null!");
                else
                    throw;
            }
        }

        public static void UnnandledExceptions()
        {
            Task.Run(() => { throw null; }).Wait();
        }

        /// <summary>
        /// A continaution says to task, "when you've finished, continue by doing something else"
        /// in c# 5 it used by async functions
        /// </summary>
        public static void ContinuationsWithAwaiter()
        {
            Task<int> primeNumberTask = Task.Run(() =>
            {
                Console.WriteLine("Thread in the task: " + Thread.CurrentThread.ManagedThreadId);
                return Enumerable.Range(2, 3000000).Count(n =>
                     Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
            });

            //var awaiter = primeNumberTask.ConfigureAwait(true).GetAwaiter();
            var awaiter = primeNumberTask.GetAwaiter();
            Console.WriteLine("Thread between " + Thread.CurrentThread.ManagedThreadId);
            awaiter.OnCompleted(() =>
            {
                int result = awaiter.GetResult(); /* If the task faults, the exception is re-thrown when the continuation code calls 
                                                     awaiter.GetResult() */
                Console.WriteLine("Thread in OnCompleted: " + Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine(result);
            });
        }

        public static void ContinuationsWith_ContinueWith()
        {
            Task<int> primeNumberTask = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Thread in the task: " + Thread.CurrentThread.ManagedThreadId);
                return Enumerable.Range(2, 3000000).Count(n =>
                     Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
            }/*,TaskCreationOptions.LongRunning*/); // run on non-pooled threads
            Console.WriteLine("Thread between " + Thread.CurrentThread.ManagedThreadId);
            primeNumberTask.ContinueWith(task =>
            {
                Console.WriteLine("Thread in OnCompleted: " + Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine(task.Result);
            });

            primeNumberTask.Wait();
        }

        public static void TaskCompletionSource_First()
        {
            var tcs = new TaskCompletionSource<int>();

            new Thread(() => { Thread.Sleep(2000); tcs.SetResult(999); }).Start();

            var tsk = tcs.Task;

            Console.WriteLine(tsk.Result);
        }

        public static Task<TResult> Run<TResult>(Func<TResult> func)
        {
            var tcs = new TaskCompletionSource<TResult>();

            new Thread(() =>
             {
                 try
                 {
                     tcs.SetResult(func());
                 }
                 catch (Exception exception)
                 {
                     tcs.SetException(exception);
                 }
             }).Start();

            return tcs.Task;
        }

        public static void TaskDelay()
        {
            Task.Delay(2000).GetAwaiter().OnCompleted(() => Console.WriteLine("Koo-koo..."));
        }
    }
}
