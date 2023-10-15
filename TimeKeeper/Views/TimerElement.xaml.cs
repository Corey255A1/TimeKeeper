//Corey Wunderlich 2018
//The Timer Element allows for labeling, describing and displaying 
//an editable time for a charge code.
//These are what gets added to the list.
using System;
using System.Windows;
using System.Windows.Controls;

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

        public TimerElement()
        {
            Initialize();
        }
        public TimerElement(string code, string description)
        {
            Initialize();
        }
        private void Initialize()
        {
            InitializeComponent();
        }

        public void Clear()
        {
        }
        public void SetTime(DateTime t)
        {
        }
        public TimeSpan GetTime()
        {
            return new TimeSpan();
        }

        private void WorkOnBtnClick(object sender, RoutedEventArgs e)
        {
            WorkOn?.Invoke(this, null);
        }

        private void RemoveBtnClick(object sender, RoutedEventArgs e)
        {
            Remove?.Invoke(this, null);
        }
    }
}
