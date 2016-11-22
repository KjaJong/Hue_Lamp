using System;
using System.Collections.Generic;
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
                var strings = responseMessage.Split('[',']');
                dynamic message = JsonConvert.DeserializeObject(strings[1]);

                Adres += (string)message.success.username;
            }
            catch (Exception e)
            {
            }
        }

        public async Task<List<Light>> GetLights()
        {
            int index = 1;
            bool isNextLight = false;
            List<Light> lights = new List<Light>();
            while (!isNextLight)
            {
                var response = await _client.GetAsync($"{Adres}/lights/{index}");
                var responseMessage = await response.Content.ReadAsStringAsync();
                dynamic message = JsonConvert.DeserializeObject(responseMessage);
               

                try
                {
                    lights.Add(new Light
                    {
                        H = (double) message.state.hue,
                        Id = (string) message.modelid,
                        IsOn = (bool) message.on,
                        S = (double) message.sat,
                        V = (double) message.bri
                    });
                    index++;
                }
                catch (Exception e)
                {
                    isNextLight = true;
                }
            }
            return lights;
        }

        public async void ChangeLight(int index, dynamic message)
        {
            HttpContent responseMessage = new JsonContent(JsonConvert.SerializeObject(message));
            await _client.PutAsync(Adres + $"/lights/{index}/state", responseMessage);
        }
    }
}
