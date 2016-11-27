using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    /// </summary>
    public sealed partial class LightsPage : Page
    {
        private bool _isListBoxSelected;
        private bool _partyAllowed;

        public LightsPage()
        {
            this.InitializeComponent();
            MyListBox.ItemsSource = HueConnector.Lights;
            UserBlock.Text = Connection.Connector.FakeUsername;
            var partyTimer = DefineTimer();
            partyTimer.Start();
            _partyAllowed = false;
        }

        private async void Button_OnClick(object sender, RoutedEventArgs e)
        {
            var thingy = (Button) sender;
            switch (thingy.Tag.ToString().ToLower())
            {
                case "settings":
                    if (!_isListBoxSelected) return;

                    var lighties = MyListBox.SelectedItems;
                    var index = "";

                    foreach (var lighty in lighties)
                    {
                      index += HueConnector.Lights.IndexOf((Light)lighty) + ",";
                    }
                    Frame.Navigate(typeof(LightSettings), index);
                    
                    break;

                case "back":
                    if (Frame.CanGoBack)
                    {
                        Frame.GoBack();
                    }
                    break;

                case "refresh":
                    await Connection.Connector.GetLights();
                    break;

                case "party":
                    _partyAllowed = !_partyAllowed;
                    break;

                default:
                    Debug.WriteLine("You're not suposse to be here.");
                    break;
            }
        }

        private void MyListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _isListBoxSelected = true;
        }

        private void Party()
        {
            var rnd = new Random();

            for (var i = 1; i <= HueConnector.Lights.Count; i++) 
            {
                var newValues = new List<int> {rnd.Next(65535), rnd.Next(255), rnd.Next(255)};

                Connection.Connector.ChangeLight(i, new
                {
                    hue = newValues[0],
                    sat = newValues[1],
                    bri = newValues[2]
                });

            }
        }

        private DispatcherTimer DefineTimer()
        {
            DispatcherTimer t = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 1)};//Sets a two second timer
            t.Tick += (s, e) => //Sets the tick event that goes of after every interval
            {
                if(_partyAllowed) { Party();}
                Debug.WriteLine("TICK TOCK MOTHAFOCKA");
            };
            return t;
        }
    }
}
