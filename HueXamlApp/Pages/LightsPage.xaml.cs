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
        private readonly DispatcherTimer _partyTimer;
        private bool _partyAllowed;

        public LightsPage()
        {
            this.InitializeComponent();
            MyListBox.ItemsSource = HueConnector.Lights;
            UserBlock.Text = Connection.Connector.FakeUsername;
            _partyTimer = DefineTimer();
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

                case "info":
                    Frame.Navigate(typeof(InfoPage));
                    break;

                case "party":
                    _partyAllowed = !_partyAllowed;

                    if (!_partyAllowed)
                    {
                        _partyTimer.Stop();
                        await Connection.Connector.GetLights();
                    }
                    else _partyTimer.Start();
                    break;

                case "name":
                    for (var i = 1; i <= HueConnector.Lights.Count; i++)
                    {
                        await Connection.Connector.ChangeNameLight(i, new
                        {
                            name = RandomName()
                        });
                    }
                    await Connection.Connector.GetLights();
                    break;

                default:
                    Debug.WriteLine("You're not suposse to be here.");
                    break;
            }
        }

        private void MyListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _isListBoxSelected = MyListBox.SelectedItems.Count != 0;
        }

        private async void Party()
        {
            var rnd = new Random();

            for (var i = 1; i <= HueConnector.Lights.Count; i++) 
            {
                var newValues = new List<int> {rnd.Next(65535)};

                await Connection.Connector.ChangeLight(i, new
                {
                    hue = newValues[0],
                    sat = 254,
                    bri = 254
                });

            }
        }

        private DispatcherTimer DefineTimer()
        {
            var t = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 1)};//Sets a two second timer
            t.Tick += (s, e) => //Sets the tick event that goes of after every interval
            {
                Party();
            };
            return t;
        }

        private void MyListBox_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var lighties = MyListBox.SelectedItems;

            if (lighties.Count <= HueConnector.Lights.Count/2) MyListBox.SelectAll();
            else MyListBox.SelectedIndex = -1;
        }

        private static string RandomName()
        {
            var rnd = new Random();

            string[] strings = {
                "#BlameBart",
                "#HomuraDidNothingWring",
                "#GrillTheHam",
                " ( ͡° ͜ʖ ͡°)"};

            var text = strings[rnd.Next(strings.Length)];
            return text;
        }
    }
}
