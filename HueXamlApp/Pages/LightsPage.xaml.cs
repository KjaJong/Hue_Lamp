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

        public LightsPage()
        {
            this.InitializeComponent();
            _lights = HueConnector.Lights;
            MyListBox.ItemsSource = _lights;
            UserBlock.Text = Connection.Connector.Username;
            LampBlock.Text = String.Empty;
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO add switch with all buttons
            //if (Frame.CanGoBack)
            //{
            //    Frame.GoBack();
            //}
        }

        private void AddidButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MyListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<object> selection = new List<object>(((ListBox) sender).SelectedItems);
            Light temp = (Light)selection.ElementAt(0);
            LampBlock.Text = temp.Id;
        }
    }
}
