using Bunit;
using CarShowcase.Components;
using CarShowcase.Models;
using AngleSharp.Dom;

namespace CarShowcase.Tests.Components;

public class CarCardTests : TestContext
{
    private Car GetTestCar() => new Car
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
        Description = "Reliable sedan",
        ImageUrl = "test-image.jpg",
        IsAvailable = true
    };

    [Fact]
    public void CarCard_RendersCorrectly()
    {
        // Arrange
        var car = GetTestCar();

        // Act
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car));

        // Assert
        Assert.Contains("2023 Toyota Camry", component.Markup);
        Assert.Contains("$30,000", component.Markup);
        Assert.Contains("test-image.jpg", component.Markup);
        Assert.Contains("View Details", component.Markup);
    }

    [Fact]
    public void CarCard_ShowsDetailsWhenEnabled()
    {
        // Arrange
        var car = GetTestCar();

        // Act
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car)
                     .Add(p => p.ShowDetails, true));

        // Assert
        Assert.Contains("Blue", component.Markup);
        Assert.Contains("15,000 miles", component.Markup);
        Assert.Contains("Gasoline", component.Markup);
        Assert.Contains("Automatic", component.Markup);
    }

    [Fact]
    public void CarCard_HidesDetailsWhenDisabled()
    {
        // Arrange
        var car = GetTestCar();

        // Act
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car)
                     .Add(p => p.ShowDetails, false));

        // Assert
        Assert.DoesNotContain("Blue •", component.Markup);
        Assert.DoesNotContain("15,000 miles", component.Markup);
    }

    [Fact]
    public void CarCard_ShowsDescriptionWhenEnabled()
    {
        // Arrange
        var car = GetTestCar();

        // Act
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car)
                     .Add(p => p.ShowDescription, true));

        // Assert
        Assert.Contains("Reliable sedan", component.Markup);
    }

    [Fact]
    public void CarCard_HidesDescriptionWhenDisabled()
    {
        // Arrange
        var car = GetTestCar();

        // Act
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car)
                     .Add(p => p.ShowDescription, false));

        // Assert
        Assert.DoesNotContain("Reliable sedan", component.Markup);
    }

    [Fact]
    public void CarCard_ShowsAvailabilityBadgeWhenEnabled()
    {
        // Arrange
        var car = GetTestCar();

        // Act
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car)
                     .Add(p => p.ShowAvailability, true));

        // Assert
        Assert.Contains("Available", component.Markup);
        Assert.Contains("bg-success", component.Markup);
    }

    [Fact]
    public void CarCard_ShowsSoldBadgeForUnavailableCar()
    {
        // Arrange
        var car = GetTestCar();
        car.IsAvailable = false;

        // Act
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car)
                     .Add(p => p.ShowBadge, true));

        // Assert
        Assert.Contains("Sold", component.Markup);
        Assert.Contains("bg-danger", component.Markup);
    }

    [Fact]
    public void CarCard_ShowsFeaturedBadgeWhenFeatured()
    {
        // Arrange
        var car = GetTestCar();

        // Act
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car)
                     .Add(p => p.IsFeatured, true)
                     .Add(p => p.ShowBadge, true));

        // Assert
        Assert.Contains("Featured", component.Markup);
        Assert.Contains("bg-warning", component.Markup);
    }

    [Fact]
    public void CarCard_ShowsQuickActionsWhenEnabled()
    {
        // Arrange
        var car = GetTestCar();

        // Act
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car)
                     .Add(p => p.ShowQuickActions, true));

        // Assert
        Assert.Contains("oi-heart", component.Markup);
        Assert.Contains("oi-share", component.Markup);
        Assert.Contains("btn-group", component.Markup);
    }

    [Fact]
    public void CarCard_AppliesCustomCssClass()
    {
        // Arrange
        var car = GetTestCar();
        var customClass = "custom-car-card";

        // Act
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car)
                     .Add(p => p.CssClass, customClass));

        // Assert
        Assert.Contains(customClass, component.Markup);
    }

    [Fact]
    public void CarCard_AppliesCustomButtonClass()
    {
        // Arrange
        var car = GetTestCar();
        var customButtonClass = "btn-success";

        // Act
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car)
                     .Add(p => p.ButtonClass, customButtonClass));

        // Assert
        Assert.Contains(customButtonClass, component.Markup);
    }

    [Fact]
    public void CarCard_HasCorrectViewDetailsLink()
    {
        // Arrange
        var car = GetTestCar();

        // Act
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car));

        // Assert
        var link = component.Find("a[href='/car/1']");
        Assert.NotNull(link);
        Assert.Contains("View Details", link.TextContent);
    }

    [Fact]
    public void CarCard_HasHoverEffects()
    {
        // Arrange
        var car = GetTestCar();

        // Act
        var component = RenderComponent<CarCard>(parameters =>
            parameters.Add(p => p.Car, car));

        // Assert
        Assert.Contains("hover-lift", component.Markup);
        Assert.Contains("card", component.Markup);
    }
}
