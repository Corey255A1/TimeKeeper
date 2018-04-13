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

    public delegate void ClockModifiedEvent(Clock obj, int h, int m, int s);
    /// <summary>
    /// Interaction logic for Clock.xaml
    /// </summary>
    public partial class Clock : UserControl
    {
        public event ClockModifiedEvent ClockModified;
        bool isClockType = true;
        bool isModifiable = true;
        [Description("Timer or Clock?"), Category("Clock Data")]
        public bool IsAClock
        {
            get
            {
                return isClockType;
            }
            set
            {
                isClockType = value;
                if(isClockType==false)
                {
                    apClk.Visibility = Visibility.Hidden;
                    mClk.Visibility = Visibility.Hidden;
                }
                else
                {
                    apClk.Visibility = Visibility.Visible;
                    mClk.Visibility = Visibility.Visible;
                }
            }
        }

        Brush numberColor;
        [Description("Digit Colors"), Category("Clock Data")]
        public Brush NumberColor
        {
            get
            {
                return numberColor;
            }
            set
            {
                numberColor = value;
                foreach(var e in theGrid.Children)
                {
                   if (e.GetType() == typeof(ClockNum)) ((ClockNum)e).MyColor = numberColor;
                }
            }
        }
        [Description("Can this be modified?"), Category("Clock Data")]
        public bool IsModifiable
        {
            get { return isModifiable; }
            set { isModifiable = value;
            ClockNumberList.ForEach((t) => t.IsModifiable = isModifiable);
            }
        }

        List<ClockNum> ClockNumberList;
        public Clock()
        {
            InitializeComponent();
            
            //Make a list of the modifiable elements
            ClockNumberList = new List<ClockNum>()
            {
                hour1Clk,hour2Clk,minute1Clk,minute2Clk,second1Clk,second2Clk,apClk
            };

            //Connect all of the individual clock numbers controls into a clock
            hour2Clk.NumberRollOver += hour1Clk.IncrementNum;
            minute1Clk.NumberRollOver += hour2Clk.IncrementNum;
            minute2Clk.NumberRollOver += minute1Clk.IncrementNum;
            second1Clk.NumberRollOver += minute2Clk.IncrementNum;
            second2Clk.NumberRollOver += second1Clk.IncrementNum;

            foreach(var cn in ClockNumberList)
            {
                cn.NumberModified += ClockChanged;
            }

            

        }

        private void ClockChanged()
        {
            bool isPM = apClk.MyNumber == ClockNumbers.P;
            int h = hour1Clk.GetInteger() * 10 + hour2Clk.GetInteger();
            if (isPM) h = h + 12;
            int m = minute1Clk.GetInteger() * 10 + minute2Clk.GetInteger();
            int s = second1Clk.GetInteger() * 10 + second2Clk.GetInteger();
            ClockModified?.Invoke(this, h, m, s);

        }

        public void SetTime(DateTime time)
        {
            int h = time.Hour;
            if (IsAClock)
            {
                
                //Not 24 Hour time.. maybe make it an option?
                if (h >= 12)
                {
                    if(h>12) h = h - 12;
                    apClk.MyNumber = ClockNumbers.P;
                }
                else
                {
                    apClk.MyNumber = ClockNumbers.A;
                }
            }

            SetTime(h, time.Minute, time.Second);

        }
        public void SetTime(TimeSpan ts)
        {
            SetTime(ts.Hours, ts.Minutes, ts.Seconds);
        }

        public void SetTime(int h, int m, int s)
        {
            hour2Clk.SetNumber(h % 10);
            hour1Clk.SetNumber(h / 10);

            minute2Clk.SetNumber(m % 10);
            minute1Clk.SetNumber(m / 10);

            second2Clk.SetNumber(s % 10);
            second1Clk.SetNumber(s / 10);
        }
    }
}
