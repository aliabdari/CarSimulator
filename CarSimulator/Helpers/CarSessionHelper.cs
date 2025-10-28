using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using CarSimulator.Models;

namespace CarSimulator.Helpers
{
    public static class CarSessionHelper
    {
        public const string MESSAGE_NO_CAR_SELECTED = "No car selected. Use /select-car first.";
        public const string MESSAGE_CAR_NOT_FOUND = "Car not found.";

        public static IResult GetCurrentCar(HttpContext context, List<Car> cars, out Car? car)
        {
            car = null;

            var idString = context.Session.GetString("CurrentCarId");
            if (string.IsNullOrEmpty(idString))
                return Results.BadRequest(MESSAGE_NO_CAR_SELECTED);

            if (!Guid.TryParse(idString, out Guid id))
                return Results.BadRequest("Invalid car ID stored in session.");

            car = cars.FirstOrDefault(c => c.Id == id);
            if (car == null)
                return Results.NotFound(MESSAGE_CAR_NOT_FOUND);

            return Results.Ok();
        }
    }
}