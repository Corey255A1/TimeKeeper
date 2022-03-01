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
    public partial class ClockNum : UserControl
    {
        public event ChangeEvent NumberRollOver;
        public event ChangeEvent NumberModified;
        private ClockNumbers _number;
        private ClockNumbers _upper_limit = ClockNumbers.Nine;
        private ClockNumbers _lower_limit = ClockNumbers.Zero;
        
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
                numGrid.Background = BrushDictionary[_number];
            }
        }
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
        
        private Brush _color;
        [Description("Set the Number Color"), Category("Clock Data")]
        public Brush Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                foreach (var n in BrushDictionary.Keys)
                {
                    ((GeometryDrawing)((DrawingGroup)BrushDictionary[n].Drawing).Children[0]).Brush = Color;
                }

            }
        }

        private bool _is_modifiable = true;
        [Description("Can this be modified?"), Category("Clock Data")]
        public bool IsModifiable
        {
            get { return _is_modifiable; }
            set { _is_modifiable = value; }
        }

        Dictionary<ClockNumbers, DrawingBrush> BrushDictionary;
        public ClockNum()
        {
            InitializeComponent();
            BrushDictionary = new Dictionary<ClockNumbers, DrawingBrush>(){
                { ClockNumbers.Zero,(DrawingBrush)this.FindResource("Num0") },
                { ClockNumbers.One,(DrawingBrush)this.FindResource("Num1") },
                { ClockNumbers.Two,(DrawingBrush)this.FindResource("Num2") },
                { ClockNumbers.Three,(DrawingBrush)this.FindResource("Num3") },
                { ClockNumbers.Four,(DrawingBrush)this.FindResource("Num4") },
                { ClockNumbers.Five,(DrawingBrush)this.FindResource("Num5") },
                { ClockNumbers.Six,(DrawingBrush)this.FindResource("Num6") },
                { ClockNumbers.Seven,(DrawingBrush)this.FindResource("Num7") },
                { ClockNumbers.Eight,(DrawingBrush)this.FindResource("Num8") },
                { ClockNumbers.Nine,(DrawingBrush)this.FindResource("Num9") },
                { ClockNumbers.A,(DrawingBrush)this.FindResource("NumA") },
                { ClockNumbers.P,(DrawingBrush)this.FindResource("NumP") },
                { ClockNumbers.M,(DrawingBrush)this.FindResource("NumM") },
                { ClockNumbers.Colon,(DrawingBrush)this.FindResource("NumCol") }
            };

            //numGrid.Background = BrushDictionary[ClockNumbers.Zero];

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
            numGrid.Background = BrushDictionary[Number];
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
            numGrid.Background = BrushDictionary[Number];
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
