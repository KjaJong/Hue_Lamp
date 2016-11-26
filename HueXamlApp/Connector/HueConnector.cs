using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public string Username { get; set; }
        public static ObservableCollection<Light> Lights { get; set; }
        private readonly HttpClient _client;

        
        public HueConnector(string adres, string username)
        {
            Username = username;
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
                    devicetype = $"MennoGijsApp#{Username}"
                }));

                var response = await _client.PostAsync(Adres, content);

                var responseMessage = await response.Content.ReadAsStringAsync();
                dynamic message = JsonConvert.DeserializeObject(responseMessage);
                //var strings = responseMessage.Split('"');
                Username = (string)message[0].success.username;
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

            while (true)
            {
                try
                {
                    var response = await _client.GetAsync($"{Adres}{Username}/lights/{index}");
                    var responseMessage = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(responseMessage);
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
                    return;
                }
            }
            
        }

        public async void ChangeLight(int index, dynamic message)
        {
            HttpContent responseMessage = new JsonContent(JsonConvert.SerializeObject(message));
            await _client.PutAsync(Adres + $"{Username}/lights/{index}/state", responseMessage);
        }
    }
}
