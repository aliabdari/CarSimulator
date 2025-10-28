using CarSimulator.Enums;
using System.Text.Json.Serialization;

namespace CarSimulator.Models
{
    public class Engine
    {
        public bool IsRunning { get; private set; }

        public void Start() => IsRunning = true;
        public void Stop() => IsRunning = false;

        public Engine() {}

        [JsonConstructor]
        public Engine(bool isRunning)
        {
        IsRunning = isRunning;
        }
    }
}