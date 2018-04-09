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
    /// Interaction logic for TimerElement.xaml
    /// </summary>
    public enum TimerElementActionEnum { WorkOn, Pause, Remove, Edit }
    public delegate void TimerElementAction(TimerElement t, TimerElementActionEnum e);
    public partial class TimerElement : UserControl
    {
        public event TimerElementAction TimerActionPerformed;
        DateTime LastTimeReceived;
        bool bHasStarted = false;
        public TimerElement()
        {
            InitializeComponent();
            timerEdit.Clear();
        }

        public void Clear()
        {
            timerEdit.Clear();
        }
        public void SetTime(DateTime t)
        {
            if(!bHasStarted) { bHasStarted = true;  LastTimeReceived = t; return; }
            timerEdit.IncrementTime(t - LastTimeReceived);
            LastTimeReceived = t;
        }
        public TimeSpan GetTime()
        {
            return timerEdit.GetTime();
        }
        public void Pause()
        {
            bHasStarted = false;
        }

        private void workOnBtn_Click(object sender, RoutedEventArgs e)
        {
            TimerActionPerformed?.Invoke(this, TimerElementActionEnum.WorkOn);
        }

        private void pauseBtn_Click(object sender, RoutedEventArgs e)
        {
            TimerActionPerformed?.Invoke(this, TimerElementActionEnum.Pause);
            bHasStarted = false;
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            TimerActionPerformed?.Invoke(this, TimerElementActionEnum.Remove);

        }
    }
}
