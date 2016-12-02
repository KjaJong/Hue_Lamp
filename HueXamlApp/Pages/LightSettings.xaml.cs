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
using Windows.UI;
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
            HueSlider.Background = RainbowBrushAfterButton();
            HueSlider.Foreground = RainbowBrushBeforeButton();
        }

        private async void GeneralSlider_OnValueChanged(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            foreach (var i in _indexes)
            {
                await Connection.Connector.ChangeLight(i, new
                {
                    hue = (int)HueSlider.Value,
                    sat = (int)SaturationSlider.Value,
                    bri = (int)BrightnessSlider.Value
                });
            }

            var color = ColorUtil.HsvToRgb(HueSlider.Value, SaturationSlider.Value, BrightnessSlider.Value);
            Rektangle.Fill = new SolidColorBrush(color);
        }

        private async void Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Frame.CanGoBack) return;

            await Connection.Connector.GetLights();
            Frame.GoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var text = e.Parameter as string;

            if (text == null) return;

            var strings = text.Split(',');
            foreach (var s in strings)
            {
                int index;  
                if (!int.TryParse(s, out index)) continue;
                index++;
                _indexes.Add(index);
            }

            var lighty = HueConnector.Lights.ElementAt(_indexes.ElementAt(0) - 1);

            SaturationSlider.Value = lighty.S;
            HueSlider.Value = lighty.H;
            BrightnessSlider.Value = lighty.V;
            Toggle.IsOn = lighty.IsOn;

            var color = ColorUtil.HsvToRgb(lighty.H, lighty.S, lighty.V);
            Rektangle.Fill = new SolidColorBrush(color);
        }

        private async void Toggle_OnToggled(object sender, RoutedEventArgs e)
        {
            foreach (var i in _indexes)
            {
                await Connection.Connector.ChangeLight(i, new
                {
                    on = Toggle.IsOn
                });
            }
        }

        private LinearGradientBrush RainbowBrushAfterButton()
        {
            GradientStopCollection gsc = new GradientStopCollection();
            for (int i = 0; i < 100; i++)
            {
                gsc.Add(new GradientStop() {Color = ColorUtil.getColor(655.35*i, 254, 254), Offset = i/100.0});
            }
            return new LinearGradientBrush(gsc, 0);
        }

        private LinearGradientBrush RainbowBrushBeforeButton()
        {
            GradientStopCollection gsc = new GradientStopCollection();
            for (int i = 0; i < (int)HueSlider.Value; i++)
            {
                gsc.Add(new GradientStop() { Color = ColorUtil.getColor(655.35 * i, 254, 254), Offset = i / 100.0 });
            }
            return new LinearGradientBrush(gsc, 0);
        }
    }
}
