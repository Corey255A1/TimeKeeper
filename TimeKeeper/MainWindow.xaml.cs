//Corey Wunderlich 2018
//The "Main" of the TimeKeeper.
//Ties all of the pieces of together and creates
//The user interface.
//
//NOTE: Should probably move a lot of this
//business logic out of the window class
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

using TimeKeeper.Models;
namespace TimeKeeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TimeTicker _time_ticker;
        private DateTime _start_time;
        private bool _work_timer_paused = true;
        private TimerElement _current_timer;
        private List<TimerElement> _timers = new List<TimerElement>();

        private TimeCardController _controller;
        public TimeCardController Controller
        {
            get => _controller;
        }

        public MainWindow()
        {
            _controller = new TimeCardController(System.AppDomain.CurrentDomain.BaseDirectory + "\\default.chg");
            DataContext = Controller;
            InitializeComponent();
            currentTimeClk.SetTime(DateTime.Now);
            _time_ticker = new TimeTicker();
            _time_ticker.TickEvent += Tick;
            startTimeClk.SetTime(new DateTime()); //00:00:00
            startTimeClk.ClockModified += ClockModified;

            
        }

        public void ClockModified(Clock obj, int h, int m, int s)
        {
            if (obj == startTimeClk && h < 24) _start_time = new DateTime(_start_time.Year, _start_time.Month, _start_time.Day, h, m, s);
        }

        public void Tick(DateTime t)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                currentTimeClk.SetTime(t);
                totalTimeClk.SetTime(t - _start_time);
                if (_controller.WorkTimerRunning) _current_timer?.SetTime(t);
                TimeSpan total = new TimeSpan();
                foreach (var telm in _timers)
                {
                    total += telm.GetTime();
                }
                chargedTimeClk.SetTime(total);
            }));

        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            var t = DateTime.Now;
            _start_time = new DateTime(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second); //truncate off any milliseconds
            startTimeClk.SetTime(_start_time);
        }
        private void TimerElement_Remove(object sender, EventArgs e)
        {
            _controller.RemoveChargeCode((sender as FrameworkElement).DataContext as ChargeCodeTimer);
        }

        private void TimerElement_WorkOn(object sender, EventArgs e)
        {
            _controller.WorkOnChargeCode((sender as FrameworkElement).DataContext as ChargeCodeTimer);
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            _controller.AddNewChargeCode();
            //telm.TimerActionPerformed += TimerActionCallback;
            
        }

        private void pauseBtn_Click(object sender, RoutedEventArgs e)
        {
            _controller.WorkTimerRunning = false;
        }

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            chargedTimeClk.SetTime(new TimeSpan(0, 0, 0));
            foreach (var telm in _timers)
            {
                telm.Clear();
            }
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
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

        private void saveBtn_Click(object sender, RoutedEventArgs e)
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

        private void logBtn_Click(object sender, RoutedEventArgs e)
        {
            var file = System.AppDomain.CurrentDomain.BaseDirectory + "\\timelog.csv";
            _controller.TimeCard.WriteCSV(file);
        }

        
    }
}
