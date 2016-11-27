using Windows.UI;
using Windows.UI.Xaml.Media;
using HughRemote.Common;

namespace HueXamlApp.Connector
{
    public class Light
    {
        public string Id { get; set; }
        public int H { get; set; }
        public int S { get; set; }
        public int V { get; set; }
        public bool IsOn { get; set; }
        public SolidColorBrush MyBrush { get; set; }

        public Light(int h, int s, int v, bool ison, string id)
        {
            H = h;
            S = s;
            V = v;
            IsOn = ison;
            Id = id;
            UpdateBrush();
           
        }

        public void UpdateBrush()
        {
            Color color = ColorUtil.HsvToRgb(H, S, V);
            SolidColorBrush brushColor = new SolidColorBrush(color);

            MyBrush = brushColor;
        }
        public override string ToString()
        {
            return $"Id = {Id}\n" +
                   $"H = {H}\n" +
                   $"S = {S}\n" +
                   $"V = {V}\n" +
                   $"On = {IsOn}";
        }
    }
}