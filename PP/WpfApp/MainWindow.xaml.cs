using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Go();
        }

        private async void Go()
        {
            for (int i = 1; i < 5; i++)
            {
                btnClick.IsEnabled = false;
                txtResults.Text += await GetPrimeCount(i * 1000000, 1000000) + " " + i + Environment.NewLine;
                btnClick.IsEnabled = true;
            }
        }

        private Task<int> GetPrimeCount(int start, int count)
        {
            return Task.Run(() =>
                ParallelEnumerable.Range(start, count)
                    .Count(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
        }

        private async Task<int> GetPrimeCount2(int start, int count)
        {
            return await Task.Run(() =>
                ParallelEnumerable.Range(start, count)
                    .Count(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
        }

        public void ContinuationsWithAwaiter()
        {
            Task<int> primeNumberTask = Task.Run(() =>
            {
                return Enumerable.Range(2, 3000000).Count(n =>
                     Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
            });

            //var awaiter = primeNumberTask.ConfigureAwait(true).GetAwaiter();
            var awaiter = primeNumberTask.GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                int result = awaiter.GetResult(); /* If the task faults, the exception is re-thrown when the continuation code calls 
                                                     awaiter.GetResult() */
                txtResults.Text += " " + result.ToString();
            });
        }

        public void ContinuationsWith_ContinueWith()
        {
            Task<int> primeNumberTask = Task.Factory.StartNew(() => ParallelEnumerable.Range(2, 3000000).Count(n =>
                Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
            primeNumberTask.ContinueWith(task =>
            {
                TextBox.Text += " " + task.Result.ToString();
            }, TaskScheduler.FromCurrentSynchronizationContext());

            //primeNumberTask.Wait();
        }

        private Task<string> DoWork()
        {

            return Task.Run(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                return "Done with work!";
            });
        }
    }
}
