# Car Simulator

This is a Car Simulator program implemented in C# language. This implementation contains a WebApi Simulating Car basic functions. 

## Install dotnet
To run the program you need to have dotnet (preferably sdk 8.0) installed on your system. For the windows you can use the [link](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) version x64 and for the linux you can use the following command.

```
sudo apt install -y dotnet-sdk-8.0
```


## How to use the Web API
To work with the Web API you can clone the project and got to the CarSimulator.Api by using the following commands:

```
git clone https://github.com/aliabdari/CarSimulator.git
cd CarSimulator/CarSimulator
dotnet run
```

Then on your browser you can go to the following address:
```
http://localhost:5175/swagger/index.html
```

In this webpage you can see different sets of GET and POST requests. To use them on Swagger you can open each of them separately and then use the "Try it out" button. Note that for all of the endpoints beginning with /session the car should be selected in advance (either through /select-car or /create-car), otherwise, they will reach an error. Their usages in detail are as follows:
- **GET /get-cars** : To get the information of all of the created cars so far.
- **POST /create-car** : To create a new car, which gets an argument as the name of the new created car. Once you create a new car, the session will be set to that car. Therefore, when a new car is created the status of the cars will be set as turned off.
- **POST /select-car** : If you just want to select a car or change the current car, you can use this part, which requires a carId parameter which can be retrieved through the /get-car endpoint. When a new car is selected the status of all the cars will be set as turned off and the user can work with the new car. 
- **GET /session/get-type** : It could be used in order to check the current Car's type.
- **POST /session/accelerate** : It will increase the speed of the Car based on the type of car and the type of fuel it uses. As response it will show the current speed of the Car.
- **POST /session/brake** : It will reduce the speed of the Car. Based on the car types either 10 or 6 units will be reduced.As response it will show the current speed of the Car.
- **POST /session/steer** : It will steer the Car based on its previous direction. It gets two parameters including direction (Left or Right) and (an integer between 0 and 360), then as response it calculates the current degree of the vehicle and its approximate direction in the map.
- **GET /session/info** : It will give all of the information of the current Car.
- **GET /session/get-speed** : It will give the current speed of the current Car.
- **GET /session/get-direction** : It will show you the current degree of the current Car and its approximate direction in the map.
- **POST /session/fill-with** : It will be used in order to fill the Tank of the Cars with appropriate fuels. As input it gets the Fuel Type which could be Premium, Regular, or Diesel. Currently, for the Compact and Sport Car Premium and Regular Fuels can be used while for the SUV and Truck Diesel can be used. As the response it will show a message that the tank is filled or if there was any problem. Please Note that if currently there is still fuel of a specific type in the Tank you should use the same type to refill the tank unless it becomes completely empty.
- **POST /session/honk**: With this option the current Car will honk and based on its type it will create different sounds, which will appear as the response.

## Database
  To save the cars' information, a JSON file has been used containing different specifications of the cars. This file will be saved in the "CarSimulator/CarSimulator/Data/cars.json".

## Models

  There are different models have been used to simulate a car. The classes are as follows:
  - [AccelatorPedal](https://github.com/aliabdari/CarSimulator/blob/main/CarSimulator/Models/AccelatorPedal.cs): This class is designed to adjust the car speed based on the type of the Car.
  - [BrakePedal](https://github.com/aliabdari/CarSimulator/blob/main/CarSimulator/Models/BrakePedal.cs): It will be used to simulate a brake based on the car's type and its current speed.
  - [Engine](https://github.com/aliabdari/CarSimulator/blob/main/CarSimulator/Models/Engine.cs): It simulates the engine of the car showing that the car is running or not.
  - [SteeringWheel](https://github.com/aliabdari/CarSimulator/blob/main/CarSimulator/Models/SteeringWheel.cs): It simulates the steering action in the car to adjust its direction based on the current direction.
  - [Tank](https://github.com/aliabdari/CarSimulator/blob/main/CarSimulator/Models/Tank.cs): It manages the procedures regarding the Fuel of a car, its Capacity, type and the process of filling or consuming the fuel.
  - [Wheels](https://github.com/aliabdari/CarSimulator/blob/main/CarSimulator/Models/Wheel.cs): This class keeps the information about the wheels numbers and sizes.
  - [Car](https://github.com/aliabdari/CarSimulator/blob/main/CarSimulator/Models/Car.cs): This class is the main class containing all of the components of a vehicle, including Id, Name, Body (Type), Speed, Engine, Tank, Wheels, Steering, Accelerate and Brake Pedals, and if it has trumpet or not.
 
## Encapsulation

To ensure clean object-oriented design and prevent uncontrolled modification of internal state, most properties in the simulator’s models (such as Car, Tank, Engine, and others) use private setters.

## Session Management and Persistence Approach
  
  In a typical HTTP environment, each request is stateless, meaning the server does not automatically remember data between separate requests from the same user. However, the car simulator requires that a user can keep interacting with the   same car across different actions (accelerate, brake, honk, etc.) without needing to re-select it each time. To achieve this, the project uses ASP.NET Core’s built-in Session Middleware to store the identifier of the currently active car on the server side for the user’s session.

  
