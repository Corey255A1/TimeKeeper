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

        private bool _isAClock = true;

        [Description("Timer or Clock?"), Category("Clock Data")]
        public bool IsAClock
        {
            get
            {
                return _isAClock;
            }
            set
            {
                _isAClock = value;
                if (!_isAClock)
                {
                    letterAorPColumn.Width = new GridLength(0);
                    letterMColumn.Width = new GridLength(0);
                }
                else
                {
                    letterAorPColumn.Width = new GridLength(20, GridUnitType.Star);
                    letterMColumn.Width = new GridLength(20, GridUnitType.Star);
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
                _clockNumberList.ForEach((t) => t.IsModifiable = _is_modifiable);
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



        private List<ClockNum> _clockNumberList;
        public Clock()
        {
            DataContext = this;
            InitializeComponent();

            //Make a list of the modifiable elements
            _clockNumberList = new List<ClockNum>()
            {
                hour1Clk,hour2Clk,minute1Clk,minute2Clk,second1Clk,second2Clk,apClk
            };

            foreach (var clockNumber in _clockNumberList)
            {
                clockNumber.NumberModified += ClockChanged;
            }
        }

        private void ClockChanged(ClockNumberChangedArgs changedArgs)
        {
            MutableTime newTime;
            MutableTime currentTime = Time;
            if (changedArgs.Clock.ClockSection == ClockSections.AMPM)
            {
                //Modifying the AMPM is just cycling by 12 hours
                newTime = new MutableTime((Time.Hours + 12) % 24, Time.Minutes, Time.Seconds);
            }
            else
            {
                ClockDigitizer digitizer = new ClockDigitizer(Time);
                switch (changedArgs.Clock.ClockSection)
                {
                    case ClockSections.HourL: digitizer.HourLeft.Number += changedArgs.ValueDelta; break;
                    case ClockSections.HourR: digitizer.HourRight.Number += changedArgs.ValueDelta; break;
                    case ClockSections.MinuteL: digitizer.MinuteLeft.Number += changedArgs.ValueDelta; break;
                    case ClockSections.MinuteR: digitizer.MinuteRight.Number += changedArgs.ValueDelta; break;
                    case ClockSections.SecondL: digitizer.SecondLeft.Number += changedArgs.ValueDelta; break;
                    case ClockSections.SecondR: digitizer.SecondRight.Number += changedArgs.ValueDelta; break;
                }
                newTime = digitizer.GetTime();
            }
            if (IsAClock)
            {
                newTime.Hours = newTime.Hours % 24;
            }
            ClockModified?.Invoke(newTime.Hours, newTime.Minutes, newTime.Seconds);

        }

    }
}
