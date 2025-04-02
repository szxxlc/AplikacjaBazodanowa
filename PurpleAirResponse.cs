using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AplikacjaBazodanowa
{
    public class PurpleAirResponse
    {
        public SensorData? sensor { get; set; }
    }

    public class SensorData
    {
        public int? sensor_index { get; set; }
        public String? name { get; set; }

        [JsonPropertyName("pm2.5_alt")]
        public float? pm2_5_alt { get; set; }
        public float? temperature { get; set; }
        public float? humidity { get; set; }
        public long? last_seen { get; set; } // timestamp unix

        public override string ToString()
        {
            var date = last_seen.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(last_seen.Value).ToLocalTime().ToString("g") // converting unix timestamp to local time
                : "N/A";
            double? temperatureC = temperature.HasValue
                ? temperatureC = Math.Round((temperature.Value - 32) * 5 / 9, 1) // converting fahrenheit to celsius
                : null;
            int temperatureCRounded = temperatureC.HasValue
                ? (int)temperatureC.Value
                : 0;
            return $"SensorIndex: {sensor_index}, Name: {name}, PM2.5: {pm2_5_alt}µg/m³, Temp: {temperatureCRounded}°C, Humidity: {humidity}%, LastSeen: {date}";
        }
    }
}
