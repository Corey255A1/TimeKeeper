//Corey Wunderlich 2018
//A Single Clock Digit Control.
//Implemented this way so that it can use vector 
//rendered numbers rather than fonts
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace TimeKeeper
{
    /// <summary>
    /// Interaction logic for ClockNum.xaml
    /// </summary>
    public enum ClockNumbers { Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, A, P, M, Colon };
    public delegate void ChangeEvent();
    public partial class ClockNum : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyChange(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        public event ChangeEvent NumberRollOver;
        public event ChangeEvent NumberModified;

        private ClockNumbers _number;
        [Description("Set the Current Number"), Category("Clock Data")]
        public ClockNumbers Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
                NotifyChange(nameof(Number));
            }
        }

        private ClockNumbers _upper_limit = ClockNumbers.Nine;
        [Description("Set the Number Upper Limit"), Category("Clock Data")]
        public ClockNumbers NumberUpperLimit
        {
            get
            {
                return _upper_limit;
            }
            set
            {
                _upper_limit = value;
            }
        }

        private ClockNumbers _lower_limit = ClockNumbers.Zero;
        [Description("Set the Number Lower Limit"), Category("Clock Data")]
        public ClockNumbers NumberLowerLimit
        {
            get
            {
                return _lower_limit;
            }
            set
            {
                _lower_limit = value;
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
            DependencyProperty.Register("NumberColor", typeof(Brush), typeof(ClockNum), new PropertyMetadata(Brushes.Black));



        private bool _is_modifiable = true;
        [Description("Can this be modified?"), Category("Clock Data")]
        public bool IsModifiable
        {
            get { return _is_modifiable; }
            set { _is_modifiable = value; }
        }

        public ClockNum()
        {
            DataContext = this;
            InitializeComponent();
        }
        public void SetNumber(int num)
        {
            if (Enum.IsDefined(typeof(ClockNumbers), num))
            {
                Number = (ClockNumbers)num;
            }
            else
            {
                Number = ClockNumbers.Zero;
            }
        }
        public int GetInteger()
        {
            return (int)Number;
        }
        public void IncrementNum()
        {
            var n = Number + 1;
            if (Enum.IsDefined(typeof(ClockNumbers), n) && n <= _upper_limit)
            {
                Number = n;
            }
            else
            {
                Number = _lower_limit;
                NumberRollOver?.Invoke();
            }
            NumberModified?.Invoke();
        }
        public void DecrementNum()
        {
            var n = Number - 1;
            if (Enum.IsDefined(typeof(ClockNumbers), n) && n >= _lower_limit)
            {
                Number = n;
            }
            else
            {
                Number = _upper_limit;
            }
            NumberModified?.Invoke();
        }

        private void incBtn_Click(object sender, RoutedEventArgs e)
        {
            IncrementNum();
        }

        private void decBtn_Click(object sender, RoutedEventArgs e)
        {
            DecrementNum();
        }

        private void numGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_is_modifiable)
            {
                incBtn.Visibility = Visibility.Visible;
                decBtn.Visibility = Visibility.Visible;
            }
        }

        private void numGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_is_modifiable)
            {
                incBtn.Visibility = Visibility.Hidden;
                decBtn.Visibility = Visibility.Hidden;
            }
        }
    }
}
