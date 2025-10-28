using System.Text.Json;
using CarSimulator.Models;

namespace CarSimulator.Data
{
    public static class CarRepository
    {
        private static string defaultPath = "Data/cars.json";

        public static List<Car> LoadCars(string? filePath_ = null)
        {
            string filePath = filePath_ ?? defaultPath;

            if (!File.Exists(filePath))
            {
                return new List<Car>();
            }

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Car>>(json) ?? new List<Car>();
        }

        public static void SaveCars(List<Car> cars, string? filePath_ = null)
        {
            string filePath = filePath_ ?? defaultPath;
            string json = JsonSerializer.Serialize(cars, new JsonSerializerOptions { WriteIndented = true });
            Directory.CreateDirectory("Data");
            File.WriteAllText(filePath, json);
        }
    }
}
