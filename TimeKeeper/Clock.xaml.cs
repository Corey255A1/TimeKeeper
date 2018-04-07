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

namespace TimeKeeper
{
    /// <summary>
    /// Interaction logic for Clock.xaml
    /// </summary>
    public partial class Clock : UserControl
    {
        public Clock()
        {
            InitializeComponent();
            theGrid.Background = Brushes.White;
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
            //Not 24 Hour time.. maybe make it an option?
            if (h > 12)
            {
                h = h - 12;
                apClk.MyNumber = ClockNumbers.P;
            }
            else
            {
                apClk.MyNumber = ClockNumbers.A;
            }
            hour2Clk.SetNumber(h % 10);
            hour1Clk.SetNumber(h / 10);

            int m = time.Minute;
            minute2Clk.SetNumber(m % 10);
            minute1Clk.SetNumber(m / 10);

            int s = time.Second;
            second2Clk.SetNumber(s % 10);
            second1Clk.SetNumber(s / 10);

        }
    }
}
