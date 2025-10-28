namespace CarSimulator.Models
{
    public class Wheels
    {
        public int Count { get; } = 4;
        public double Size { get; }

        public Wheels(double size = 17)
        {
            Size = size;
        }
    }
}