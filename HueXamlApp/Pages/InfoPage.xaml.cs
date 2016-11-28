using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HueXamlApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InfoPage : Page
    {
        public InfoPage()
        {
            this.InitializeComponent();
            HueIdBlock.Text = Connection.Connector.Adres;
            UsernameBlock.Text = Connection.Connector.FakeUsername;
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name.ToLower())
            {
                case "backbutton":
                    if(Frame.CanGoBack) Frame.GoBack();
                    break;
                default:
                    Debug.WriteLine("you're not suppose to be in here");
                    break;
            }
        }
    }
}
