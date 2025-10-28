using CarSimulator.Enums;
using System.Text.Json.Serialization;

namespace CarSimulator.Models
{
    public class SteeringWheel
    {

        public int CurrentDegree { get; private set; }

        public SteeringWheel()
        {
            CurrentDegree = 0;
        }

        [JsonConstructor]
        public SteeringWheel(int currentDegree)
        {
            CurrentDegree = currentDegree;
        }

        public string GetApproximateDirection()
        {
            if (337.5 < CurrentDegree || CurrentDegree <= 22.5)
                return "North";
            if (22.5 < CurrentDegree && CurrentDegree <= 67.5)
                return "North-East";
            if (67.5 < CurrentDegree && CurrentDegree <= 112.5)
                return "East";
            if (112.5 < CurrentDegree && CurrentDegree <= 157.5)
                return "South-East";
            if (157.5 < CurrentDegree && CurrentDegree <= 202.5)
                return "South";
            if (202.5 < CurrentDegree && CurrentDegree <= 247.5)
                return "South-West";
            if (247.5 < CurrentDegree && CurrentDegree <= 292.5)
                return "West";
            else
                return "North-West";
        }

        public string GetCurrentDirection()
        {
            return $"Current status: {CurrentDegree} degree and approximately it is towards {GetApproximateDirection()}.";
        }
        
        public string Turn(Direction direction, int degrees)
        {
            if (direction == Direction.Straight)
                return $"Your Direction is Straight therefore, there will be no change." +
                    $"{GetCurrentDirection()}";

            if (degrees == 0)
                return $"The degree is zero therefore, there will be no change." +
                    $"{GetCurrentDirection()}";

            if (direction == Direction.Right)
                CurrentDegree = (CurrentDegree + degrees) % 360;
            if (direction == Direction.Left)
                CurrentDegree = (CurrentDegree - degrees) % 360;
            if (CurrentDegree < 0)
                CurrentDegree += 360;

            return $"Car steered {direction} by {degrees} degrees. " +
                        $"{GetCurrentDirection()}";
        }
    }
}