using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace TimeKeeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TimeTicker theTicker;
        DateTime StartTime;
        bool WorkTimerPaused = true;
        TimerElement CurrentTimer;
        List<TimerElement> Timers = new List<TimerElement>();
        public MainWindow()
        {
            InitializeComponent();
            currentTimeClk.SetTime(DateTime.Now);
            theTicker = new TimeTicker();
            theTicker.TickEvent += Tick;
            startTimeClk.SetTime(new DateTime()); //00:00:00

        }
        public void Tick(DateTime t)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                currentTimeClk.SetTime(t);
                totalTimeClk.SetTime(t - StartTime);
                if(!WorkTimerPaused) CurrentTimer?.SetTime(t);
                TimeSpan total = new TimeSpan();
                foreach(var telm in Timers)
                {
                    total += telm.GetTime();
                }
                chargedTimeClk.SetTime(total);
            }));
            
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            var t = DateTime.Now;
            StartTime = new DateTime(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second); //truncate off any milliseconds
            startTimeClk.SetTime(StartTime);
            chargedTimeClk.SetTime(new TimeSpan(0,0,0));
            totalTimeClk.SetTime(new TimeSpan(0, 0, 0));
            foreach (var telm in Timers)
            {
                telm.Clear();
            }
        }

        private void TimerActionCallback(TimerElement t, TimerElementActionEnum e)
        {
            switch(e)
            {
                case TimerElementActionEnum.WorkOn: CurrentTimer = t; WorkTimerPaused = false; break;
                //case TimerElementActionEnum.Pause:  break;
                case TimerElementActionEnum.Remove:
                    Timers.Remove(t);
                    chargeNumberStack.Children.Remove(t);
                    if (t == CurrentTimer)
                    {
                        WorkTimerPaused = true;
                        CurrentTimer = null;
                    }
                    
                    break;
            }
            
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            var telm = new TimerElement();
            Timers.Add(telm);
            telm.TimerActionPerformed += TimerActionCallback;
            chargeNumberStack.Children.Add(telm);
        }

        private void pauseBtn_Click(object sender, RoutedEventArgs e)
        {
            WorkTimerPaused = true;
        }
    }
}
