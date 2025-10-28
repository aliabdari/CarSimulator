namespace CarSimulator.Models
{
    public class BrakePedal
    {
        public int Strength { get; } = 10;

        public int ApplyBrake(int currentSpeed, bool isTruck)
        {
            int reduction = isTruck ? 6 : Strength;
            return currentSpeed - reduction < 0 ? 0 : currentSpeed - reduction;
        }
    }
}