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
        private ObservableCollection<Light> _lights;
        private bool IsListBoxSelected;

        public LightsPage()
        {
            this.InitializeComponent();
            _lights = HueConnector.Lights;
            MyListBox.ItemsSource = _lights;
            //UserBlock.Text = Connection.Connector.Username;
            //LampBlock.Text = String.Empty;
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            var thingy = (Button) sender;
            switch (thingy.Tag.ToString().ToLower())
            {
                case "settings":
                    if (!IsListBoxSelected) return;

                    var lighty = (Light) MyListBox.SelectedItem;
                    var index = HueConnector.Lights.IndexOf(lighty).ToString();

                    Frame.Navigate(typeof(LightSettings), index);
                    
                    break;
                case "back":
                    if (Frame.CanGoBack)
                    {
                        Frame.GoBack();
                    }
                    break;
                default:
                    Debug.WriteLine("You're not suposse to be here.");
                    break;
            }
        }

        private void MyListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsListBoxSelected = true;
        }
    }
}
