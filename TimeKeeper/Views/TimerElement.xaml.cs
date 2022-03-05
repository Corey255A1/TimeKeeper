//Corey Wunderlich 2018
//The Timer Element allows for labeling, describing and displaying 
//an editable time for a charge code.
//These are what gets added to the list.
using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace TimeKeeper
{
    /// <summary>
    /// Interaction logic for TimerElement.xaml
    /// </summary>
    public enum TimerElementActionEnum { WorkOn, Pause, Remove, Edit }
    public delegate void TimerElementAction(TimerElement t, TimerElementActionEnum e);
    public partial class TimerElement : UserControl
    {
        public event EventHandler Remove;
        public event EventHandler WorkOn;

        //public event TimerElementAction TimerActionPerformed;
        //public event PropertyChangedEventHandler PropertyChanged;
        //private void NotifyChange(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        //private DateTime _last_time_received;

        //private string _code;
        //public string Code
        //{
        //    get => _code;
        //    set { _code = value; NotifyChange(nameof(Code)); }
        //}

        //private string _description;
        //public string Description
        //{
        //    get => _description;
        //    set { _description = value; NotifyChange(nameof(Description)); }
        //}

        //public MutableTime Time
        //{
        //    get { return (MutableTime)GetValue(TimeProperty); }
        //    set { SetValue(TimeProperty, value); }
        //}

        // Using a DependencyProperty as the backing store for Time.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty TimeProperty =
        //    DependencyProperty.Register("Time", typeof(MutableTime), typeof(TimeEdit), new PropertyMetadata(new MutableTime()));

        public TimerElement()
        {
            Initialize();
        }
        public TimerElement(string code, string description)
        {
            //_code = code;
            //_description = description;
            Initialize();
        }
        private void Initialize()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            //Time.Clear();
        }
        public void SetTime(DateTime t)
        {
           // Time.IncrementTime(t - _last_time_received);
           // _last_time_received = t;
        }
        public TimeSpan GetTime()
        {
            return new TimeSpan();
        }

        private void workOnBtn_Click(object sender, RoutedEventArgs e)
        {
            WorkOn?.Invoke(this, null);
            //Set my last time received to Right now when the button was clicked. Start Time from now
          //  var t = DateTime.Now;
          //  _last_time_received = new DateTime(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second); //truncate off any milliseconds
          //  TimerActionPerformed?.Invoke(this, TimerElementActionEnum.WorkOn);
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            Remove?.Invoke(this, null);
           // TimerActionPerformed?.Invoke(this, TimerElementActionEnum.Remove);

        }
    }
}
