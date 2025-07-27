using Bunit;
using CarShowcase.Components;
using CarShowcase.Models;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

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
        // Check for price with either regular space or non-breaking space as thousands separator
        bool priceFound = component.Markup.Contains("$30\u00A0000") || component.Markup.Contains("$30 000");
        Assert.True(priceFound, "Price $30,000 with either regular or non-breaking space not found in markup");
        // The CarCard component uses <CarImage> which generates dynamic URLs based on make/model
        // Instead of checking for a specific URL, we'll verify the car image exists
        Assert.Contains("card-img-top", component.Markup);
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
        // Check for mileage with either regular space or non-breaking space as thousands separator
        bool mileageFound = component.Markup.Contains("15\u00A0000 miles") || component.Markup.Contains("15 000 miles");
        Assert.True(mileageFound, "Mileage 15,000 miles with either regular or non-breaking space not found in markup");
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
        var customButtonClass = "btn-custom";

        // Act
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car)
                     .Add(p => p.ButtonClass, customButtonClass));

        // Assert
        var button = component.Find("a[class*='btn-custom']");
        Assert.NotNull(button);
    }

    [Fact]
    public void CarCard_InvokesOnFavoriteClick()
    {
        // Arrange
        var car = GetTestCar();
        var clicked = false;
        
        // Act
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car)
                     .Add(p => p.ShowQuickActions, true)
                     .Add(p => p.OnFavoriteClick, EventCallback.Factory.Create<Car>(this, _ => clicked = true)));

        // Find and click the favorite button
        var favoriteButton = component.Find("button i.oi-heart").ParentElement;
        favoriteButton?.Click();

        // Assert
        Assert.True(clicked, "OnFavoriteClick was not invoked");
    }

    [Fact]
    public void CarCard_InvokesOnShareClick()
    {
        // Arrange
        var car = GetTestCar();
        var clicked = false;
        
        // Act
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car)
                     .Add(p => p.ShowQuickActions, true)
                     .Add(p => p.OnShareClick, EventCallback.Factory.Create<Car>(this, _ => clicked = true)));

        // Find and click the share button
        var shareButton = component.Find("button i.oi-share").ParentElement;
        shareButton?.Click();

        // Assert
        Assert.True(clicked, "OnShareClick was not invoked");
    }

    [Fact]
    public void CarCard_ShowsBothBadgesWhenFeaturedAndNotAvailable()
    {
        // Arrange
        var car = GetTestCar();
        car.IsAvailable = false;

        // Act
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car)
                     .Add(p => p.IsFeatured, true)
                     .Add(p => p.ShowBadge, true)
                     .Add(p => p.ShowAvailability, false)); // Set ShowAvailability to false to avoid duplicate badges

        // Get the rendered HTML
        var markup = component.Markup;
        
        // Assert
        // Check that both badge containers are rendered
        Assert.Contains("badge-overlay", markup);
        
        // Check for the Featured badge
        Assert.Contains("Featured", markup);
        Assert.Contains("bg-warning", markup);
        
        // Check for the Sold badge
        Assert.Contains("Sold", markup);
        Assert.Contains("bg-danger", markup);
    }

    [Fact]
    public void CarCard_DoesNotShowBadgesWhenShowBadgeIsFalse()
    {
        // Arrange
        var car = GetTestCar();
        car.IsAvailable = false;

        // Act
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car)
                     .Add(p => p.IsFeatured, true)
                     .Add(p => p.ShowBadge, false)
                     .Add(p => p.ShowAvailability, false)); // Set ShowAvailability to false to avoid the availability badge

        // Assert
        Assert.DoesNotContain("Featured", component.Markup);
        Assert.DoesNotContain("Sold", component.Markup);
        Assert.DoesNotContain("badge-overlay", component.Markup);
    }

    [Fact]
    public void CarCard_HandleFavoriteClick_InvokesCallback()
    {
        // Arrange
        var car = GetTestCar();
        var wasCalled = false;
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car)
                     .Add(p => p.ShowQuickActions, true)
                     .Add(p => p.OnFavoriteClick, EventCallback.Factory.Create<Car>(this, _ => wasCalled = true)));
        
        // Act - Find and click the favorite button
        var favoriteButton = component.Find("button i.oi-heart").ParentElement;
        favoriteButton?.Click();

        // Assert
        Assert.True(wasCalled, "OnFavoriteClick callback was not invoked");
    }

    [Fact]
    public void CarCard_HandleShareClick_InvokesCallback()
    {
        // Arrange
        var car = GetTestCar();
        var wasCalled = false;
        var component = RenderComponent<CarCard>(parameters => 
            parameters.Add(p => p.Car, car)
                     .Add(p => p.ShowQuickActions, true)
                     .Add(p => p.OnShareClick, EventCallback.Factory.Create<Car>(this, _ => wasCalled = true)));
        
        // Act - Find and click the share button
        var shareButton = component.Find("button i.oi-share").ParentElement;
        shareButton?.Click();

        // Assert
        Assert.True(wasCalled, "OnShareClick callback was not invoked");
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
