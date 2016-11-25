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
        private double lastKnowHue = 0;
        private double lastKnowSaturation = 0;
        private double lastKnowBrightness = 0;

        public LightSettings()
        {
            this.InitializeComponent();
        }

        private void GeneralSlider_OnDragLeave(object sender, DragEventArgs e)
        {
            //Todo update waarden op basis van switch
            string tag = ((Slider) sender).Tag.ToString();

            switch (tag)
            {
                case "Hue":
                {
                    lastKnowHue = ((Slider) sender).Value;
                    break;
                }
                case "Saturation":
                {
                    lastKnowSaturation = ((Slider)sender).Value;
                    break;
                }
                case "Brightness":
                {
                    lastKnowBrightness = ((Slider)sender).Value;
                    break;
                    }
            }
        }

        private void ConfirmButton_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO updaten van lamp met slider waarden
        }
    }
}
