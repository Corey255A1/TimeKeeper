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

        public string Code
        {
            get { return chargeCodeField.Text; }
        }
        public string Description
        {
            get { return descriptionField.Text; }
        }

        public TimerElement()
        {
            initialize();
        }
        public TimerElement(string code, string description)
        {
            initialize();
            chargeCodeField.Text = code;
            descriptionField.Text = description;
        }
        private void initialize()
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
            timerEdit.IncrementTime(t - LastTimeReceived);
            LastTimeReceived = t;
        }
        public TimeSpan GetTime()
        {
            return timerEdit.GetTime();
        }

        private void workOnBtn_Click(object sender, RoutedEventArgs e)
        {
            //Set my last time received to Right now when the button was clicked. Start Time from now
            var t = DateTime.Now;
            LastTimeReceived = new DateTime(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second); //truncate off any milliseconds
            TimerActionPerformed?.Invoke(this, TimerElementActionEnum.WorkOn);
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            TimerActionPerformed?.Invoke(this, TimerElementActionEnum.Remove);

        }
    }
}
