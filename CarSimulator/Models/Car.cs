using System;
using CarSimulator.Enums;
using System.Text.Json.Serialization;

namespace CarSimulator.Models
{
    public class Car
    {
        private static readonly Random random = new Random();

        // specify the max speed of each vehicle based on their type
        private int GetMaxSpeed()
        {
            return Body switch
            {
                CarBody.Compact => 120,
                CarBody.Sport => 300,
                CarBody.SUV => 150,
                CarBody.Truck => 120,
                _ => 180
            };
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; } = "Unnamed";
        public CarBody Body { get; private set; }

        public int Speed { get; private set; }
        public bool HasTrumpet { get; private set; }

        public Engine Engine { get; private set; }
        public Tank Tank { get; private set; }
        public Wheels Wheels { get; private set; }
        public SteeringWheel Steering { get; private set; }
        public BrakePedal BrakePedal { get; private set; }
        public AcceleratorPedal Accelerator { get; private set; }

        public Car()
        {
            Engine = new Engine();
            Tank = new Tank();
            Wheels = new Wheels();
            Steering = new SteeringWheel();
            BrakePedal = new BrakePedal();
            Accelerator = new AcceleratorPedal();
        }

        [JsonConstructor]
        public Car(Guid id, string name, CarBody body, int speed, bool hasTrumpet,
                Engine engine, Tank tank, Wheels wheels, SteeringWheel steering,
                BrakePedal brakePedal, AcceleratorPedal accelerator)
        {
            Id = id;
            Name = name;
            Body = body;
            Speed = speed;
            HasTrumpet = hasTrumpet;
            Engine = engine;
            Tank = tank;
            Wheels = wheels;
            Steering = steering;
            BrakePedal = brakePedal;
            Accelerator = accelerator;
        }

        public Car(string name)
        {
            Id = Guid.NewGuid();
            Name = name;

            // assigning a random type to each created car
            Body = (CarBody)random.Next(0, Enum.GetValues(typeof(CarBody)).Length);
            // consider Trumpet for the Trucks // and also other types of the cars by 10% probability
            HasTrumpet = (Body == CarBody.Truck) || (random.Next(0, 10) == 0);

            Engine = new Engine();
            Wheels = new Wheels();
            Steering = new SteeringWheel();
            BrakePedal = new BrakePedal();
            Accelerator = new AcceleratorPedal();
            Tank = new Tank(GetTankCapacity(Body));
            Speed = 0;
        }

        // consider the capacity of the Tank of each vehicle base on its type
        private int GetTankCapacity(CarBody body) => body switch
        {
            CarBody.Compact => 40,
            CarBody.Sport => 50,
            CarBody.SUV => 70,
            CarBody.Truck => 120,
            _ => 60
        };

        // check the possibility of a vehicle of using a specific type of Fuel
        public bool CanUseFuel(FuelType fuel)
        {
            return Body switch
            {
                CarBody.Truck or CarBody.SUV => fuel == FuelType.Diesel,
                CarBody.Compact or CarBody.Sport =>
                    fuel == FuelType.Regular || fuel == FuelType.Premium,
                _ => false
            };
        }

        public string Accelerate()
        {
            int max_speed = GetMaxSpeed();
            if (Tank.IsEmpty())
            {
                return "Cannot accelerate: tank is empty!";
            }

            if (Speed >= max_speed)
            {
                return $"Cannot accelerate anymore: You have reached the Max Speed of {max_speed}!";
            }

            int increase = Accelerator.Press(Body, Tank.FuelType);
            Speed += increase;
            Speed = Math.Min(Speed, max_speed);
            Tank.Consume(1);
            Engine.Start();
            return $"Accelerating... Speed = {Speed}";
        }

        public string Brake()
        {
            Speed = BrakePedal.ApplyBrake(Speed, Body == CarBody.Truck);
            if (Speed == 0)
                Engine.Stop();
            return $"Braking... Speed = {Speed}";
        }

        public string Steer(Direction direction, int degrees)
        {
            return Steering.Turn(direction, degrees);
        }

        public string GetCarType()
        {
            return "The type of the Car is " + Body.ToString();
        }

        public string GetSpeed()
        {
            return "The Current Speed of the Car is " + Speed;
        }

        public string GetDirection()
        {
            return Steering.GetCurrentDirection();
        }

        public string Honk()
        {
            string sound = HasTrumpet ? "Da-da-da-da-daah!" : "Beep!";
            return sound;
        }

        public string FillWith(FuelType fuel)
        {
            if (!CanUseFuel(fuel))
            {
                string allowed = (Body == CarBody.Truck || Body == CarBody.SUV)
                    ? "Diesel only"
                    : "Regular or Premium only";
                return $"Invalid fuel for {Body}. Allowed: {allowed}";
            }

            string result = Tank.Fill(fuel);

            return result;
        }
        
        public void ShutDown()
        {
            Speed = 0;
            Engine.Stop();
        }

    }
}
