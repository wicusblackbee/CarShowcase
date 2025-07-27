using Bunit;
using Microsoft.Extensions.DependencyInjection;
using CarShowcase.Services;
using CarShowcase.Models;
using Moq;
using IndexPage = CarShowcase.Pages.Index;

namespace CarShowcase.Tests.Components;

public class IndexPageTests : TestContext
{
    [Fact]
    public void IndexPage_RendersCorrectly()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var sampleCars = new List<Car>
        {
            new Car { Id = 1, Make = "Toyota", Model = "Camry", Year = 2023, Price = 30000, Description = "Test car 1", ImageUrl = "test1.jpg" },
            new Car { Id = 2, Make = "Honda", Model = "Civic", Year = 2022, Price = 25000, Description = "Test car 2", ImageUrl = "test2.jpg" },
            new Car { Id = 3, Make = "Ford", Model = "F-150", Year = 2023, Price = 35000, Description = "Test car 3", ImageUrl = "test3.jpg" }
        };

        mockCarService.Setup(s => s.GetAllCarsAsync()).ReturnsAsync(sampleCars);
        Services.AddSingleton(mockCarService.Object);

        // Act
        var component = RenderComponent<IndexPage>();

        // Assert
        Assert.Contains("Premium Car Showcase", component.Markup);
        Assert.Contains("Discover your dream car", component.Markup);
        Assert.Contains("Browse Cars", component.Markup);
        Assert.Contains("Featured Cars", component.Markup);
    }

    [Fact]
    public void IndexPage_ShowsLoadingSpinner_WhenCarsAreNull()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        // Use explicit cast to match expected nullable return type
        mockCarService.Setup(s => s.GetAllCarsAsync()).ReturnsAsync((List<Car>?)null);
        Services.AddSingleton(mockCarService.Object);

        // Act
        var component = RenderComponent<IndexPage>();

        // Assert
        Assert.Contains("spinner-border", component.Markup);
        Assert.Contains("Loading...", component.Markup);
    }

    [Fact]
    public void IndexPage_DisplaysFeaturedCars_WhenCarsAreLoaded()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        // Create sample cars with explicit nullability handling
        List<Car> sampleCars = new List<Car>
        {
            new Car { Id = 1, Make = "Toyota", Model = "Camry", Year = 2023, Price = 30000, Description = "Reliable sedan", ImageUrl = "test1.jpg" },
            new Car { Id = 2, Make = "Honda", Model = "Civic", Year = 2022, Price = 25000, Description = "Sporty compact", ImageUrl = "test2.jpg" }
        };

        mockCarService.Setup(s => s.GetAllCarsAsync()).ReturnsAsync(sampleCars);
        Services.AddSingleton(mockCarService.Object);

        // Act
        var component = RenderComponent<IndexPage>();

        // Assert
        Assert.Contains("2023 Toyota Camry", component.Markup);
        Assert.Contains("2022 Honda Civic", component.Markup);
        // Check for price with either regular space or non-breaking space as thousands separator
        bool price1Found = component.Markup.Contains("$30\u00A0000") || component.Markup.Contains("$30 000");
        Assert.True(price1Found, "Price $30,000 with either regular or non-breaking space not found in markup");
        
        // Check for second price with either regular space or non-breaking space as thousands separator
        bool price2Found = component.Markup.Contains("$25\u00A0000") || component.Markup.Contains("$25 000");
        Assert.True(price2Found, "Price $25,000 with either regular or non-breaking space not found in markup");
        Assert.Contains("Reliable sedan", component.Markup);
        Assert.Contains("Sporty compact", component.Markup);
    }

    [Fact]
    public void IndexPage_LimitsToThreeFeaturedCars()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var sampleCars = new List<Car>
        {
            new Car { Id = 1, Make = "Toyota", Model = "Camry", Year = 2023, Price = 30000, Description = "Car 1", ImageUrl = "test1.jpg" },
            new Car { Id = 2, Make = "Honda", Model = "Civic", Year = 2022, Price = 25000, Description = "Car 2", ImageUrl = "test2.jpg" },
            new Car { Id = 3, Make = "Ford", Model = "F-150", Year = 2023, Price = 35000, Description = "Car 3", ImageUrl = "test3.jpg" },
            new Car { Id = 4, Make = "BMW", Model = "X5", Year = 2023, Price = 55000, Description = "Car 4", ImageUrl = "test4.jpg" },
            new Car { Id = 5, Make = "Audi", Model = "A4", Year = 2023, Price = 42000, Description = "Car 5", ImageUrl = "test5.jpg" }
        };

        mockCarService.Setup(s => s.GetAllCarsAsync()).ReturnsAsync(sampleCars);
        Services.AddSingleton(mockCarService.Object);

        // Act
        var component = RenderComponent<IndexPage>();

        // Assert
        var cardElements = component.FindAll(".card");
        Assert.Equal(3, cardElements.Count);
        
        // Verify first three cars are shown
        Assert.Contains("Toyota Camry", component.Markup);
        Assert.Contains("Honda Civic", component.Markup);
        Assert.Contains("Ford F-150", component.Markup);
        
        // Verify fourth and fifth cars are not shown
        Assert.DoesNotContain("BMW X5", component.Markup);
        Assert.DoesNotContain("Audi A4", component.Markup);
    }

    [Fact]
    public void IndexPage_ContainsViewDetailsLinks()
    {
        // Arrange
        var mockCarService = new Mock<ICarService>();
        var sampleCars = new List<Car>
        {
            new Car { Id = 1, Make = "Toyota", Model = "Camry", Year = 2023, Price = 30000, Description = "Test car", ImageUrl = "test1.jpg" }
        };

        mockCarService.Setup(s => s.GetAllCarsAsync()).ReturnsAsync(sampleCars);
        Services.AddSingleton(mockCarService.Object);

        // Act
        var component = RenderComponent<IndexPage>();

        // Assert
        var viewDetailsLinks = component.FindAll("a[href='/car/1']");
        Assert.NotEmpty(viewDetailsLinks);
        Assert.Contains("View Details", component.Markup);
    }
}
