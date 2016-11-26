using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
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
        private readonly List<int> _indexes;
        private int _lastSaturation;
        private int _lastHue;
        private int _lastBrightNess;

        public LightSettings()
        {
            this.InitializeComponent();
            _indexes = new List<int>();
            _lastBrightNess = 0;
            _lastHue = 0;
            _lastSaturation = 0;
        }

        private void GeneralSlider_OnDragLeave(object sender, RangeBaseValueChangedEventArgs rangeBaseValueChangedEventArgs)
        {
            //TODO: Make this a feminazi that triggers and doesn't update every fucking time
            Debug.WriteLine("Error");

            switch (((Slider) sender).Tag.ToString().ToLower())
            {
                case "hue":
                    _lastHue = (int)((Slider)sender).Value;
                    break;

                case "saturation":
                    _lastSaturation = (int)((Slider)sender).Value;
                    break;

                case "brightness":
                    _lastBrightNess = (int)((Slider) sender).Value;
                    break;
            }

            foreach (var i in _indexes)
            {
                Connection.Connector.ChangeLight(i, new
                {
                    hue = _lastHue,
                    sat = _lastSaturation,
                    bri = _lastBrightNess
                });
            }
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
            _indexes.Clear();
            var text = e.Parameter as string;

            if (text == null) return;

            var strings = text.ToCharArray();
            foreach (var s in strings)
            {
                int index;
                if (!int.TryParse(s.ToString(), out index)) return;
                index++;
                _indexes.Add(index);
            }

            if (_indexes.Count != 1) return;

            var lighty = HueConnector.Lights.ElementAt(0);

            _lastHue = lighty.H;
            _lastSaturation = lighty.S;
            _lastBrightNess = lighty.V;

            SaturationSlider.Value = _lastSaturation;
            HueSlider.Value = _lastHue;
            BrightnessSlider.Value = _lastBrightNess;
            Toggle.IsOn = lighty.IsOn;
        }

        private void Toggle_OnToggled(object sender, RoutedEventArgs e)
        {
            foreach (var i in _indexes)
            {
                Connection.Connector.ChangeLight(i, new
                {
                    on = Toggle.IsOn
                });
            }
        }
    }
}
