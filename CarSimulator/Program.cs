using CarSimulator.Models;
using CarSimulator.Data;
using CarSimulator.Enums;
using CarSimulator.Helpers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSession();
}

app.UseHttpsRedirection();

List<Car> cars = CarRepository.LoadCars();

// load the cars from the database
ShutDownAllCars();

// to shutdown all the cars when a new car is selected
void ShutDownAllCars()
{
    foreach (var car in cars)
        car.ShutDown();

    CarRepository.SaveCars(cars);
}


// Endpoint for retrieving Cars
app.MapGet("/get-cars", () => cars);

// Endpoint for create a new car
app.MapPost("/create-car", (HttpContext context, string name) =>
{
    var car = new Car(name);
    cars.Add(car);
    CarRepository.SaveCars(cars);
    context.Session.SetString("CurrentCarId", car.Id.ToString());
    ShutDownAllCars();
    return Results.Created($"/cars/{car.Id}", new
    {
        message = $"Car '{car.Name}' created and set as current session car.",
        car
    });
});

// Endpoint for selecting a car
app.MapPost("/select-car", (HttpContext context, Guid carId) =>
{
    var car = cars.FirstOrDefault(c => c.Id == carId);
    if (car == null)
        return Results.NotFound("Car not found.");
    
    ShutDownAllCars();
    context.Session.SetString("CurrentCarId", car.Id.ToString());
    return Results.Ok($"Selected car '{car.Name}' ({car.Body}) as current session car.");
});

// Endpoint for getting the type of the current car
app.MapGet("/session/get-type", (HttpContext context) =>
{
    var result = CarSessionHelper.GetCurrentCar(context, cars, out Car? car);
    if (car == null)
        return result;

    return Results.Ok(car.GetCarType());
});

// Endpoint for acceleraing the current car
app.MapPost("/session/accelerate", (HttpContext context) =>
{
    var result = CarSessionHelper.GetCurrentCar(context, cars, out Car? car);
    if (car == null)
        return result;

    string output = car.Accelerate();
    CarRepository.SaveCars(cars);
    return Results.Ok(output);
});

// Endpoint for braking the current car
app.MapPost("/session/brake", (HttpContext context) =>
{
    var result = CarSessionHelper.GetCurrentCar(context, cars, out Car? car);
    if (car == null)
        return result;

    string output = car.Brake();
    CarRepository.SaveCars(cars);
    return Results.Ok(output);
});

// Endpoint for steering the current car
app.MapPost("/session/steer", (HttpContext context, Direction direction, int degree) =>
{
    var result = CarSessionHelper.GetCurrentCar(context, cars, out Car? car);
    if (car == null)
        return result;

    string output = car.Steer(direction, degree);
    CarRepository.SaveCars(cars);
    return Results.Ok(output);
})
.WithOpenApi(op =>
{
    var p = op.Parameters.First(p => p.Name == "direction");
    p.Description = "Direction. Allowed: Left, Right";
    return op;
});;

// Endpoint for getting the information of the current car
app.MapGet("/session/info", (HttpContext context) =>
{
    var result = CarSessionHelper.GetCurrentCar(context, cars, out Car? car);
    if (car == null)
        return result;

    return Results.Ok(car);
});

// Endpoint for getting the speed of the current car
app.MapGet("/session/get-speed", (HttpContext context) =>
{
    var result = CarSessionHelper.GetCurrentCar(context, cars, out Car? car);
    if (car == null)
        return result;

    return Results.Ok(car.GetSpeed());
});

// Endpoint for getting the direction of the current car
app.MapGet("/session/get-direction", (HttpContext context) =>
{
    var result = CarSessionHelper.GetCurrentCar(context, cars, out Car? car);
    if (car == null)
        return result;

    return Results.Ok(car.GetDirection());
});

// Endpoint for filling the tank of the current car
app.MapPost("/session/fill-with", (HttpContext context, FuelType fuel) =>
{
    var result = CarSessionHelper.GetCurrentCar(context, cars, out Car? car);
    if (car == null)
        return result;

    string output = car.FillWith(fuel);
    CarRepository.SaveCars(cars);
    return Results.Ok(output);
})
.WithOpenApi(op =>
{
    var p = op.Parameters.First(p => p.Name == "fuel");
    p.Description = "Fuel type. Allowed: Premium, Regular, Diesel";
    return op;
});

// Endpoint for honking the current car
app.MapPost("/session/honk", (HttpContext context) =>
{
    var result = CarSessionHelper.GetCurrentCar(context, cars, out Car? car);
    if (car == null)
        return result;

    string output = car.Honk();
    CarRepository.SaveCars(cars);
    return Results.Ok(output);
});

app.Run();