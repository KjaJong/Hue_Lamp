﻿using System;
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
        private int _changeAmount;
        private readonly DispatcherTimer _changeTimer;

        public LightSettings()
        {
            this.InitializeComponent();
            _indexes = new List<int>();
            _changeAmount = 0;
            _changeTimer = DefineTimer();
            UserBlock.Text = Connection.Connector.FakeUsername;
            //HueSlider.Background = ColorGradient();
        }

        private DispatcherTimer DefineTimer()
        {
            DispatcherTimer t = new DispatcherTimer();
            t.Interval = new TimeSpan(0, 0, 0, 5); //Sets a five second timer
            t.Tick += (s, e) => //Sets the tick event that goes of after every interval
            {
                UpdateLamp();
                Debug.WriteLine("TICK TOCK MOTHAFOCKA");
                t.Stop();
            };

            return t;
        }

        private void GeneralSlider_OnValueChanged(object sender, RangeBaseValueChangedEventArgs rangeBaseValueChangedEventArgs)
        {
            //TODO: needs to work without timers and just when you leave the pointer.
            Debug.WriteLine("Error");
            if (!_changeTimer.IsEnabled) { _changeTimer.Start(); }
            _changeAmount++;

            if (_changeAmount >= 809) //geeft ongeveer 81 request voor een hele slide. 
            {
                UpdateLamp();
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

        private void UpdateLamp()
        {
            foreach (var i in _indexes)
            {
                Connection.Connector.ChangeLight(i, new
                {
                    hue = HueSlider.Value,
                    sat = SaturationSlider.Value,
                    bri = BrightnessSlider.Value
                });
            }
        }

        private LinearGradientBrush ColorGradient()
        {
            GradientStopCollection collection = new GradientStopCollection();
            for (int i = 0; i < 100; i++)
            {
                collection.Add(new GradientStop() {Color = ColorUtil.getColor((655.35*i), 254, 254)});
            }
            return new LinearGradientBrush() {GradientStops = collection};
        }
    }
}
