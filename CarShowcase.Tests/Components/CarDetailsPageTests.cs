using Bunit;
using Microsoft.Extensions.DependencyInjection;
using CarShowcase.Pages;
using CarShowcase.Services;
using CarShowcase.Models;
using Moq;
using Microsoft.AspNetCore.Components;

namespace CarShowcase.Tests.Components;

public class CarDetailsPageTests : TestContext
{
    [Fact]
    public void CarDetailsPage_WithValidCar_RendersCorrectly()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var mockNavigationManager = new Mock<NavigationManager>();
        
        var testCar = new Car
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
            Description = "Reliable and fuel-efficient sedan",
            ImageUrl = "test1.jpg",
            IsAvailable = true,
            DateAdded = DateTime.Now.AddDays(-30)
        };

        mockCarService.Setup(s => s.GetCarByIdAsync(1)).ReturnsAsync(testCar);
        Services.AddSingleton(mockCarService.Object);
        Services.AddSingleton(mockNavigationManager.Object);

        // Act
        var component = RenderComponent<CarDetails>(parameters => parameters.Add(p => p.Id, 1));

        // Assert
        Assert.Contains("2023 Toyota Camry", component.Markup);
        Assert.Contains("$30,000", component.Markup);
        Assert.Contains("Reliable and fuel-efficient sedan", component.Markup);
        Assert.Contains("Blue", component.Markup);
        Assert.Contains("15,000 miles", component.Markup);
        Assert.Contains("Gasoline", component.Markup);
        Assert.Contains("Automatic", component.Markup);
    }

    [Fact]
    public void CarDetailsPage_WithAvailableCar_ShowsActionButtons()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var mockNavigationManager = new Mock<NavigationManager>();
        
        var testCar = new Car
        {
            Id = 1,
            Make = "Toyota",
            Model = "Camry",
            Year = 2023,
            Price = 30000,
            IsAvailable = true
        };

        mockCarService.Setup(s => s.GetCarByIdAsync(1)).ReturnsAsync(testCar);
        Services.AddSingleton(mockCarService.Object);
        Services.AddSingleton(mockNavigationManager.Object);

        // Act
        var component = RenderComponent<CarDetails>(parameters => parameters.Add(p => p.Id, 1));

        // Assert
        Assert.Contains("Contact Dealer", component.Markup);
        Assert.Contains("Schedule Test Drive", component.Markup);
        Assert.Contains("Get Financing", component.Markup);
        Assert.DoesNotContain("Vehicle Sold", component.Markup);
    }

    [Fact]
    public void CarDetailsPage_WithUnavailableCar_ShowsSoldButton()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var mockNavigationManager = new Mock<NavigationManager>();
        
        var testCar = new Car
        {
            Id = 1,
            Make = "Toyota",
            Model = "Camry",
            Year = 2023,
            Price = 30000,
            IsAvailable = false
        };

        mockCarService.Setup(s => s.GetCarByIdAsync(1)).ReturnsAsync(testCar);
        Services.AddSingleton(mockCarService.Object);
        Services.AddSingleton(mockNavigationManager.Object);

        // Act
        var component = RenderComponent<CarDetails>(parameters => parameters.Add(p => p.Id, 1));

        // Assert
        Assert.Contains("Vehicle Sold", component.Markup);
        Assert.DoesNotContain("Contact Dealer", component.Markup);
        Assert.DoesNotContain("Schedule Test Drive", component.Markup);
        Assert.DoesNotContain("Get Financing", component.Markup);
    }

    [Fact]
    public void CarDetailsPage_WithNonExistentCar_ShowsNotFoundMessage()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var mockNavigationManager = new Mock<NavigationManager>();
        
        mockCarService.Setup(s => s.GetCarByIdAsync(999)).ReturnsAsync((Car?)null);
        Services.AddSingleton(mockCarService.Object);
        Services.AddSingleton(mockNavigationManager.Object);

        // Act
        var component = RenderComponent<CarDetails>(parameters => parameters.Add(p => p.Id, 999));

        // Assert
        Assert.Contains("Car Not Found", component.Markup);
        Assert.Contains("doesn't exist or is no longer available", component.Markup);
        Assert.Contains("Browse All Cars", component.Markup);
    }

    [Fact]
    public void CarDetailsPage_ShowsBreadcrumb()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var mockNavigationManager = new Mock<NavigationManager>();
        
        var testCar = new Car
        {
            Id = 1,
            Make = "Toyota",
            Model = "Camry",
            Year = 2023,
            Price = 30000
        };

        mockCarService.Setup(s => s.GetCarByIdAsync(1)).ReturnsAsync(testCar);
        Services.AddSingleton(mockCarService.Object);
        Services.AddSingleton(mockNavigationManager.Object);

        // Act
        var component = RenderComponent<CarDetails>(parameters => parameters.Add(p => p.Id, 1));

        // Assert
        Assert.Contains("breadcrumb", component.Markup);
        Assert.Contains("Home", component.Markup);
        Assert.Contains("Cars", component.Markup);
        Assert.Contains("Toyota Camry", component.Markup);
    }

    [Fact]
    public void CarDetailsPage_ShowsAvailabilityBadge()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var mockNavigationManager = new Mock<NavigationManager>();
        
        var testCar = new Car
        {
            Id = 1,
            Make = "Toyota",
            Model = "Camry",
            Year = 2023,
            Price = 30000,
            IsAvailable = true
        };

        mockCarService.Setup(s => s.GetCarByIdAsync(1)).ReturnsAsync(testCar);
        Services.AddSingleton(mockCarService.Object);
        Services.AddSingleton(mockNavigationManager.Object);

        // Act
        var component = RenderComponent<CarDetails>(parameters => parameters.Add(p => p.Id, 1));

        // Assert
        Assert.Contains("badge bg-success", component.Markup);
        Assert.Contains("Available", component.Markup);
    }

    [Fact]
    public void CarDetailsPage_ShowsQuickActions()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var mockNavigationManager = new Mock<NavigationManager>();
        
        var testCar = new Car
        {
            Id = 1,
            Make = "Toyota",
            Model = "Camry",
            Year = 2023,
            Price = 30000
        };

        mockCarService.Setup(s => s.GetCarByIdAsync(1)).ReturnsAsync(testCar);
        Services.AddSingleton(mockCarService.Object);
        Services.AddSingleton(mockNavigationManager.Object);

        // Act
        var component = RenderComponent<CarDetails>(parameters => parameters.Add(p => p.Id, 1));

        // Assert
        Assert.Contains("Quick Actions", component.Markup);
        Assert.Contains("Back to All Cars", component.Markup);
        Assert.Contains("View Similar Cars", component.Markup);
        Assert.Contains("Share", component.Markup);
        Assert.Contains("Save to Favorites", component.Markup);
    }

    [Fact]
    public void CarDetailsPage_ShowsVehicleDetails()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var mockNavigationManager = new Mock<NavigationManager>();
        
        var testCar = new Car
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
            DateAdded = new DateTime(2023, 6, 15)
        };

        mockCarService.Setup(s => s.GetCarByIdAsync(1)).ReturnsAsync(testCar);
        Services.AddSingleton(mockCarService.Object);
        Services.AddSingleton(mockNavigationManager.Object);

        // Act
        var component = RenderComponent<CarDetails>(parameters => parameters.Add(p => p.Id, 1));

        // Assert
        Assert.Contains("Vehicle Details", component.Markup);
        Assert.Contains("Technical Specifications", component.Markup);
        Assert.Contains("Year:", component.Markup);
        Assert.Contains("Make:", component.Markup);
        Assert.Contains("Model:", component.Markup);
        Assert.Contains("Color:", component.Markup);
        Assert.Contains("Mileage:", component.Markup);
        Assert.Contains("Fuel Type:", component.Markup);
        Assert.Contains("Transmission:", component.Markup);
        Assert.Contains("Date Added:", component.Markup);
    }
}
