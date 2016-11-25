using System;
using System.Collections.Generic;
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
        public readonly string Username;
        private readonly HttpClient _client;
        public Lights Lights { get; set; }
        public HueConnector(string adres, string username)
        {
            Username = username;
            Adres = adres;

            _client = new HttpClient();
            Login();

        }

        public async void Login()
        {
            try
            {
                HttpContent content = new JsonContent(JsonConvert.SerializeObject(new
                {
                    devicetype = $"MennoGijsApp#{Username}"
                }));

                var response = await _client.PostAsync(Adres, content);

                var responseMessage = await response.Content.ReadAsStringAsync();
                var strings = responseMessage.Split('"');
                Adres += strings[5];
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            GetLights();
        }

        public async void GetLights()
        {
            List<Light> lights = new List<Light>();
            int index = 1;
            bool isNextLight = false;
            while (!isNextLight)
            {
                try
                {
                    var response = await _client.GetAsync($"{Adres}/lights/{index}");
                    var responseMessage = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(responseMessage);
                    dynamic message = JsonConvert.DeserializeObject(responseMessage);
                    
                    lights.Add(new Light
                    {
                        Id = (string) message.name,
                        IsOn = (bool) message.state.on,
                        H = (double) message.state.hue,
                        S = (double) message.state.sat,
                        V = (double) message.state.bri
                    });
                    index++;
                }
                catch (Exception e)
                {
                    isNextLight = true;
                }
                Lights = new Lights(lights);
            }
        }

        public async void ChangeLight(int index, dynamic message)
        {
            HttpContent responseMessage = new JsonContent(JsonConvert.SerializeObject(message));
            await _client.PutAsync(Adres + $"/lights/{index}/state", responseMessage);
        }
    }
}
