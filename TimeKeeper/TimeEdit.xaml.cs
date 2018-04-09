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
using System.Text.RegularExpressions;
namespace TimeKeeper
{
    /// <summary>
    /// Interaction logic for TimeEdit.xaml
    /// </summary>
    public partial class TimeEdit : UserControl
    {
        Regex charFilter = new Regex("[^0-9.-]+");
        int hours;
        int minutes;
        int seconds;
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
            AddSeconds((int)ts.TotalSeconds);
        }

        public TimeSpan GetTime()
        {
            return new TimeSpan(hours, minutes, seconds);
        }


        private void numsOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (charFilter.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
        private void AddSeconds(int s)
        {
            SetSeconds(seconds + s);
        }
        private void SetSeconds(int s)
        {
            if (s > 59)
            {                
                SetMinutes(minutes + (s / 60));
                s = s % 60;
            }
            seconds = s;
            secondsBox.Text = s.ToString("D2");
        }
        private void SetMinutes(int m)
        {
            if(m>59)
            {                
                SetHours(hours + (m / 60));
                m = m % 60;
            }
            minutes = m;
            minutesBox.Text = m.ToString("D2");
        }
        private void SetHours(int h)
        {
            hours = h;
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
            if(e.Key == Key.Enter)
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
