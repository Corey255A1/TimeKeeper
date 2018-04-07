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
    public delegate void ClockNumberRollOverCallback();
    public partial class ClockNum : UserControl
    {
        public event ClockNumberRollOverCallback ClockNumberRollOver;
        public ClockNumbers myNumber;
        public ClockNumbers myLimit;
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
        [Description("Set the Current Number"), Category("Clock Data")]
        public ClockNumbers NumberLimit
        {
            get
            {
                return myLimit;
            }
            set
            {
                myLimit = value;
            }
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
        public void IncrementNum()
        {
            if (Enum.IsDefined(typeof(ClockNumbers), MyNumber+1))
            {
                MyNumber = MyNumber + 1;
                if(MyNumber > myLimit)
                {
                    MyNumber = ClockNumbers.Zero;
                    ClockNumberRollOver?.Invoke();
                }
                numGrid.Background = BrushDictionary[MyNumber];
            }
        }

    }
}
