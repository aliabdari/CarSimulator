using CarSimulator.Enums;
using System.Text.Json.Serialization;

namespace CarSimulator.Models
{
    public class Tank
    {
        public int Capacity { get; private set; }
        public int CurrentLevel { get; private set; }
        public FuelType? FuelType { get; private set; }

        public Tank() {}

        [JsonConstructor]
        public Tank(int capacity, int currentLevel, FuelType? fuelType)
        {
        Capacity = capacity;
        CurrentLevel = currentLevel;
        FuelType = fuelType;
        }
        
        public Tank(int capacity)
        {
            Capacity = capacity;
            CurrentLevel = 0;
            FuelType = null;
        }

        public bool IsEmpty() => CurrentLevel <= 0;

        public string Fill(FuelType fuel)
        {
            if (FuelType != null && FuelType != fuel)
            {
                return "Cannot mix different fuel types! Please empty tank first.";
            }
            CurrentLevel = Capacity;
            FuelType = fuel;

            return $"Filled the Tank: {CurrentLevel}/{Capacity}L";
        }

        public void Consume(int liters)
        {
            if (IsEmpty())
            {
                Console.WriteLine("Tank is empty!");
                return;
            }

            CurrentLevel -= liters;
            if (CurrentLevel < 0) CurrentLevel = 0;
        }

        public double GetLevelPercentage() => (CurrentLevel / Capacity) * 100;
    }
}