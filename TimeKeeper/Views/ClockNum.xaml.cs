//Corey Wunderlich 2018
//A Single Clock Digit Control.
//Implemented this way so that it can use vector 
//rendered numbers rather than fonts
using System;
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
    public class ClockNumberChangedArgs
    {
        public ClockNum Clock { get; set; }
        public ClockNumbers NewValue { get; set; }
        public ClockNumbers OldValue { get; set; }
        public bool RolledOver { get; set; }
        public int ValueDelta { get; set; }
    }
    public delegate void ClockNumberChanged(ClockNumberChangedArgs changed_args);
    public partial class ClockNum : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyChange(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        public event ClockNumberChanged NumberRollOver;
        public event ClockNumberChanged NumberModified;

        public ClockNumbers Number
        {
            get { return (ClockNumbers)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        private ClockSections _clockSection = ClockSections.SecondR;

        public ClockSections ClockSection
        {
            get => _clockSection;
            set { _clockSection = value; NotifyChange(nameof(ClockSection)); }
        }


        // Using a DependencyProperty as the backing store for Number.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register("Number", typeof(ClockNumbers), typeof(ClockNum), new PropertyMetadata(ClockNumbers.Zero));



        private ClockNumbers _upperLimit = ClockNumbers.Nine;
        [Description("Set the Number Upper Limit"), Category("Clock Data")]
        public ClockNumbers NumberUpperLimit
        {
            get
            {
                return _upperLimit;
            }
            set
            {
                _upperLimit = value;
            }
        }

        private ClockNumbers _lowerLimit = ClockNumbers.Zero;
        [Description("Set the Number Lower Limit"), Category("Clock Data")]
        public ClockNumbers NumberLowerLimit
        {
            get
            {
                return _lowerLimit;
            }
            set
            {
                _lowerLimit = value;
            }
        }

        [Description("Digit Colors"), Category("Clock Data")]
        public Brush NumberColor
        {
            get { return (Brush)GetValue(NumberColorProperty); }
            set { SetValue(NumberColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NumberColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberColorProperty =
            DependencyProperty.Register("NumberColor", typeof(Brush), typeof(ClockNum), new PropertyMetadata(Brushes.Black));



        private bool _isModifiable = true;
        [Description("Can this be modified?"), Category("Clock Data")]
        public bool IsModifiable
        {
            get { return _isModifiable; }
            set { _isModifiable = value; }
        }

        public ClockNum()
        {
            DataContext = this;
            InitializeComponent();
        }
        public int GetInteger()
        {
            return (int)Number;
        }
        public void RollOver(ClockNum clock, ClockNumbers value)
        {
            IncrementNumber();
        }
        public void IncrementNumber()
        {
            var newNumber = Number + 1;
            bool hasRolledOver = false;
            if (!Enum.IsDefined(typeof(ClockNumbers), newNumber) || newNumber > _upperLimit)
            {
                newNumber = _lowerLimit;
                hasRolledOver = true;
            }
            var changedArgs = new ClockNumberChangedArgs()
            {
                Clock = this,
                OldValue = Number,
                NewValue = newNumber,
                ValueDelta = 1,
                RolledOver = hasRolledOver
            };
            NumberModified?.Invoke(changedArgs);
            if (hasRolledOver) NumberRollOver?.Invoke(changedArgs);
        }
        public void DecrementNumber()
        {
            var newNumber = Number - 1;
            if (!Enum.IsDefined(typeof(ClockNumbers), newNumber) || newNumber < _lowerLimit)
            {
                newNumber = _upperLimit;
            }
            var changed_args = new ClockNumberChangedArgs()
            {
                Clock = this,
                OldValue = Number,
                NewValue = newNumber,
                ValueDelta = -1,
                RolledOver = false
            };
            NumberModified?.Invoke(changed_args);
        }

        private void incrementButtonClicked(object sender, RoutedEventArgs e)
        {
            IncrementNumber();
        }

        private void decrementButtonClicked(object sender, RoutedEventArgs e)
        {
            DecrementNumber();
        }

        private void numberGridMouseEnter(object sender, MouseEventArgs e)
        {
            if (_isModifiable)
            {
                incBtn.Visibility = Visibility.Visible;
                decBtn.Visibility = Visibility.Visible;
            }
        }

        private void numberGridMouseLeave(object sender, MouseEventArgs e)
        {
            if (_isModifiable)
            {
                incBtn.Visibility = Visibility.Hidden;
                decBtn.Visibility = Visibility.Hidden;
            }
        }
    }
}
