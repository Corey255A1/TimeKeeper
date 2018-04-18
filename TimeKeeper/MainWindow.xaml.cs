//Corey Wunderlich 2018
//The "Main" of the TimeKeeper.
//Ties all of the pieces of together and creates
//The user interface.
//
//NOTE: Should probably move a lot of this
//business logic out of the window class
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
using System.IO;

using CSCSV;
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
            startTimeClk.ClockModified += ClockModified;

            var file = System.AppDomain.CurrentDomain.BaseDirectory + "\\default.chg";
            if (File.Exists(file))
            {
                var ccf = ChargeCodeFile.ReadFile(file);
                Timers.Clear();
                chargeNumberStack.Children.Clear();
                CurrentTimer = null;
                foreach (var ccode in ccf.ChargeCode)
                {
                    var telement = new TimerElement(ccode.Code, ccode.Description);
                    Timers.Add(telement);
                    chargeNumberStack.Children.Add(telement);
                    telement.TimerActionPerformed += TimerActionCallback;
                }
            }

        }

        public void ClockModified(Clock obj, int h, int m, int s)
        {
            if (obj == startTimeClk && h<24) StartTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, h, m, s);
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

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            chargedTimeClk.SetTime(new TimeSpan(0, 0, 0));
            foreach (var telm in Timers)
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
                var name = ofd.FileName;
                var ccf = ChargeCodeFile.ReadFile(name);
                Timers.Clear();
                chargeNumberStack.Children.Clear();
                CurrentTimer = null;
                foreach(var ccode in ccf.ChargeCode)
                {
                    var telement = new TimerElement(ccode.Code, ccode.Description);
                    Timers.Add(telement);
                    chargeNumberStack.Children.Add(telement);
                    telement.TimerActionPerformed += TimerActionCallback;
                }

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

            if (sfd.ShowDialog()==true)
            {
                var name = sfd.FileName;
                var ccf = new ChargeCodeFile();
                foreach(var timer in Timers)
                {
                    ccf.ChargeCode.Add(new ChargeCode()
                    {
                        Code = timer.Code,
                        Description = timer.Description
                    });
                }
                ccf.WriteFile(name);
                
            }


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var file = System.AppDomain.CurrentDomain.BaseDirectory + "\\default.chg";
            var ccf = new ChargeCodeFile();
            foreach (var timer in Timers)
            {
                ccf.ChargeCode.Add(new ChargeCode()
                {
                    Code = timer.Code,
                    Description = timer.Description
                });
            }
            ccf.WriteFile(file);
        }

        private void logBtn_Click(object sender, RoutedEventArgs e)
        {
            var file = System.AppDomain.CurrentDomain.BaseDirectory + "\\timelog.csv";
            var timeDict = new Dictionary<string, TimerElement>();
            foreach (var timer in Timers)
            {
                if(!timeDict.ContainsKey(timer.Code))
                {
                    timeDict.Add(timer.Code, timer);
                }
                
            }
            DateTime currTime = DateTime.Now;
            string todaysColumn = currTime.ToShortDateString();
            if (File.Exists(file))
            {
                //If I have the file already, we must add our time for today
                CSCSV.Table lasttable = CSCSV.Table.LoadFromFile(file);
                //If this table doesn't have a column for today, add one
                if (!lasttable.ContainsHeader(todaysColumn))
                {
                    lasttable.AddColumn(todaysColumn);
                }
                List<string> chargecodes = lasttable.GetColumn("Charge Codes").ToList();
                int ccCount = chargecodes.Count();
                foreach (string code in timeDict.Keys)
                {
                    int idx = chargecodes.IndexOf(code);
                    //If the code is already in the list, set the time.
                    if (idx >= 0)
                    {
                        lasttable.SetValue(todaysColumn, idx, timeDict[code].GetTime().ToString());
                    }
                    else // We will have to add the code..
                    {
                        lasttable.RowCount += 1;
                        lasttable.SetValue("Charge Codes", ccCount, code);
                        lasttable.SetValue(todaysColumn, ccCount, timeDict[code].GetTime().ToString());

                        ++ccCount;
                    }
                }

                lasttable.WriteToFile(file);

            }
            else
            {
                var newtable = new CSCSV.Table();
                newtable.RowCount = timeDict.Count();
                newtable.AddColumn("Charge Codes");
                newtable.AddColumn(todaysColumn);
                int ccCount = 0;
                foreach (string code in timeDict.Keys)
                {
                    newtable.SetValue("Charge Codes", ccCount, code);
                    newtable.SetValue(todaysColumn, ccCount, timeDict[code].GetTime().ToString());

                    ++ccCount;
                }
                newtable.WriteToFile(file);
            }
        }
    }
}
