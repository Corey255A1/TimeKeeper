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
        public MainWindow()
        {
            InitializeComponent();
            currentTimeClk.SetTime(DateTime.Now);
            _time_ticker = new TimeTicker();
            _time_ticker.TickEvent += Tick;
            startTimeClk.SetTime(new DateTime()); //00:00:00
            startTimeClk.ClockModified += ClockModified;

            var file = System.AppDomain.CurrentDomain.BaseDirectory + "\\default.chg";
            if (File.Exists(file))
            {
                var ccf = ChargeCodeFile.ReadFile(file);
                _timers.Clear();
                chargeNumberStack.Children.Clear();
                _current_timer = null;
                foreach (var ccode in ccf.ChargeCode)
                {
                    var telement = new TimerElement(ccode.Code, ccode.Description);
                    _timers.Add(telement);
                    chargeNumberStack.Children.Add(telement);
                    telement.TimerActionPerformed += TimerActionCallback;
                }
            }

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
                if (!_work_timer_paused) _current_timer?.SetTime(t);
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

        private void TimerActionCallback(TimerElement t, TimerElementActionEnum e)
        {
            switch (e)
            {
                case TimerElementActionEnum.WorkOn: _current_timer = t; _work_timer_paused = false; break;
                //case TimerElementActionEnum.Pause:  break;
                case TimerElementActionEnum.Remove:
                    _timers.Remove(t);
                    chargeNumberStack.Children.Remove(t);
                    if (t == _current_timer)
                    {
                        _work_timer_paused = true;
                        _current_timer = null;
                    }

                    break;
            }

        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            var telm = new TimerElement();
            _timers.Add(telm);
            telm.TimerActionPerformed += TimerActionCallback;
            chargeNumberStack.Children.Add(telm);
        }

        private void pauseBtn_Click(object sender, RoutedEventArgs e)
        {
            _work_timer_paused = true;
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
                var name = ofd.FileName;
                var ccf = ChargeCodeFile.ReadFile(name);
                _timers.Clear();
                chargeNumberStack.Children.Clear();
                _current_timer = null;
                foreach (var ccode in ccf.ChargeCode)
                {
                    var telement = new TimerElement(ccode.Code, ccode.Description);
                    _timers.Add(telement);
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

            if (sfd.ShowDialog() == true)
            {
                var name = sfd.FileName;
                var charge_code_file = new ChargeCodeFile();
                foreach (var timer in _timers)
                {
                    charge_code_file.ChargeCode.Add(new ChargeCode()
                    {
                        Code = timer.Code,
                        Description = timer.Description
                    });
                }
                charge_code_file.WriteFile(name);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var file = System.AppDomain.CurrentDomain.BaseDirectory + "\\default.chg";
            var charge_code_file = new ChargeCodeFile();
            foreach (var timer in _timers)
            {
                charge_code_file.ChargeCode.Add(new ChargeCode()
                {
                    Code = timer.Code,
                    Description = timer.Description
                });
            }
            charge_code_file.WriteFile(file);
        }

        private void logBtn_Click(object sender, RoutedEventArgs e)
        {
            var file = System.AppDomain.CurrentDomain.BaseDirectory + "\\timelog.csv";
            var time_dict = new Dictionary<string, TimerElement>();
            foreach (var timer in _timers)
            {
                if (timer.Code != null)
                {
                    if (!time_dict.ContainsKey(timer.Code))
                    {
                        time_dict.Add(timer.Code, timer);
                    }
                }

            }
            DateTime current_time = DateTime.Now;
            string todays_column = current_time.ToShortDateString();
            CSCSV.Table table = null;
            if (File.Exists(file))
            {
                table = CSCSV.Table.LoadFromFile(file);
            }
            else
            {
                table = new CSCSV.Table();
            }
            if (!table.ContainsHeader("Charge Codes"))
            {
                table.AddColumn("Charge Codes");
            }
            //If this table doesn't have a column for today, add one
            if (!table.ContainsHeader(todays_column))
            {
                table.AddColumn(todays_column);
            }
            List<string> chargecodes = table.GetColumn("Charge Codes").ToList();
            foreach (string code in time_dict.Keys)
            {
                int idx = chargecodes.IndexOf(code);
                //If the code is already in the list, set the time.
                if (idx >= 0)
                {
                    table.SetValue(todays_column, idx, time_dict[code].GetTime().ToString());
                }
                else // We will have to add the code..
                {
                    int row_idx = table.NewRow();
                    table.SetValue("Charge Codes", row_idx, code);
                    table.SetValue(todays_column, row_idx, time_dict[code].GetTime().ToString());
                }
            }
            table.WriteToFile(file);
        }
    }
}
