//Corey Wunderlich WunderVision 2022
//Converts to control how certain values are displayed
using System;
using System.Globalization;
using System.Windows;
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



    public class BoolToVisibility : IValueConverter
    {
        public bool Not { get; set; } = false;
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool visibile)
            {
                return (visibile && !Not) ? Visibility.Visible : Visibility.Hidden;
            }

            return Visibility.Hidden;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility visibile)
            {
                return (Visibility.Visible == visibile) && !Not;
            }

            return false;
        }
    }

    public class BoolToColor : IValueConverter
    {
        public SolidColorBrush TrueColor { get; set; }
        public SolidColorBrush FalseColor { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool colorBool && colorBool) ? TrueColor : FalseColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    public class DateTimeToClockNumDigit : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value is MutableTime time)
            {
                ClockSections section = (ClockSections)parameter;
                var hour = time.Hours;
                if (time.IsClock)
                {
                    if (time.IsPM)
                    {
                        //12:xxPM
                        if (hour != 12)
                            hour -= 12;
                    }
                    else
                    {
                        //12:xxAM
                        if (hour == 0) hour = 12;
                    }
                }
                switch (section)
                {
                    case ClockSections.HourL: return (ClockNumbers)(hour / 10);
                    case ClockSections.HourR: return (ClockNumbers)(hour % 10);
                    case ClockSections.MinuteL: return (ClockNumbers)(time.Minutes / 10);
                    case ClockSections.MinuteR: return (ClockNumbers)(time.Minutes % 10);
                    case ClockSections.SecondL: return (ClockNumbers)(time.Seconds / 10);
                    case ClockSections.SecondR: return (ClockNumbers)(time.Seconds % 10);
                    case ClockSections.AMPM: return time.IsPM ? ClockNumbers.P : ClockNumbers.A;
                }
            }

            return ClockNumbers.Zero;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
