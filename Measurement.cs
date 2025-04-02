using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaBazodanowa
{
    public class Measurement
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public float PM25 { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }

        public int SensorId { get; set; }
        public Sensor Sensor { get; set; } = null!;
    }

}
