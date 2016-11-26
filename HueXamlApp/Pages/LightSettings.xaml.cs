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

        public LightSettings()
        {
            this.InitializeComponent();
            _indexes = new List<int>();
        }

        private void GeneralSlider_OnDragLeave(object sender, RangeBaseValueChangedEventArgs rangeBaseValueChangedEventArgs)
        {
            //TODO: Make this a feminazi that triggers and doesn't update every fucking time
            switch (((Slider) sender).Tag.ToString().ToLower())
            {
                case "hue":
                    foreach (var i in _indexes)
                    {
                        Connection.Connector.ChangeLight(i, new
                        {
                            hue = (int) ((Slider) sender).Value
                        });
                    }
                    break;

                case "saturation":
                    foreach (var i in _indexes)
                    {
                        Connection.Connector.ChangeLight(i, new
                        {
                            hue = (int) ((Slider) sender).Value
                        });
                    }
                    break;

                case "brightness":
                    foreach (var i in _indexes)
                    {
                        Connection.Connector.ChangeLight(i, new
                        {
                            hue = (int) ((Slider) sender).Value
                        });
                    }
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

            SaturationSlider.Value = lighty.S;
            HueSlider.Value = lighty.H;
            BrightnessSlider.Value = lighty.V;
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
