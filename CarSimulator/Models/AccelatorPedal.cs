using CarSimulator.Enums;

namespace CarSimulator.Models
{
    public class AcceleratorPedal
    {
        public int Press(CarBody type, FuelType? fuel)
        {
            int baseIncrease = type switch
            {
                CarBody.Truck => 4,
                CarBody.Sport => 7,
                _ => 5
            };

            if (fuel == null)
                return baseIncrease;
                
            if (fuel == FuelType.Premium) {
                baseIncrease += 1;
            }

            return baseIncrease;
        }
    }
}
