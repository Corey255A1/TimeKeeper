﻿//Corey Wunderlich WunderVision 2022
//A Class to store Hours/Minutes/Seconds
//Allows modification and calculates roll over into
//the next higher up time unit
//90seconds = 1minute 30seconds
using System;
using System.ComponentModel;

namespace TimeKeeper
{
    public enum ClockSections { HourL, HourR, MinuteL, MinuteR, SecondL, SecondR, AMPM }
    public class MutableTime : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyChange(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private int _hours = 0;
        public int Hours
        {
            get => _hours;
            set
            {
                _hours = value;
                NotifyChange(nameof(Hours));
                NotifyChange(nameof(IsPM));
            }
        }

        private int _minutes = 0;
        public int Minutes
        {
            get => _minutes;
            set
            {
                _minutes = value;
                NotifyChange(nameof(Minutes));
            }
        }

        private int _seconds = 0;
        public int Seconds
        {
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

        private bool _is_clock = false;
        public bool IsClock
        {
            get => _is_clock;
            set { _is_clock = value; NotifyChange(nameof(IsClock)); }
        }

        public static MutableTime FromDateTime(DateTime time) => new MutableTime(time);
        public static MutableTime FromDateTimeSpan(TimeSpan time_span) => new MutableTime(time_span);
        public static implicit operator MutableTime(DateTime time) => MutableTime.FromDateTime(time);
        public static implicit operator MutableTime(TimeSpan time_span) => MutableTime.FromDateTimeSpan(time_span);

        public static MutableTime AddTimes(MutableTime a, MutableTime b) => new MutableTime(a.ToTimeSpan() + b.ToTimeSpan());
        public static MutableTime SubtractTimes(MutableTime a, MutableTime b) => new MutableTime(a.ToTimeSpan() - b.ToTimeSpan());

        public static MutableTime operator +(MutableTime a, MutableTime b) => MutableTime.AddTimes(a, b);
        public static MutableTime operator -(MutableTime a, MutableTime b) => MutableTime.SubtractTimes(a, b);

        public MutableTime() { }
        public MutableTime(int hours, int minutes, int seconds)
        {
            Seconds = seconds;
            Minutes = minutes;
            Hours = hours;
        }
        public MutableTime(TimeSpan time_span)
        {
            SetTimeSpan(time_span);
        }
        public MutableTime(DateTime time)
        {
            SetDateTime(time);
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

        public void SetTimeSpan(TimeSpan time_span)
        {
            Seconds = time_span.Seconds + (int)Math.Round(time_span.Milliseconds / 1000.0);
            Minutes = time_span.Minutes;
            Hours = time_span.Hours;
        }

        public void SetDateTime(DateTime time)
        {
            Hours = time.Hour;
            Minutes = time.Minute;
            Seconds = time.Second + (int)Math.Round(time.Millisecond / 1000.0);
            IsClock = true;
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
            var new_time = ToTimeSpan() + time_span;
            SetTimeSpan(new_time);
        }
        public void DecrementTime(TimeSpan time_span)
        {
            var new_time = ToTimeSpan() - time_span;
            SetTimeSpan(new_time);
        }


    }
}
