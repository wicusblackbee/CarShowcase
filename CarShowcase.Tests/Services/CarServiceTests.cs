using CarShowcase.Models;
using CarShowcase.Services;

namespace CarShowcase.Tests.Services;

public class CarServiceTests
{
    private readonly ICarService _carService;

    public CarServiceTests()
    {
        _carService = new CarService();
    }

    [Fact]
    public async Task GetAllCarsAsync_ReturnsAvailableCars()
    {
        // Act
        var cars = await _carService.GetAllCarsAsync();

        // Assert
        Assert.NotNull(cars);
        Assert.NotEmpty(cars);
        Assert.All(cars, car => Assert.True(car.IsAvailable));
    }

    [Fact]
    public async Task GetCarByIdAsync_WithValidId_ReturnsCar()
    {
        // Arrange
        var cars = await _carService.GetAllCarsAsync();
        var firstCar = cars.First();

        // Act
        var result = await _carService.GetCarByIdAsync(firstCar.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(firstCar.Id, result.Id);
        Assert.Equal(firstCar.Make, result.Make);
        Assert.Equal(firstCar.Model, result.Model);
    }

    [Fact]
    public async Task GetCarByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = await _carService.GetCarByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task SearchCarsAsync_WithMakeFilter_ReturnsFilteredCars()
    {
        // Act
        var result = await _carService.SearchCarsAsync(make: "Toyota");

        // Assert
        Assert.NotNull(result);
        Assert.All(result, car => Assert.Contains("Toyota", car.Make, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task SearchCarsAsync_WithModelFilter_ReturnsFilteredCars()
    {
        // Act
        var result = await _carService.SearchCarsAsync(model: "Camry");

        // Assert
        Assert.NotNull(result);
        Assert.All(result, car => Assert.Contains("Camry", car.Model, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task SearchCarsAsync_WithYearRange_ReturnsFilteredCars()
    {
        // Act
        var result = await _carService.SearchCarsAsync(minYear: 2022, maxYear: 2023);

        // Assert
        Assert.NotNull(result);
        Assert.All(result, car => Assert.InRange(car.Year, 2022, 2023));
    }

    [Fact]
    public async Task SearchCarsAsync_WithMaxPrice_ReturnsFilteredCars()
    {
        // Act
        var result = await _carService.SearchCarsAsync(maxPrice: 30000);

        // Assert
        Assert.NotNull(result);
        Assert.All(result, car => Assert.True(car.Price <= 30000));
    }

    [Fact]
    public async Task SearchCarsAsync_WithMultipleFilters_ReturnsFilteredCars()
    {
        // Act
        var result = await _carService.SearchCarsAsync(
            make: "Toyota",
            minYear: 2020,
            maxPrice: 50000);

        // Assert
        Assert.NotNull(result);
        Assert.All(result, car =>
        {
            Assert.Contains("Toyota", car.Make, StringComparison.OrdinalIgnoreCase);
            Assert.True(car.Year >= 2020);
            Assert.True(car.Price <= 50000);
        });
    }

    [Fact]
    public async Task SearchCarsAsync_WithNoMatchingCriteria_ReturnsEmptyList()
    {
        // Act
        var result = await _carService.SearchCarsAsync(make: "NonExistentMake");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddCarAsync_WithValidCar_ReturnsTrue()
    {
        // Arrange
        var newCar = new Car
        {
            Make = "Test",
            Model = "TestModel",
            Year = 2023,
            Price = 25000,
            Color = "Red",
            Mileage = 0,
            FuelType = "Electric",
            Transmission = "Automatic",
            Description = "Test car"
        };

        // Act
        var result = await _carService.AddCarAsync(newCar);

        // Assert
        Assert.True(result);
        Assert.True(newCar.Id > 0);
        Assert.True(newCar.DateAdded <= DateTime.Now);
    }

    [Fact]
    public async Task AddCarAsync_WithNullCar_ReturnsFalse()
    {
        // Act
        var result = await _carService.AddCarAsync(null!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateCarAsync_WithValidCar_ReturnsTrue()
    {
        // Arrange
        var cars = await _carService.GetAllCarsAsync();
        var carToUpdate = cars.First();
        carToUpdate.Make = "UpdatedMake";
        carToUpdate.Model = "UpdatedModel";

        // Act
        var result = await _carService.UpdateCarAsync(carToUpdate);

        // Assert
        Assert.True(result);
        
        var updatedCar = await _carService.GetCarByIdAsync(carToUpdate.Id);
        Assert.NotNull(updatedCar);
        Assert.Equal("UpdatedMake", updatedCar.Make);
        Assert.Equal("UpdatedModel", updatedCar.Model);
    }

    [Fact]
    public async Task UpdateCarAsync_WithNullCar_ReturnsFalse()
    {
        // Act
        var result = await _carService.UpdateCarAsync(null!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateCarAsync_WithNonExistentCar_ReturnsFalse()
    {
        // Arrange
        var nonExistentCar = new Car { Id = 999, Make = "Test" };

        // Act
        var result = await _carService.UpdateCarAsync(nonExistentCar);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteCarAsync_WithValidId_ReturnsTrue()
    {
        // Arrange
        var cars = await _carService.GetAllCarsAsync();
        var carToDelete = cars.First();

        // Act
        var result = await _carService.DeleteCarAsync(carToDelete.Id);

        // Assert
        Assert.True(result);
        
        var deletedCar = await _carService.GetCarByIdAsync(carToDelete.Id);
        Assert.NotNull(deletedCar);
        Assert.False(deletedCar.IsAvailable);
    }

    [Fact]
    public async Task DeleteCarAsync_WithInvalidId_ReturnsFalse()
    {
        // Act
        var result = await _carService.DeleteCarAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetMakesAsync_ReturnsDistinctMakes()
    {
        // Act
        var makes = await _carService.GetMakesAsync();

        // Assert
        Assert.NotNull(makes);
        Assert.NotEmpty(makes);
        Assert.Equal(makes.Count, makes.Distinct().Count()); // Ensure no duplicates
        Assert.True(makes.SequenceEqual(makes.OrderBy(m => m))); // Ensure sorted
    }

    [Fact]
    public async Task GetModelsAsync_WithValidMake_ReturnsModels()
    {
        // Arrange
        var makes = await _carService.GetMakesAsync();
        var firstMake = makes.First();

        // Act
        var models = await _carService.GetModelsAsync(firstMake);

        // Assert
        Assert.NotNull(models);
        Assert.NotEmpty(models);
        Assert.Equal(models.Count, models.Distinct().Count()); // Ensure no duplicates
        Assert.True(models.SequenceEqual(models.OrderBy(m => m))); // Ensure sorted
    }

    [Fact]
    public async Task GetModelsAsync_WithInvalidMake_ReturnsEmptyList()
    {
        // Act
        var models = await _carService.GetModelsAsync("NonExistentMake");

        // Assert
        Assert.NotNull(models);
        Assert.Empty(models);
    }
}
