//Corey Wunderlich 2018
//The "Main" of the TimeKeeper.
//Ties all of the pieces of together and creates
//The user interface.
//
using System;
using System.Windows;

using TimeKeeper.Models;
namespace TimeKeeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TimeCardController _controller;
        public TimeCardController Controller
        {
            get => _controller;
        }

        public MainWindow()
        {
            _controller = new TimeCardController(Dispatcher, System.AppDomain.CurrentDomain.BaseDirectory + "\\default.chg");
            DataContext = Controller;
            InitializeComponent();
        }

        private void StartTimer_Clicked(object sender, RoutedEventArgs e)
        {
            var t = DateTime.Now;
            _controller.StartDateTime = new DateTime(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second); //truncate off any milliseconds
        }
        private void TimerElement_Remove(object sender, EventArgs e)
        {
            _controller.RemoveChargeCode((sender as FrameworkElement).DataContext as ChargeCodeTimer);
        }

        private void TimerElement_WorkOn(object sender, EventArgs e)
        {
            _controller.WorkOnChargeCode((sender as FrameworkElement).DataContext as ChargeCodeTimer);
            _controller.WorkTimerRunning = true;
        }

        private void AddNewChargeCode_Clicked(object sender, RoutedEventArgs e)
        {
            _controller.AddNewChargeCode();
        }

        private void PauseTimers_Clicked(object sender, RoutedEventArgs e)
        {
            _controller.WorkTimerRunning = false;
        }

        private void ResetTimers_Clicked(object sender, RoutedEventArgs e)
        {
            _controller.Reset();
        }

        private void LoadLogButton_Clicked(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "ChargeCodes",
                DefaultExt = "chg",
                Filter = "Charge Codes (.chg)|*.chg"
            };

            if (ofd.ShowDialog() == true)
            {
                _controller.TimeCard.Load(ofd.FileName);
            }
        }

        private void SaveLogButton_Clicked(object sender, RoutedEventArgs e)
        {
            var sfd = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "ChargeCodes",
                DefaultExt = "chg",
                Filter = "Charge Codes (.chg)|*.chg"
            };
            if (sfd.ShowDialog() == true)
            {
                _controller.TimeCard.Save(sfd.FileName);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var file = System.AppDomain.CurrentDomain.BaseDirectory + "\\default.chg";
            _controller.TimeCard.Save(file);
        }

        private void LogBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var file = System.AppDomain.CurrentDomain.BaseDirectory + "\\timelog.csv";
            _controller.TimeCard.WriteCSV(file);
        }

        private void StartTimerModified(int hour, int minute, int second)
        {
            Controller.AdjustStartTime(hour, minute, second);
        }
    }
}
