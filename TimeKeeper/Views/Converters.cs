﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace TimeKeeper
{
    public class ClockNumbersEnumToResourceConverter : IValueConverter
    {
        public DrawingBrush Num0 { get; set; }
        public DrawingBrush Num1 { get; set; }
        public DrawingBrush Num2 { get; set; }
        public DrawingBrush Num3 { get; set; }
        public DrawingBrush Num4 { get; set; }
        public DrawingBrush Num5 { get; set; }
        public DrawingBrush Num6 { get; set; }
        public DrawingBrush Num7 { get; set; }
        public DrawingBrush Num8 { get; set; }
        public DrawingBrush Num9 { get; set; }
        public DrawingBrush NumA { get; set; }
        public DrawingBrush NumP { get; set; }
        public DrawingBrush NumM { get; set; }
        public DrawingBrush NumCol { get; set; }


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ClockNumbers clock_enum)
            {
                switch (clock_enum)
                {
                    case ClockNumbers.Zero: return Num0;
                    case ClockNumbers.One: return Num1;
                    case ClockNumbers.Two: return Num2;
                    case ClockNumbers.Three: return Num3;
                    case ClockNumbers.Four: return Num4;
                    case ClockNumbers.Five: return Num5;
                    case ClockNumbers.Six: return Num6;
                    case ClockNumbers.Seven: return Num7;
                    case ClockNumbers.Eight: return Num8;
                    case ClockNumbers.Nine: return Num9;
                    case ClockNumbers.A: return NumA;
                    case ClockNumbers.P: return NumP;
                    case ClockNumbers.M: return NumM;
                    case ClockNumbers.Colon: return NumCol;
                }
            }

            return null;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}