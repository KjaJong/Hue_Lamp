using System;
using System.Net.Http;
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

        public async void ChangeLight(int index, dynamic message)
        {
            HttpContent responseMessage = new JsonContent(JsonConvert.SerializeObject(message));
            await _client.PutAsync(Adres + $"/lights/{index}/state", responseMessage);
        }
    }
}
