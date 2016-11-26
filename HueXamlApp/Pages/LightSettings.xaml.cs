using System;
using System.Collections.Generic;
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
    public sealed partial class LightSettings : Page
    {
        private double _lastKnowHue = 0;
        private double _lastKnowSaturation = 0;
        private double _lastKnowBrightness = 0;

        public LightSettings()
        {
            this.InitializeComponent();
        }

        private void GeneralSlider_OnDragLeave(object sender, DragEventArgs e)
        {
            //TODO update naar lamp sturen
            string tag = ((Slider) sender).Tag.ToString();

            switch (tag)
            {
                case "Hue":
                    _lastKnowHue = ((Slider) sender).Value;
                    break;

                case "Saturation":
                    _lastKnowSaturation = ((Slider)sender).Value;
                    break;

                case "Brightness":
                    _lastKnowBrightness = ((Slider)sender).Value;
                    break;
            }
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
