//Corey Wunderlich 2018
//The "Main" of the TimeKeeper.
//Ties all of the pieces of together and creates
//The user interface.
//
using System;
using System.Windows;

using TimeKeeper.Models;
namespace TimeKeeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TimeCardController _controller;
        public TimeCardController Controller
        {
            get => _controller;
        }

        public MainWindow()
        {
            _controller = new TimeCardController(Dispatcher, System.AppDomain.CurrentDomain.BaseDirectory + "\\default.chg");
            DataContext = Controller;
            InitializeComponent();
        }

        private void LoadLogButton_Clicked(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "ChargeCodes",
                DefaultExt = "chg",
                Filter = "Charge Codes (.chg)|*.chg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _controller.TimeCard.Load(openFileDialog.FileName);
            }
        }

        private void SaveLogButton_Clicked(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "ChargeCodes",
                DefaultExt = "chg",
                Filter = "Charge Codes (.chg)|*.chg"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                _controller.TimeCard.Save(saveFileDialog.FileName);
            }
        }

        private void LogBtn_Clicked(object sender, RoutedEventArgs e)
        {
            var file = System.AppDomain.CurrentDomain.BaseDirectory + "\\timelog.csv";
            _controller.TimeCard.WriteCSV(file);
        }

        private void StartTimerModified(int hour, int minute, int second)
        {
            Controller.AdjustStartTime(hour, minute, second);
        }


        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var file = System.AppDomain.CurrentDomain.BaseDirectory + "\\default.chg";
            _controller.TimeCard.Save(file);
        }

        private void MainWindowMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(e.ChangedButton != System.Windows.Input.MouseButton.Left) { return; }
            this.DragMove();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
