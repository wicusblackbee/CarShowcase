using CarShowcase.Models;

namespace CarShowcase.Tests.Models;

public class CarTests
{
    [Fact]
    public void Car_DefaultConstructor_SetsDefaultValues()
    {
        // Arrange & Act
        var car = new Car();

        // Assert
        Assert.Equal(0, car.Id);
        Assert.Equal(string.Empty, car.Make);
        Assert.Equal(string.Empty, car.Model);
        Assert.Equal(0, car.Year);
        Assert.Equal(0, car.Price);
        Assert.Equal(string.Empty, car.Color);
        Assert.Equal(0, car.Mileage);
        Assert.Equal(string.Empty, car.FuelType);
        Assert.Equal(string.Empty, car.Transmission);
        Assert.Equal(string.Empty, car.Description);
        Assert.Equal(string.Empty, car.ImageUrl);
        Assert.True(car.IsAvailable);
        Assert.True(car.DateAdded <= DateTime.Now);
    }

    [Fact]
    public void Car_SetProperties_ReturnsCorrectValues()
    {
        // Arrange
        var car = new Car();
        var testDate = DateTime.Now;

        // Act
        car.Id = 1;
        car.Make = "Toyota";
        car.Model = "Camry";
        car.Year = 2023;
        car.Price = 30000;
        car.Color = "Blue";
        car.Mileage = 15000;
        car.FuelType = "Gasoline";
        car.Transmission = "Automatic";
        car.Description = "Test description";
        car.ImageUrl = "test-image.jpg";
        car.IsAvailable = false;
        car.DateAdded = testDate;

        // Assert
        Assert.Equal(1, car.Id);
        Assert.Equal("Toyota", car.Make);
        Assert.Equal("Camry", car.Model);
        Assert.Equal(2023, car.Year);
        Assert.Equal(30000, car.Price);
        Assert.Equal("Blue", car.Color);
        Assert.Equal(15000, car.Mileage);
        Assert.Equal("Gasoline", car.FuelType);
        Assert.Equal("Automatic", car.Transmission);
        Assert.Equal("Test description", car.Description);
        Assert.Equal("test-image.jpg", car.ImageUrl);
        Assert.False(car.IsAvailable);
        Assert.Equal(testDate, car.DateAdded);
    }

    [Theory]
    [InlineData("Toyota", "Camry", 2023, 30000, true)]
    [InlineData("Honda", "Civic", 2022, 25000, false)]
    [InlineData("Ford", "F-150", 2021, 35000, true)]
    public void Car_WithDifferentValues_SetsCorrectly(string make, string model, int year, decimal price, bool isAvailable)
    {
        // Arrange & Act
        var car = new Car
        {
            Make = make,
            Model = model,
            Year = year,
            Price = price,
            IsAvailable = isAvailable
        };

        // Assert
        Assert.Equal(make, car.Make);
        Assert.Equal(model, car.Model);
        Assert.Equal(year, car.Year);
        Assert.Equal(price, car.Price);
        Assert.Equal(isAvailable, car.IsAvailable);
    }
}
