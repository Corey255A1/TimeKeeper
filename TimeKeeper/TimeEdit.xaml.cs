//Corey Wunderlich
//A control that uses edit boxes to allow for quickly modifying a time element
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace TimeKeeper
{
    /// <summary>
    /// Interaction logic for TimeEdit.xaml
    /// </summary>
    public partial class TimeEdit : UserControl
    {
        private Regex _char_filter = new Regex("[^0-9.-]+");
        private int _hours;
        private int _minutes;
        private int _seconds;
        public TimeEdit()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            SetSeconds(0);
            SetHours(0);
            SetMinutes(0);
        }

        public void IncrementTime(TimeSpan ts)
        {
            AddSeconds((int)Math.Round(ts.TotalSeconds));
        }

        public TimeSpan GetTime()
        {
            return new TimeSpan(0, _hours, _minutes, (int)_seconds, (int)(_seconds - (int)_seconds) * 1000);
        }


        private void numsOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (_char_filter.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
        private void AddSeconds(int s)
        {
            SetSeconds(_seconds + s);
        }
        private void SetSeconds(int s)
        {
            if (s > 59)
            {
                SetMinutes(_minutes + (int)(s / 60));
                s = s % 60;
            }
            _seconds = s;
            secondsBox.Text = _seconds.ToString("D2");
        }
        private void SetMinutes(int m)
        {
            if (m > 59)
            {
                SetHours(_hours + (m / 60));
                m = m % 60;
            }
            _minutes = m;
            minutesBox.Text = m.ToString("D2");
        }
        private void SetHours(int h)
        {
            _hours = h;
            hoursBox.Text = h.ToString("D3");
        }

        private void ApplySeconds()
        {
            if (secondsBox == null) return;
            if (secondsBox.Text == "") return;
            int s = Convert.ToInt32(secondsBox.Text);
            SetSeconds(s);
        }
        private void ApplyMinutes()
        {
            if (minutesBox == null) return;
            if (minutesBox.Text == "") return;
            int m = Convert.ToInt32(minutesBox.Text);
            SetMinutes(m);
        }
        private void ApplyHours()
        {
            if (hoursBox == null) return;
            if (hoursBox.Text == "") return;
            int h = Convert.ToInt32(hoursBox.Text);
            SetHours(h);
        }

        private void secondsBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ApplySeconds();
        }

        private void hoursBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ApplyHours();
        }

        private void minutesBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ApplyMinutes();
        }

        private void secondsBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ApplySeconds();
            }
        }

        private void hoursBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ApplyHours();
            }
        }

        private void minutesBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ApplyMinutes();
            }
        }
    }
}
