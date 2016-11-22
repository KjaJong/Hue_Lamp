﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HueXamlApp.Connector;
using HughRemote.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HueXamlApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    public sealed partial class Connection : Page
    {

        public static HueConnector Connector { get; set; }
        public Connection()
        {
            this.InitializeComponent();
        }

        private void ConnectionButton_OnClick(object sender, RoutedEventArgs e)
        {

            if (UsernameTextBox.Text == "") return;
            Connector = new HueConnector(IpTextBox.Text, UsernameTextBox.Text);
        }

        private void Party_OnClick(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            for (int i = 1; i <= 3; i++)
            {
                double r = random.Next(256);
                double g = random.Next(256);
                double b = random.Next(256);

                double h;
                double s;
                double v;
                
                ColorUtil.RGBtoHSV(r, g, b, out h, out s, out v);
                Connector.ChangeLight(i, new
                {
                    sat = 255,
                    bri = 255,
                    hue = h
                });
            }
        }
    }
}