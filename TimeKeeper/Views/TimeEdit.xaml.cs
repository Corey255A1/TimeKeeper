//Corey Wunderlich
//A control that uses edit boxes to allow for quickly modifying a time element
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
namespace TimeKeeper
{
    /// <summary>
    /// Interaction logic for TimeEdit.xaml
    /// </summary>
    public partial class TimeEdit : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyChange(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        //For filtering out non positive integer characters
        private readonly Regex _integerRegex = new Regex("[^0-9]+");

        public MutableTime Time
        {
            get { return (MutableTime)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Time.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(MutableTime), typeof(TimeEdit), new PropertyMetadata(new MutableTime()));


        public TimeEdit()
        {
            InitializeComponent();
        }

        //Filter out non integer entries
        private void NumbersOnlyPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (_integerRegex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        //Apply the time modifications when the Enter key is pressed
        private void TextBoxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var textbox = sender as TextBox;
                DependencyProperty property = TextBox.TextProperty;
                BindingExpression binding = BindingOperations.GetBindingExpression(textbox, property);
                if (binding != null)
                {
                    binding.UpdateSource();
                }
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(textbox), null);
                Keyboard.ClearFocus();
            }
        }
    }
}
