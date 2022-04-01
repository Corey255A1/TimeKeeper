using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.Utils
{
    public delegate void DigitRollOverCallback();
    public class Digit
    {
        public static implicit operator Digit(int number) => new Digit(number, 9);
        public event DigitRollOverCallback DigitRollOver;
        private int _number = 0;
        public int Number {
            get => _number;
            set {
                _number = value;
                if(_number > Max)
                {
                    _number = 0;
                    DigitRollOver?.Invoke();
                }
                else if(_number < 0)
                {
                    _number = Max;
                }
            }
        }
        public int Max { get; private set; } = 9;
        public Digit(int number, int max)
        {
            Number = number;
            Max = max;
        }
        public void Increment()
        {
            Number = Number + 1;
        }

    }
    public class ClockDigitizer
    {
        public Digit HourLeft { get; private set; } = 0;
        public Digit HourRight { get; private set; } = 0;
        public Digit MinuteLeft { get; private set; } = new Digit(0, 5);
        public Digit MinuteRight { get; private set; } = 0;
        public Digit SecondLeft { get; private set; } = new Digit(0, 5);
        public Digit SecondRight { get; private set; } = 0;
        public ClockDigitizer()
        {
            SetRollOvers();
        }
        public ClockDigitizer(MutableTime time)
        {
            SetRollOvers();
            SecondRight.Number = time.Seconds % 10;
            SecondLeft.Number = time.Seconds / 10;
            MinuteRight.Number = time.Minutes % 10;
            MinuteLeft.Number = time.Minutes / 10;
            HourRight.Number = time.Hours % 10;
            HourLeft.Number = time.Hours / 10;
        }
        private void SetRollOvers()
        {
            SecondRight.DigitRollOver += SecondLeft.Increment;
            SecondLeft.DigitRollOver += MinuteRight.Increment;
            MinuteRight.DigitRollOver += MinuteLeft.Increment;
            MinuteLeft.DigitRollOver += HourRight.Increment;
            HourRight.DigitRollOver += HourLeft.Increment;
        }
        public MutableTime GetTime()
        {
            return new MutableTime(
                HourLeft.Number * 10 + HourRight.Number,
                MinuteLeft.Number * 10 + MinuteRight.Number,
                SecondLeft.Number * 10 + SecondRight.Number
                );
        }
    }
}
