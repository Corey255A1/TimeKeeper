﻿//Corey Wunderlich 2018
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

    public delegate void ClockModifiedEvent(int h, int m, int s);
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
            get { return (DateTime)GetValue(TimeProperty); }
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
            bool is_pm = apClk.Number == ClockNumbers.P;
            int h = hour1Clk.GetInteger() * 10 + hour2Clk.GetInteger();
            
            if (is_pm) h += 12;
            if (IsAClock)
            {
                if (h >= 24) h -= 24;
            }
            int m = minute1Clk.GetInteger() * 10 + minute2Clk.GetInteger();
            int s = second1Clk.GetInteger() * 10 + second2Clk.GetInteger();
            ClockModified?.Invoke(h, m, s);

        }

    }
}
