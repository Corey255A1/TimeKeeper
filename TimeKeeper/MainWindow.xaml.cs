//Corey Wunderlich 2018
//The "Main" of the TimeKeeper.
//Ties all of the pieces of together and creates
//The user interface.
//
using System;
using System.IO;
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
            _controller = new TimeCardController(Dispatcher, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TimeCard.DefaultChargeCodeFileName));
            DataContext = Controller;
            InitializeComponent();
        }

        private void LoadLogButtonClicked(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "ChargeCodes",
                DefaultExt = TimeCard.ChargeCodeFileExtension,
                Filter = String.Format("Charge Codes (.{0})|*.{0}", TimeCard.ChargeCodeFileExtension)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _controller.TimeCard.Load(openFileDialog.FileName);
            }
        }

        private void SaveLogButtonClicked(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "ChargeCodes",
                DefaultExt = TimeCard.ChargeCodeFileExtension,
                Filter = String.Format("Charge Codes (.{0})|*.{0}", TimeCard.ChargeCodeFileExtension)
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                _controller.TimeCard.Save(saveFileDialog.FileName);
            }
        }

        private void LogButtonClicked(object sender, RoutedEventArgs e)
        {
            var file = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "timelog.csv");
            _controller.TimeCard.WriteCSV(file);
        }

        private void StartTimerModified(int hour, int minute, int second)
        {
            Controller.AdjustStartTime(hour, minute, second);
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TimeCard.DefaultChargeCodeFileName);
            _controller.TimeCard.Save(file);
        }

        private void MainWindowMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton != System.Windows.Input.MouseButton.Left) { return; }
            this.DragMove();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
