using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using HueXamlApp.Annotations;
using HueXamlApp.Connector;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HueXamlApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LightSettings
    {
        private int _index;
        public int _lastKnowHue = 0;
        public int _lastKnowSaturation = 0;
        public int _lastKnowBrightness = 0;

        public LightSettings()
        {
            this.InitializeComponent();
        }

        private void GeneralSlider_OnDragLeave(object sender, DragEventArgs dragEventArgs)
        {
            //TODO: Make this a feminazi that triggers
            string tag = ((Slider) sender).Tag.ToString();
            Debug.WriteLine("yolo");

            switch (tag.ToLower())
            {
                case "hue":
                    _lastKnowHue = (int) ((Slider) sender).Value;
                    break;

                case "saturation":
                    _lastKnowSaturation = (int) ((Slider) sender).Value;
                    break;

                case "brightness":
                    _lastKnowBrightness = (int) ((Slider) sender).Value;
                    break;
            }

            Connection.Connector.ChangeLight(_index, new
            {
                hue = _lastKnowHue,
                sat = _lastKnowSaturation,
                bri = _lastKnowBrightness
            });
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var text = e.Parameter as string;

            if (text == null) return;
            if (!int.TryParse(text, out _index)) return;

            var lighty = HueConnector.Lights.ElementAt(_index);
            _index++;

            _lastKnowBrightness = lighty.V;
            _lastKnowHue = lighty.H;
            _lastKnowSaturation = lighty.S;

            SaturationSlider.Value = _lastKnowSaturation;
            HueSlider.Value = _lastKnowHue;
            BrightnessSlider.Value = _lastKnowBrightness;
            Toggle.IsOn = lighty.IsOn;
        }

        private void Toggle_OnToggled(object sender, RoutedEventArgs e)
        {
            Connection.Connector.ChangeLight(_index, new
            {
                on = Toggle.IsOn
            });
        }
    }
}
