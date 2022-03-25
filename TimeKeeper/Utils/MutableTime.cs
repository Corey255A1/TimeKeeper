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
    public class MutableTime : INotifyPropertyChanged
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
                NotifyChange(nameof(IsPM));
            }
        }

        private int _minutes = 0;
        public int Minutes {
            get => _minutes;
            set
            {
                _minutes = value;
                NotifyChange(nameof(Minutes));
            }
        }

        private int _seconds = 0;
        public int Seconds {
            get => _seconds;
            set
            {
                _seconds = value;
                NotifyChange(nameof(Seconds));
            }
        }

        public bool IsPM
        {
            get => Hours > 11;
        }

        public static MutableTime FromDateTime(DateTime time) => new MutableTime(time);
        public static MutableTime FromDateTimeSpan(TimeSpan time_span) => new MutableTime(time_span);
        public static implicit operator MutableTime(DateTime time) => MutableTime.FromDateTime(time);
        public static implicit operator MutableTime(TimeSpan time_span) => MutableTime.FromDateTimeSpan(time_span);

        public static MutableTime AddTimes(MutableTime a, MutableTime b) => new MutableTime(a.ToTimeSpan() + b.ToTimeSpan());
        public static MutableTime SubtractTimes(MutableTime a, MutableTime b) => new MutableTime(a.ToTimeSpan() - b.ToTimeSpan());

        public static MutableTime operator +(MutableTime a, MutableTime b) => MutableTime.AddTimes(a, b);
        public static MutableTime operator -(MutableTime a, MutableTime b) => MutableTime.SubtractTimes(a, b);

        public MutableTime(){}
        public MutableTime(int hours, int minutes, int seconds)
        {
            Seconds = seconds;
            Minutes = minutes;
            Hours = hours;
        }
        public MutableTime(TimeSpan time_span)
        {
            Seconds = time_span.Seconds;
            Minutes = time_span.Minutes;
            Hours = time_span.Hours;
        }
        public MutableTime(DateTime time)
        {
            Seconds = time.Second;
            Minutes = time.Minute;
            Hours = time.Hour;
        }
        public void Clear()
        {
            Seconds = 0;
            Minutes = 0;
            Hours = 0;
        }
        public TimeSpan ToTimeSpan()
        {
            return new TimeSpan(0, _hours, _minutes, _seconds, 0);
        }

        public void AddTime(MutableTime mut_time)
        {
            IncrementTime(mut_time.ToTimeSpan());
        }
        public void SubtractTime(MutableTime mut_time)
        {
            DecrementTime(mut_time.ToTimeSpan());
        }

        public void IncrementTime(TimeSpan time_span)
        {
            var newTime = ToTimeSpan() + time_span;
            Seconds = newTime.Seconds;
            Minutes = newTime.Minutes;
            Hours = newTime.Hours;
        }
        public void DecrementTime(TimeSpan time_span)
        {
            var newTime = ToTimeSpan() - time_span;
            Seconds = newTime.Seconds;
            Minutes = newTime.Minutes;
            Hours = newTime.Hours;
        }

        public void SetDateTime(DateTime time)
        {
            Hours = time.Hour;
            Minutes = time.Minute;
            Seconds = time.Second;
        }
    }
}
