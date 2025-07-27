using Bunit;
using Microsoft.Extensions.DependencyInjection;
using CarShowcase.Pages;
using CarShowcase.Services;
using CarShowcase.Models;
using Moq;

namespace CarShowcase.Tests.Components;

public class CarsPageTests : TestContext
{
    [Fact]
    public void CarsPage_RendersCorrectly()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var sampleCars = new List<Car>
        {
            new Car { Id = 1, Make = "Toyota", Model = "Camry", Year = 2023, Price = 30000, Color = "Blue", Mileage = 15000, FuelType = "Gasoline", Transmission = "Automatic", ImageUrl = "test1.jpg" }
        };
        var makes = new List<string> { "Toyota", "Honda", "Ford" };

        mockCarService.Setup(s => s.GetMakesAsync()).ReturnsAsync(makes);
        mockCarService.Setup(s => s.SearchCarsAsync(null, null, null, null, null)).ReturnsAsync(sampleCars);
        Services.AddSingleton(mockCarService.Object);

        // Act
        var component = RenderComponent<Cars>();

        // Assert
        Assert.Contains("Our Car Collection", component.Markup);
        Assert.Contains("Browse through our premium selection", component.Markup);
        Assert.Contains("Search Filters", component.Markup);
    }

    [Fact]
    public void CarsPage_DisplaysSearchFilters()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var makes = new List<string> { "Toyota", "Honda", "Ford" };
        var sampleCars = new List<Car>();

        mockCarService.Setup(s => s.GetMakesAsync()).ReturnsAsync(makes);
        mockCarService.Setup(s => s.SearchCarsAsync(null, null, null, null, null)).ReturnsAsync(sampleCars);
        Services.AddSingleton(mockCarService.Object);

        // Act
        var component = RenderComponent<Cars>();

        // Assert
        Assert.Contains("All Makes", component.Markup);
        Assert.Contains("Toyota", component.Markup);
        Assert.Contains("Honda", component.Markup);
        Assert.Contains("Ford", component.Markup);
        Assert.Contains("Min Year", component.Markup);
        Assert.Contains("Max Year", component.Markup);
        Assert.Contains("Max Price", component.Markup);
    }

    [Fact]
    public void CarsPage_ShowsLoadingSpinner_WhenCarsAreNull()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var makes = new List<string> { "Toyota" };

        mockCarService.Setup(s => s.GetMakesAsync()).ReturnsAsync(makes);
        mockCarService.Setup(s => s.SearchCarsAsync(null, null, null, null, null)).ReturnsAsync((List<Car>?)null);
        Services.AddSingleton(mockCarService.Object);

        // Act
        var component = RenderComponent<Cars>();

        // Assert
        Assert.Contains("spinner-border", component.Markup);
        Assert.Contains("Loading...", component.Markup);
    }

    [Fact]
    public void CarsPage_ShowsNoCarsMessage_WhenListIsEmpty()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var makes = new List<string> { "Toyota" };
        var emptyCars = new List<Car>();

        mockCarService.Setup(s => s.GetMakesAsync()).ReturnsAsync(makes);
        mockCarService.Setup(s => s.SearchCarsAsync(null, null, null, null, null)).ReturnsAsync(emptyCars);
        Services.AddSingleton(mockCarService.Object);

        // Act
        var component = RenderComponent<Cars>();

        // Assert
        Assert.Contains("No cars found", component.Markup);
        Assert.Contains("Try adjusting your search criteria to find more results", component.Markup);
    }

    [Fact]
    public void CarsPage_DisplaysCars_WhenCarsAreLoaded()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var makes = new List<string> { "Toyota" };
        var sampleCars = new List<Car>
        {
            new Car 
            { 
                Id = 1, 
                Make = "Toyota", 
                Model = "Camry", 
                Year = 2023, 
                Price = 30000, 
                Color = "Blue", 
                Mileage = 15000, 
                FuelType = "Gasoline", 
                Transmission = "Automatic", 
                ImageUrl = "test1.jpg" 
            },
            new Car 
            { 
                Id = 2, 
                Make = "Honda", 
                Model = "Civic", 
                Year = 2022, 
                Price = 25000, 
                Color = "Red", 
                Mileage = 20000, 
                FuelType = "Gasoline", 
                Transmission = "Manual", 
                ImageUrl = "test2.jpg" 
            }
        };

        mockCarService.Setup(s => s.GetMakesAsync()).ReturnsAsync(makes);
        mockCarService.Setup(s => s.SearchCarsAsync(null, null, null, null, null)).ReturnsAsync(sampleCars);
        Services.AddSingleton(mockCarService.Object);

        // Act
        var component = RenderComponent<Cars>();

        // Assert
        Assert.Contains("2023 Toyota Camry", component.Markup);
        Assert.Contains("2022 Honda Civic", component.Markup);
        Assert.Contains("$30,000", component.Markup);
        Assert.Contains("$25,000", component.Markup);
        Assert.Contains("Blue", component.Markup);
        Assert.Contains("Red", component.Markup);
        Assert.Contains("15,000 miles", component.Markup);
        Assert.Contains("20,000 miles", component.Markup);
        Assert.Contains("Gasoline", component.Markup);
        Assert.Contains("Automatic", component.Markup);
        Assert.Contains("Manual", component.Markup);
    }

    [Fact]
    public void CarsPage_ContainsViewDetailsLinks()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var makes = new List<string> { "Toyota" };
        var sampleCars = new List<Car>
        {
            new Car { Id = 1, Make = "Toyota", Model = "Camry", Year = 2023, Price = 30000, Color = "Blue", Mileage = 15000, FuelType = "Gasoline", Transmission = "Automatic", ImageUrl = "test1.jpg" }
        };

        mockCarService.Setup(s => s.GetMakesAsync()).ReturnsAsync(makes);
        mockCarService.Setup(s => s.SearchCarsAsync(null, null, null, null, null)).ReturnsAsync(sampleCars);
        Services.AddSingleton(mockCarService.Object);

        // Act
        var component = RenderComponent<Cars>();

        // Assert
        var viewDetailsLinks = component.FindAll("a[href='/car/1']");
        Assert.NotEmpty(viewDetailsLinks);
        Assert.Contains("View Details", component.Markup);
    }

    [Fact]
    public void CarsPage_HasCorrectCardStructure()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var makes = new List<string> { "Toyota" };
        var sampleCars = new List<Car>
        {
            new Car { Id = 1, Make = "Toyota", Model = "Camry", Year = 2023, Price = 30000, Color = "Blue", Mileage = 15000, FuelType = "Gasoline", Transmission = "Automatic", ImageUrl = "test1.jpg" }
        };

        mockCarService.Setup(s => s.GetMakesAsync()).ReturnsAsync(makes);
        mockCarService.Setup(s => s.SearchCarsAsync(null, null, null, null, null)).ReturnsAsync(sampleCars);
        Services.AddSingleton(mockCarService.Object);

        // Act
        var component = RenderComponent<Cars>();

        // Assert
        // Check for car cards specifically (not including the search filter card)
        var carCards = component.FindAll(".col-md-4.col-lg-3 .card");
        Assert.Single(carCards);

        var cardImages = component.FindAll(".card-img-top");
        Assert.Single(cardImages);

        var cardTitles = component.FindAll(".card-title");
        Assert.Contains("Search Filters", component.Markup); // Search filter card title
        Assert.Contains("2023 Toyota Camry", component.Markup); // Car card title

        var viewDetailsButtons = component.FindAll(".btn-primary");
        Assert.NotEmpty(viewDetailsButtons);
    }
}
