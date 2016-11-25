using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueXamlApp.Connector
{
    public class Lights : ObservableCollection<Light>
    {
        public Lights(List<Light> lights)
        {
            foreach (var l in lights)
            {
                Add(l);
            }
        }

        public void Refresh(List<Light> lights)
        {
            Clear();
            foreach (var l in lights)
            {
                Add(l);
            }
        }
    }
}
