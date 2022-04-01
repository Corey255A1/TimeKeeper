//Corey Wunderlich 2018
//A combination of the clock numbers into a clock control
//Can be Timer Style or Clock Style

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TimeKeeper.Utils;
namespace TimeKeeper
{

    public delegate void ClockModifiedEvent(int hour, int minute, int second);
    /// <summary>
    /// Interaction logic for Clock.xaml
    /// </summary>
    public partial class Clock : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyChange(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

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
                if (!_is_clock_type)
                {
                    apCol.Width = new GridLength(0);
                    mCol.Width = new GridLength(0);
                }
                else
                {
                    apCol.Width = new GridLength(20, GridUnitType.Star);
                    mCol.Width = new GridLength(20, GridUnitType.Star);
                }
                NotifyChange(nameof(IsAClock));
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



        public MutableTime Time
        {
            get { return (MutableTime)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Time.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(MutableTime), typeof(Clock), new PropertyMetadata(new MutableTime()));



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

            foreach (var cn in _clock_number_list)
            {
                cn.NumberModified += ClockChanged;
            }
        }

        private int CompareAndGetClockNumChange(ClockNum ctrl, ClockNum changed_ctrl, ClockNumbers changed_value, int current)
        {
            return ctrl == changed_ctrl ? (int)changed_value : current;
        }

        private int CombineDigits(int digit1, int digit2)
        {
            return digit1 * 10 + digit2;
        }

        private void ClockChanged(ClockNumberChangedArgs changed_args)
        {
            MutableTime new_time;
            MutableTime curr_time = Time;
            if (changed_args.Clock.ClockSection == ClockSections.AMPM)
            {
                //Modifying the AMPM is just cycling by 12 hours
                new_time = new MutableTime((Time.Hours + 12) % 24, Time.Minutes, Time.Seconds);
            }
            else
            {
                ClockDigitizer digitizer = new ClockDigitizer(Time);
                switch (changed_args.Clock.ClockSection)
                {
                    case ClockSections.HourL: digitizer.HourLeft.Number += changed_args.ValueDelta; break;
                    case ClockSections.HourR: digitizer.HourRight.Number += changed_args.ValueDelta; break;
                    case ClockSections.MinuteL: digitizer.MinuteLeft.Number += changed_args.ValueDelta; break;
                    case ClockSections.MinuteR: digitizer.MinuteRight.Number += changed_args.ValueDelta; break;
                    case ClockSections.SecondL: digitizer.SecondLeft.Number += changed_args.ValueDelta; break;
                    case ClockSections.SecondR: digitizer.SecondRight.Number += changed_args.ValueDelta; break;
                }
                new_time = digitizer.GetTime();
            }
            if (IsAClock)
            {
                new_time.Hours = new_time.Hours % 24;
            }
            ClockModified?.Invoke(new_time.Hours, new_time.Minutes, new_time.Seconds);

        }

    }
}
