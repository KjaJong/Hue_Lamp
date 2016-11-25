namespace HueXamlApp.Connector
{
    public class Light
    {
        public string Id { get; set; }
        public double H { get; set; }
        public double S { get; set; }
        public double V { get; set; }
        public bool IsOn { get; set; }

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