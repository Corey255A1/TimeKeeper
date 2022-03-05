//CWunderlich 2022
//A Class to store Hours/Minutes/Seconds
//Allows modification and calculates roll over into
//the next higher up time unit
//90seconds = 1minute 30seconds
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper
{
    public class MutableTime: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyChange(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private int _hours = 0;
        public int Hours {
            get => _hours;
            set
            {
                _hours = value;
                NotifyChange(nameof(Hours));
            }
        }

        private int _minutes = 0;
        public int Minutes {
            get => _minutes;
            set
            {
                int m = value;
                if (m > 59)
                {
                    Hours = _hours + (m / 60);
                    m = m % 60;
                }
                _minutes = m;
                NotifyChange(nameof(Minutes));
            }
        }

        private int _seconds = 0;
        public int Seconds {
            get => _seconds;
            set
            {
                int s = value;
                if (s > 59)
                {
                    Minutes = _minutes + (int)(s / 60);
                    s = s % 60;
                }
                _seconds = s;
                NotifyChange(nameof(Seconds));
            }
        }

        public MutableTime(){}
        public MutableTime(int hours, int minutes, int seconds)
        {
            Seconds = seconds;
            Minutes = minutes;
            Hours = hours;
        }
        public void Clear()
        {
            Seconds = 0;
            Minutes = 0;
            Hours = 0;
        }
        public TimeSpan GetTime()
        {
            return new TimeSpan(0, _hours, _minutes, (int)_seconds, (int)(_seconds - (int)_seconds) * 1000);
        }
        public void IncrementTime(TimeSpan time_span)
        {
            Seconds += (int)Math.Round(time_span.TotalSeconds);
        }
    }
}
