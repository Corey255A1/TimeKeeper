﻿using System;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TimeTicker theTicker;
        public MainWindow()
        {
            InitializeComponent();
            theTicker = new TimeTicker();
            theTicker.TickEvent += Tick;

        }
        public void Tick(DateTime t)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                testTimeLbl.Text = t.ToLongTimeString();
            }));
            
        }
    }
}
