using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AplikacjaBazodanowa
{
    class PurpleAirClient
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "40CCE364-0A30-11F0-81BE-42010A80001F";
        private const string BaseUrl = "https://api.purpleair.com/v1/sensors/";

        public PurpleAirClient()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-API-Key", ApiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<SensorData?> GetSensorDataAsync(int sensorIndex)
        {
            string url = $"{BaseUrl}{sensorIndex}?fields=name,pm2.5_alt,temperature,humidity,last_seen";

            string json = await _httpClient.GetStringAsync(url);

            PurpleAirResponse? response = JsonSerializer.Deserialize<PurpleAirResponse>(json);

            return response?.sensor;
        }
    }
}
