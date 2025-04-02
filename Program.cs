using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace AplikacjaBazodanowa;

internal class Program
{
    static async Task Main(string[] args)
    {
        var choice = "1";

        var sensorIndices = new Dictionary<int, string>();
        sensorIndices.Add(77429, "Wrocław, Poland");
        sensorIndices.Add(11178, "Łódź, Poland");
        sensorIndices.Add(55271, "Berlin, Germany");
        sensorIndices.Add(15969, "Barcelona, Spain");
        sensorIndices.Add(177937, "California, USA");
        sensorIndices.Add(264387, "El Paso, USA");


        while (choice != null)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Get sensor data from API");
            Console.WriteLine("2. Show sensor measurements from database");
            Console.WriteLine("3. Show all sensors in database");
            Console.WriteLine("4. Delete all sensors and measurements");
            Console.WriteLine("5. Show example sensor indices");
            Console.WriteLine("6. Exit program");
            Console.Write("Your choice: ");

            choice = Console.ReadLine();
            Console.WriteLine();

            if (choice == "1")
            {
                Console.WriteLine("Enter sensor index:");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int sensorIndex))
                {
                    using var db = new AppDbContext();
                    var currentSensor = db.Sensors.FirstOrDefault(s => s.SensorIndex == sensorIndex);

                    if (currentSensor != null)
                    {
                        Console.WriteLine("Sensor already exists in the database!");
                        continue;
                    }

                    var client = new PurpleAirClient();
                    var sensorData = await client.GetSensorDataAsync(sensorIndex);

                    if (sensorData != null)
                    {
                        Console.WriteLine("\nSensor data:");
                        Console.WriteLine(sensorData.ToString());

                        currentSensor = new Sensor
                        {
                            SensorIndex = sensorData.sensor_index ?? 0,
                            Name = sensorData.name
                        };
                        db.Sensors.Add(currentSensor);
                        db.SaveChanges();
                        Console.WriteLine("New sensor added to database.");

                        var measurement = new Measurement
                        {
                            Timestamp = sensorData.last_seen.HasValue ? DateTimeOffset.FromUnixTimeSeconds(sensorData.last_seen.Value).LocalDateTime : DateTime.Now,
                            PM25 = sensorData.pm2_5_alt ?? 0,
                            Temperature = sensorData.temperature.HasValue ? (int)((sensorData.temperature.Value - 32) * 5 / 9) : 0,
                            Humidity = sensorData.humidity ?? 0,
                            SensorId = currentSensor.Id
                        };

                        db.Measurements.Add(measurement);
                        db.SaveChanges();
                        Console.WriteLine("Measurement saved to database.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to retrieve data from the API.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid sensor index.");
                }
            }
            else if (choice == "2")
            {
                Console.WriteLine("Enter sensor index to display measurements:");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int sensorIndex))
                {
                    using var db = new AppDbContext();

                    var sensor = db.Sensors.FirstOrDefault(s => s.SensorIndex == sensorIndex);
                    if (sensor == null)
                    {
                        Console.WriteLine("Sensor not found in database.");
                        continue;
                    }

                    var latestMeasurement = db.Measurements
                        .Where(m => m.SensorId == sensor.Id)
                        .FirstOrDefault();


                    if (latestMeasurement == null)
                    {
                        Console.WriteLine("No measurements found for this sensor.");
                    }
                    else
                    {
                        Console.WriteLine($"Latest measurement for sensor {sensor.Name}:");
                        Console.WriteLine($"{latestMeasurement.Timestamp:g} | PM2.5: {latestMeasurement.PM25} µg/m³ | Temp: {latestMeasurement.Temperature}°C | Humidity: {latestMeasurement.Humidity}%");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid sensor index.");
                }
            }
            else if (choice == "3")
            {
                using var db = new AppDbContext();
                var sensors = db.Sensors
                    .OrderByDescending(s => s.SensorIndex)
                    .ToList();


                if (sensors.Count == 0)
                {
                    Console.WriteLine("No sensors in the database.");
                }
                else
                {
                    Console.WriteLine("Sensors in database, sorted by index descending:");
                    foreach (var s in sensors)
                    {
                        Console.WriteLine($"ID: {s.Id}, Index: {s.SensorIndex}, Name: {s.Name}");
                    }
                }
            }
            else if (choice == "4")
            {
                using var db = new AppDbContext();

                db.Measurements.RemoveRange(db.Measurements);
                db.Sensors.RemoveRange(db.Sensors);
                db.SaveChanges();

                Console.WriteLine("All data removed from database.");
            }
            else if (choice == "5")
            {
                for (int i = 0; i < sensorIndices.Count; i++)
                {
                    Console.WriteLine($"{sensorIndices.ElementAt(i).Value} ({sensorIndices.ElementAt(i).Key})");
                }
            }
            else if (choice == "6")
            {
                return;
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
            Console.ReadLine();
        }
    }
}