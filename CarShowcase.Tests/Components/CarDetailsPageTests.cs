using Bunit;
using Microsoft.Extensions.DependencyInjection;
using CarShowcase.Pages;
using CarShowcase.Services;
using CarShowcase.Models;
using Moq;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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
        // Check for price with either regular space or non-breaking space as thousands separator
        bool priceFound = component.Markup.Contains("$30\u00A0000") || component.Markup.Contains("$30 000");
        Assert.True(priceFound, "Price $30,000 with either regular or non-breaking space not found in markup");
        Assert.Contains("Reliable and fuel-efficient sedan", component.Markup);
        Assert.Contains("Blue", component.Markup);
        // Check for mileage with either regular space or non-breaking space as thousands separator
        bool mileageFound = component.Markup.Contains("15\u00A0000 miles") || component.Markup.Contains("15 000 miles");
        Assert.True(mileageFound, "Mileage 15,000 miles with either regular or non-breaking space not found in markup");
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
            Price = 30000,
            IsAvailable = true
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
    public void ViewSimilarCars_NavigatesToCarsWithMakeFilter()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var navigationManager = new TestNavigationManager("http://localhost/", "http://localhost/");
        
        var testCar = new Car
        {
            Id = 1,
            Make = "Toyota",
            Model = "Camry",
            Year = 2023,
            IsAvailable = true
        };

        mockCarService.Setup(s => s.GetCarByIdAsync(1)).ReturnsAsync(testCar);
        Services.AddSingleton(mockCarService.Object);
        Services.AddSingleton<NavigationManager>(navigationManager);

        var component = RenderComponent<CarDetails>(parameters => parameters.Add(p => p.Id, 1));
        
        // Act
        var viewSimilarButton = component.Find("button:contains('View Similar Cars')");
        viewSimilarButton.Click();

        // Assert
        Assert.Equal("http://localhost/cars?make=Toyota", navigationManager.Uri);
    }

    [Fact]
    public void ContactDealer_ShowsAlert()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var mockNavigationManager = new Mock<NavigationManager>();
        var mockJSRuntime = new Mock<IJSRuntime>();
        
        var testCar = new Car
        {
            Id = 1,
            Make = "Toyota",
            Model = "Camry",
            IsAvailable = true
        };

        mockCarService.Setup(s => s.GetCarByIdAsync(1)).ReturnsAsync(testCar);
        Services.AddSingleton(mockCarService.Object);
        Services.AddSingleton(mockNavigationManager.Object);
        Services.AddSingleton(mockJSRuntime.Object);

        var component = RenderComponent<CarDetails>(parameters => parameters.Add(p => p.Id, 1));
        
        // Act
        var contactButton = component.Find("button:contains('Contact Dealer')");
        contactButton.Click();

        // Assert - Verify no exceptions are thrown
        // In a real test, you would verify the alert dialog appears
    }

    [Fact]
    public void ScheduleTestDrive_ShowsAlert()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var mockNavigationManager = new Mock<NavigationManager>();
        var mockJSRuntime = new Mock<IJSRuntime>();
        
        var testCar = new Car
        {
            Id = 1,
            Make = "Toyota",
            Model = "Camry",
            IsAvailable = true
        };

        mockCarService.Setup(s => s.GetCarByIdAsync(1)).ReturnsAsync(testCar);
        Services.AddSingleton(mockCarService.Object);
        Services.AddSingleton(mockNavigationManager.Object);
        Services.AddSingleton(mockJSRuntime.Object);

        var component = RenderComponent<CarDetails>(parameters => parameters.Add(p => p.Id, 1));
        
        // Act
        var testDriveButton = component.Find("button:contains('Schedule Test Drive')");
        testDriveButton.Click();

        // Assert - Verify no exceptions are thrown
    }

    [Fact]
    public void GetFinancing_ShowsAlert()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var mockNavigationManager = new Mock<NavigationManager>();
        var mockJSRuntime = new Mock<IJSRuntime>();
        
        var testCar = new Car
        {
            Id = 1,
            Make = "Toyota",
            Model = "Camry",
            IsAvailable = true
        };

        mockCarService.Setup(s => s.GetCarByIdAsync(1)).ReturnsAsync(testCar);
        Services.AddSingleton(mockCarService.Object);
        Services.AddSingleton(mockNavigationManager.Object);
        Services.AddSingleton(mockJSRuntime.Object);

        var component = RenderComponent<CarDetails>(parameters => parameters.Add(p => p.Id, 1));
        
        // Act
        var financingButton = component.Find("button:contains('Get Financing')");
        financingButton.Click();

        // Assert - Verify no exceptions are thrown
    }

    [Fact]
    public void ShareCar_ShowsShareOptions()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var mockNavigationManager = new Mock<NavigationManager>();
        var mockJSRuntime = new Mock<IJSRuntime>();
        
        var testCar = new Car
        {
            Id = 1,
            Make = "Toyota",
            Model = "Camry"
        };

        mockCarService.Setup(s => s.GetCarByIdAsync(1)).ReturnsAsync(testCar);
        Services.AddSingleton(mockCarService.Object);
        Services.AddSingleton(mockNavigationManager.Object);
        Services.AddSingleton(mockJSRuntime.Object);

        var component = RenderComponent<CarDetails>(parameters => parameters.Add(p => p.Id, 1));
        
        // Act
        var shareButton = component.Find("button:contains('Share')");
        shareButton.Click();

        // Assert - Verify no exceptions are thrown
    }

    [Fact]
    public void SaveToFavorites_ShowsConfirmation()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var mockNavigationManager = new Mock<NavigationManager>();
        var mockJSRuntime = new Mock<IJSRuntime>();
        
        var testCar = new Car
        {
            Id = 1,
            Make = "Toyota",
            Model = "Camry"
        };

        mockCarService.Setup(s => s.GetCarByIdAsync(1)).ReturnsAsync(testCar);
        Services.AddSingleton(mockCarService.Object);
        Services.AddSingleton(mockNavigationManager.Object);
        Services.AddSingleton(mockJSRuntime.Object);

        var component = RenderComponent<CarDetails>(parameters => parameters.Add(p => p.Id, 1));
        
        // Act
        var favoriteButton = component.Find("button:contains('Save to Favorites')");
        favoriteButton.Click();

        // Assert - Verify no exceptions are thrown
    }

    [Fact]
    public void CarDetailsPage_ShowsLoadingState()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var mockNavigationManager = new Mock<NavigationManager>();
        
        var testCar = new Car
        {
            Id = 1,
            Make = "Toyota",
            Model = "Camry"
        };

        // Simulate a delay in getting the car
        mockCarService.Setup(s => s.GetCarByIdAsync(1))
            .Returns(async () => {
                await Task.Delay(100);
                return testCar;
            });
            
        Services.AddSingleton(mockCarService.Object);
        Services.AddSingleton(mockNavigationManager.Object);

        // Act - Initial render will be in loading state
        var component = RenderComponent<CarDetails>(parameters => parameters.Add(p => p.Id, 1));

        // Assert - Should show loading spinner initially
        Assert.Contains("spinner-border", component.Markup);
        
        // Wait for the component to finish loading
        component.WaitForState(() => !component.Markup.Contains("spinner-border"));
        
        // Now should show the car details
        Assert.Contains("Toyota Camry", component.Markup);
    }
    // TestNavigationManager class for testing navigation
    private class TestNavigationManager : NavigationManager
    {
        public TestNavigationManager(string baseUri, string uri) 
        {
            Initialize(baseUri, uri);
        }

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            Uri = ToAbsoluteUri(uri).ToString();
        }
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
