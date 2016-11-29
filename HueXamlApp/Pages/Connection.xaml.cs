using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            IpTextBox.Text = "http://145.48.205.33/api/iYrmsQq1wu5FxF9CPqpJCnm1GpPVylKBWDUsNDhB";
            //IpTextBox.Text = "http://localhost:80/api/";
        }

        private async void ConnectionButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (UsernameTextBox.Text == "") return;

            var thingy = (Button) sender;
            switch (thingy.Name.ToLower())
            {
                case "connectionbutton":
                    Connector = new HueConnector(IpTextBox.Text, UsernameTextBox.Text);
                    await Connector.Login();
                    break;
                default:
                    Debug.WriteLine("You're not suppose to be here");
                    break;
            }
            Frame.Navigate(typeof(LightsPage));
        }
    }
}
