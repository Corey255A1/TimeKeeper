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
    /// Interaction logic for Clock.xaml
    /// </summary>
    public partial class Clock : UserControl
    {        
        bool isClockType = true;

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

        public Clock()
        {
            InitializeComponent();
            //Not sure if this is really even needed.
            hour2Clk.ClockNumberRollOver += hour1Clk.IncrementNum;
            minute1Clk.ClockNumberRollOver += hour2Clk.IncrementNum;
            minute2Clk.ClockNumberRollOver += minute1Clk.IncrementNum;
            second1Clk.ClockNumberRollOver += minute2Clk.IncrementNum;
            second2Clk.ClockNumberRollOver += second1Clk.IncrementNum;
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
