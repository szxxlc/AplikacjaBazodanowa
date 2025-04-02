using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaBazodanowa
{
    public class Sensor
    {
        public int Id { get; set; }
        public required int SensorIndex { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }

        public List<Measurement> Measurements { get; set; } = new();
    }
}
