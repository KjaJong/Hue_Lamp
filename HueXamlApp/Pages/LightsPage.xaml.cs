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
        private readonly ObservableCollection<Light> _lights;
        private bool _isListBoxSelected;

        public LightsPage()
        {
            this.InitializeComponent();
            _lights = HueConnector.Lights;
            MyListBox.ItemsSource = _lights;
            UserBlock.Text = Connection.Connector.FakeUsername;
        }

        private async void Button_OnClick(object sender, RoutedEventArgs e)
        {
            var thingy = (Button) sender;
            switch (thingy.Tag.ToString().ToLower())
            {
                case "settings":
                    if (!_isListBoxSelected) return;

                    var lighties = MyListBox.SelectedItems;
                    string index = "";

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
                    Party();
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
            Random rnd = new Random();
            List<int> newValues = new List<int>(3);

            for (int i = 0; i < 20 /*Now a magic number, dunno if it should be anything else though.*/; i++)
            {
                newValues[0] = rnd.Next(65535);
                newValues[1] = rnd.Next(255);
                newValues[2] = rnd.Next(255);

                foreach (var l in _lights)
                {
                    Connection.Connector.ChangeLight(i, new
                    {
                        hue = newValues[0],
                        sat = newValues[1],
                        bri = newValues[2]
                    });
                }
            }
        }
    }
}
