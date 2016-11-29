using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Q42.HueApi.Models;

namespace HueXamlApp.Connector
{
    public class HueConnector
    {
        public string Adres { get; set; }
        public bool IsConnected { get; set; }
        public string FakeUsername { get; set; }
        public static ObservableCollection<Light> Lights { get; set; }
        private readonly HttpClient _client;
        
        public HueConnector(string adres, string username)
        {
            FakeUsername = username;
            Adres = adres;

            _client = new HttpClient();
            Lights = new ObservableCollection<Light>();
        }

        public async Task Login()
        {
            try
            {
                HttpContent content = new JsonContent(JsonConvert.SerializeObject(new
                {
                    devicetype = $"MennoGijsApp#{FakeUsername}"
                }));

                var response = await _client.PostAsync(Adres, content);
                var responseMessage = await response.Content.ReadAsStringAsync();

                dynamic message = JsonConvert.DeserializeObject(responseMessage);
                Adres += (string)message[0].success.username;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }   
            await GetLights();
            
        }

        public async Task GetLights()
        {
            int index = 1;
            Lights.Clear();

            while (true)
            {
                try
                {
                    var response = await _client.GetAsync($"{Adres}/lights/{index}");
                    var responseMessage = await response.Content.ReadAsStringAsync();
                    dynamic message = JsonConvert.DeserializeObject(responseMessage);

                    Lights.Add(new Light(
                        (int) message.state.hue,
                        (int) message.state.sat,
                        (int) message.state.bri,
                        (bool) message.state.on,
                        (string) message.name
                    ));
                    index++;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                    if (index == 1) FakeUsername = "Login failed";
                    return;
                }
            }
        }

        public async void ChangeLight(int index, dynamic message)
        {
            HttpContent responseMessage = new JsonContent(JsonConvert.SerializeObject(message));
            await _client.PutAsync(Adres + $"/lights/{index}/state", responseMessage);
        }
    }
}
