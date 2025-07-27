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

    private string GetCarImageUrl(string make, string model)
    {
        // Use multiple fallback strategies for reliable image loading
        var imageStrategies = new List<Func<string, string, string>>
        {
            // Strategy 1: Picsum Photos (Lorem Picsum) - reliable and fast
            (m, mod) => $"https://picsum.photos/400/300?random={GetImageSeed(m, mod)}",
            
            // Strategy 2: DummyImage.com - reliable placeholder service
            (m, mod) => $"https://dummyimage.com/400x300/4a90e2/ffffff&text={Uri.EscapeDataString($"{m} {mod}")}",
            
            // Strategy 3: Placeholder.com - another reliable service
            (m, mod) => $"https://placeholder.com/400x300/4a90e2/ffffff?text={Uri.EscapeDataString($"{m} {mod}")}",
            
            // Strategy 4: Local fallback - generate a data URL with SVG
            (m, mod) => GenerateLocalCarImage(m, mod)
        };

        // Try the first strategy (Picsum Photos)
        return imageStrategies[0](make, model);
    }

    private int GetImageSeed(string make, string model)
    {
        // Generate a consistent seed based on make and model for consistent images
        return (make + model).GetHashCode() & 0x7FFFFFFF; // Ensure positive number
    }

    private string GenerateLocalCarImage(string make, string model)
    {
        // Generate a simple SVG image as a fallback
        var colors = new[] { "#4a90e2", "#7ed321", "#f5a623", "#d0021b", "#9013fe", "#50e3c2" };
        var colorIndex = Math.Abs((make + model).GetHashCode()) % colors.Length;
        var color = colors[colorIndex];
        
        var svg = $@"
            <svg width=""400"" height=""300"" xmlns=""http://www.w3.org/2000/svg"">
                <rect width=""400"" height=""300"" fill=""{color}"" />
                <text x=""200"" y=""130"" font-family=""Arial, sans-serif"" font-size=""24"" font-weight=""bold"" 
                      text-anchor=""middle"" fill=""white"">{make}</text>
                <text x=""200"" y=""170"" font-family=""Arial, sans-serif"" font-size=""20"" 
                      text-anchor=""middle"" fill=""white"">{model}</text>
                <circle cx=""100"" cy=""220"" r=""25"" fill=""rgba(255,255,255,0.3)"" />
                <circle cx=""300"" cy=""220"" r=""25"" fill=""rgba(255,255,255,0.3)"" />
                <rect x=""80"" y=""100"" width=""240"" height=""80"" rx=""10"" fill=""rgba(255,255,255,0.2)"" />
            </svg>";
        
        var base64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(svg));
        return $"data:image/svg+xml;base64,{base64}";
    }

    private List<Car> GenerateSampleCars()
    {
        return
        [
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
                ImageUrl = GetCarImageUrl("Toyota", "Camry")
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
                ImageUrl = GetCarImageUrl("Honda", "Civic")
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
                ImageUrl = GetCarImageUrl("Tesla", "Model 3")
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
                ImageUrl = GetCarImageUrl("Ford", "F-150")
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
                ImageUrl = GetCarImageUrl("BMW", "X5")
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
                ImageUrl = GetCarImageUrl("Audi", "A4")
            }
        ];
    }
}
