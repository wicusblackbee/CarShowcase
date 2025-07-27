using CarShowcase.Models;

namespace CarShowcase.Services;

public class CarService : ICarService
{
    private readonly List<Car> _cars;

    public CarService()
    {
        _cars = GenerateSampleCars();
    }

    public Task<List<Car>> GetAllCarsAsync()
    {
        return Task.FromResult(_cars.Where(c => c.IsAvailable).ToList());
    }

    public Task<Car?> GetCarByIdAsync(int id)
    {
        var car = _cars.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(car);
    }

    public Task<List<Car>> SearchCarsAsync(string? make = null, string? model = null, int? minYear = null, int? maxYear = null, decimal? maxPrice = null)
    {
        var query = _cars.Where(c => c.IsAvailable);

        if (!string.IsNullOrEmpty(make))
            query = query.Where(c => c.Make.Contains(make, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(model))
            query = query.Where(c => c.Model.Contains(model, StringComparison.OrdinalIgnoreCase));

        if (minYear.HasValue)
            query = query.Where(c => c.Year >= minYear.Value);

        if (maxYear.HasValue)
            query = query.Where(c => c.Year <= maxYear.Value);

        if (maxPrice.HasValue)
            query = query.Where(c => c.Price <= maxPrice.Value);

        return Task.FromResult(query.ToList());
    }

    public Task<bool> AddCarAsync(Car car)
    {
        if (car == null) return Task.FromResult(false);
        
        car.Id = _cars.Count > 0 ? _cars.Max(c => c.Id) + 1 : 1;
        car.DateAdded = DateTime.Now;
        _cars.Add(car);
        return Task.FromResult(true);
    }

    public Task<bool> UpdateCarAsync(Car car)
    {
        if (car == null) return Task.FromResult(false);
        
        var existingCar = _cars.FirstOrDefault(c => c.Id == car.Id);
        if (existingCar == null) return Task.FromResult(false);

        var index = _cars.IndexOf(existingCar);
        _cars[index] = car;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteCarAsync(int id)
    {
        var car = _cars.FirstOrDefault(c => c.Id == id);
        if (car == null) return Task.FromResult(false);

        car.IsAvailable = false;
        return Task.FromResult(true);
    }

    public Task<List<string>> GetMakesAsync()
    {
        var makes = _cars.Select(c => c.Make).Distinct().OrderBy(m => m).ToList();
        return Task.FromResult(makes);
    }

    public Task<List<string>> GetModelsAsync(string make)
    {
        var models = _cars.Where(c => c.Make.Equals(make, StringComparison.OrdinalIgnoreCase))
                          .Select(c => c.Model).Distinct().OrderBy(m => m).ToList();
        return Task.FromResult(models);
    }

    private List<Car> GenerateSampleCars()
    {
        return new List<Car>
        {
            new Car
            {
                Id = 1,
                Make = "Toyota",
                Model = "Camry",
                Year = 2022,
                Price = 28500,
                Color = "Silver",
                Mileage = 15000,
                FuelType = "Gasoline",
                Transmission = "Automatic",
                Description = "Reliable and fuel-efficient sedan with excellent safety ratings.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Toyota+Camry"
            },
            new Car
            {
                Id = 2,
                Make = "Honda",
                Model = "Civic",
                Year = 2023,
                Price = 24000,
                Color = "Blue",
                Mileage = 8000,
                FuelType = "Gasoline",
                Transmission = "Manual",
                Description = "Sporty compact car with great handling and modern features.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Honda+Civic"
            },
            new Car
            {
                Id = 3,
                Make = "Tesla",
                Model = "Model 3",
                Year = 2023,
                Price = 45000,
                Color = "White",
                Mileage = 5000,
                FuelType = "Electric",
                Transmission = "Automatic",
                Description = "Premium electric sedan with autopilot and cutting-edge technology.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Tesla+Model+3"
            },
            new Car
            {
                Id = 4,
                Make = "Ford",
                Model = "F-150",
                Year = 2022,
                Price = 35000,
                Color = "Black",
                Mileage = 20000,
                FuelType = "Gasoline",
                Transmission = "Automatic",
                Description = "America's best-selling truck with impressive towing capacity.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Ford+F-150"
            },
            new Car
            {
                Id = 5,
                Make = "BMW",
                Model = "X5",
                Year = 2021,
                Price = 55000,
                Color = "Gray",
                Mileage = 25000,
                FuelType = "Gasoline",
                Transmission = "Automatic",
                Description = "Luxury SUV with premium interior and advanced driver assistance.",
                ImageUrl = "https://via.placeholder.com/400x300?text=BMW+X5"
            },
            new Car
            {
                Id = 6,
                Make = "Audi",
                Model = "A4",
                Year = 2023,
                Price = 42000,
                Color = "Red",
                Mileage = 3000,
                FuelType = "Gasoline",
                Transmission = "Automatic",
                Description = "Elegant sedan with quattro all-wheel drive and premium features.",
                ImageUrl = "https://via.placeholder.com/400x300?text=Audi+A4"
            }
        };
    }
}
