//Corey Wunderlich 2018
//A combination of the clock numbers into a clock control
//Can be Timer Style or Clock Style

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace TimeKeeper
{

    public delegate void ClockModifiedEvent(Clock obj, int h, int m, int s);
    /// <summary>
    /// Interaction logic for Clock.xaml
    /// </summary>
    public partial class Clock : UserControl
    {
        public event ClockModifiedEvent ClockModified;
        private bool _is_clock_type = true;
        
        [Description("Timer or Clock?"), Category("Clock Data")]
        public bool IsAClock
        {
            get
            {
                return _is_clock_type;
            }
            set
            {
                _is_clock_type = value;
                if (_is_clock_type == false)
                {
                    apClk.Visibility = Visibility.Hidden;
                    mClk.Visibility = Visibility.Hidden;
                }
                else
                {
                    apClk.Visibility = Visibility.Visible;
                    mClk.Visibility = Visibility.Visible;
                }
            }
        }

        [Description("Digit Colors"), Category("Clock Data")]
        public Brush NumberColor
        {
            get { return (Brush)GetValue(NumberColorProperty); }
            set { SetValue(NumberColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NumberColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberColorProperty =
            DependencyProperty.Register("NumberColor", typeof(Brush), typeof(Clock), new PropertyMetadata(Brushes.Black));



        private bool _is_modifiable = true;
        [Description("Can this be modified?"), Category("Clock Data")]
        public bool IsModifiable
        {
            get { return _is_modifiable; }
            set
            {
                _is_modifiable = value;
                _clock_number_list.ForEach((t) => t.IsModifiable = _is_modifiable);
            }
        }

        //private MutableTime _time;
        //public MutableTime Time
        //{
        //    get => _time;
        //}

        private List<ClockNum> _clock_number_list;
        public Clock()
        {
            DataContext = this;
            InitializeComponent();

            //Make a list of the modifiable elements
            _clock_number_list = new List<ClockNum>()
            {
                hour1Clk,hour2Clk,minute1Clk,minute2Clk,second1Clk,second2Clk,apClk
            };

            //Connect all of the individual clock numbers controls into a clock
            hour2Clk.NumberRollOver += hour1Clk.IncrementNum;
            minute1Clk.NumberRollOver += hour2Clk.IncrementNum;
            minute2Clk.NumberRollOver += minute1Clk.IncrementNum;
            second1Clk.NumberRollOver += minute2Clk.IncrementNum;
            second2Clk.NumberRollOver += second1Clk.IncrementNum;

            foreach (var cn in _clock_number_list)
            {
                cn.NumberModified += ClockChanged;
            }



        }

        private void ClockChanged()
        {
            bool isPM = apClk.Number == ClockNumbers.P;
            int h = hour1Clk.GetInteger() * 10 + hour2Clk.GetInteger();
            if (isPM) h = h + 12;
            int m = minute1Clk.GetInteger() * 10 + minute2Clk.GetInteger();
            int s = second1Clk.GetInteger() * 10 + second2Clk.GetInteger();
            ClockModified?.Invoke(this, h, m, s);

        }

        public void SetTime(DateTime time)
        {
            int h = time.Hour;
            if (IsAClock)
            {

                //Not 24 Hour time.. maybe make it an option?
                if (h >= 12)
                {
                    if (h > 12) h = h - 12;
                    apClk.Number = ClockNumbers.P;
                }
                else
                {
                    apClk.Number = ClockNumbers.A;
                }
            }

            SetTime(h, time.Minute, time.Second);

        }
        public void SetTime(TimeSpan ts)
        {
            SetTime(ts.Hours, ts.Minutes, ts.Seconds);
        }

        public void SetTime(int h, int m, int s)
        {
            hour2Clk.SetNumber(h % 10);
            hour1Clk.SetNumber(h / 10);

            minute2Clk.SetNumber(m % 10);
            minute1Clk.SetNumber(m / 10);

            second2Clk.SetNumber(s % 10);
            second1Clk.SetNumber(s / 10);
        }
    }
}
