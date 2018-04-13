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
using System.ComponentModel;
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
        private ClockNumbers myNumber;
        private ClockNumbers myUpperLimit = ClockNumbers.Nine;
        private ClockNumbers myLowerLimit = ClockNumbers.Zero;
        private bool isModifiable = true;
        [Description("Set the Current Number"), Category("Clock Data")]
        public ClockNumbers MyNumber
        {
            get {
                return myNumber;
            }
            set
            {
                myNumber = value;
                numGrid.Background = BrushDictionary[myNumber];
            }
        }
        [Description("Set the Number Upper Limit"), Category("Clock Data")]
        public ClockNumbers NumberUpperLimit
        {
            get
            {
                return myUpperLimit;
            }
            set
            {
                myUpperLimit = value;
            }
        }
        [Description("Set the Number Lower Limit"), Category("Clock Data")]
        public ClockNumbers NumberLowerLimit
        {
            get
            {
                return myLowerLimit;
            }
            set
            {
                myLowerLimit = value;
            }
        }
        Brush myColor;
        [Description("Set the Number Color"), Category("Clock Data")]
        public Brush MyColor
        {
            get
            {
                return myColor;
            }
            set
            {
                myColor = value;
                foreach (var n in BrushDictionary.Keys) {
                    ((GeometryDrawing)((DrawingGroup)BrushDictionary[n].Drawing).Children[0]).Brush = MyColor;
                }

            }
        }

        [Description("Can this be modified?"), Category("Clock Data")]
        public bool IsModifiable
        {
            get { return isModifiable; }
            set { isModifiable = value; }
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
            if(Enum.IsDefined(typeof(ClockNumbers), num))
            {
                MyNumber = (ClockNumbers)num;
            }
            else
            {
                MyNumber = ClockNumbers.Zero;
            }
        }
        public int GetInteger()
        {
            return (int)MyNumber;
        }
        public void IncrementNum()
        {
            var n = MyNumber + 1;
            if (Enum.IsDefined(typeof(ClockNumbers), n) && n<=myUpperLimit)
            {
                MyNumber = n;
            }
            else
            {
                MyNumber = myLowerLimit;
                NumberRollOver?.Invoke();
            }
            numGrid.Background = BrushDictionary[MyNumber];
            NumberModified?.Invoke();
        }
        public void DecrementNum()
        {
            var n = MyNumber - 1;
            if (Enum.IsDefined(typeof(ClockNumbers), n) && n >= myLowerLimit)
            {
                MyNumber = n;                
            }
            else
            {
                MyNumber = myUpperLimit;
            }
            numGrid.Background = BrushDictionary[MyNumber];
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
            if (isModifiable)
            {
                incBtn.Visibility = Visibility.Visible;
                decBtn.Visibility = Visibility.Visible;
            }
        }

        private void numGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isModifiable)
            {
                incBtn.Visibility = Visibility.Hidden;
                decBtn.Visibility = Visibility.Hidden;
            }
        }
    }
}
