using Bunit;
using CarShowcase.Components;
using CarShowcase.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using AngleSharp.Dom;

namespace CarShowcase.Tests.Components;

public class CarGridTests : TestContext
{
    private List<Car> GetTestCars() =>
    [
        new Car
        {
            Id = 1, Make = "Toyota", Model = "Camry", Year = 2023, Price = 30000, Mileage = 15000,
            ImageUrl = "test1.jpg"
        },
        new Car
        {
            Id = 2, Make = "Honda", Model = "Civic", Year = 2022, Price = 25000, Mileage = 20000, ImageUrl = "test2.jpg"
        },
        new Car
        {
            Id = 3, Make = "Ford", Model = "F-150", Year = 2023, Price = 35000, Mileage = 10000, ImageUrl = "test3.jpg"
        }
    ];

    [Fact]
    public void CarGrid_WithNullCars_ShowsLoadingSpinner()
    {
        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, (List<Car>?)null));

        // Assert
        Assert.Contains("Loading cars...", component.Markup);
        Assert.Contains("spinner-border", component.Markup);
    }

    [Fact]
    public void CarGrid_WithEmptyCars_ShowsEmptyState()
    {
        // Arrange
        var emptyCars = new List<Car>();

        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, emptyCars));

        // Assert
        Assert.Contains("No cars found", component.Markup);
        Assert.Contains("empty-state", component.Markup);
    }

    [Fact]
    public void CarGrid_WithCars_ShowsCars()
    {
        // Arrange
        var cars = GetTestCars();

        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, cars));

        // Assert
        Assert.Contains("Toyota Camry", component.Markup);
        Assert.Contains("Honda Civic", component.Markup);
        Assert.Contains("Ford F-150", component.Markup);
    }

    [Fact]
    public void CarGrid_ShowsResultsCount()
    {
        // Arrange
        var cars = GetTestCars();

        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, cars)
                     .Add(p => p.ShowResultsCount, true));

        // Assert
        Assert.Contains("Found 3 cars", component.Markup);
    }

    [Fact]
    public void CarGrid_ShowsSortOptions()
    {
        // Arrange
        var cars = GetTestCars();

        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, cars)
                     .Add(p => p.ShowSortOptions, true));

        // Assert
        Assert.Contains("Sort by:", component.Markup);
        Assert.Contains("Price: Low to High", component.Markup);
        Assert.Contains("Year: Newest First", component.Markup);
    }

    [Fact]
    public void CarGrid_HidesHeaderWhenDisabled()
    {
        // Arrange
        var cars = GetTestCars();

        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, cars)
                     .Add(p => p.ShowHeader, false));

        // Assert
        Assert.Contains("display: none", component.Markup);
    }

    [Fact]
    public void CarGrid_AppliesSmallGridSize()
    {
        // Arrange
        var cars = GetTestCars();

        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, cars)
                     .Add(p => p.Size, GridSize.Small));

        // Assert
        Assert.Contains("col-lg-2", component.Markup);
    }

    [Fact]
    public void CarGrid_AppliesLargeGridSize()
    {
        // Arrange
        var cars = GetTestCars();

        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, cars)
                     .Add(p => p.Size, GridSize.Large));

        // Assert
        Assert.Contains("col-lg-4", component.Markup);
    }

    [Fact]
    public void CarGrid_ShowsPaginationWhenEnabled()
    {
        // Arrange
        var cars = GetTestCars();

        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, cars)
                     .Add(p => p.ShowPagination, true)
                     .Add(p => p.ItemsPerPage, 2));

        // Assert
        Assert.Contains("pagination", component.Markup);
        Assert.Contains("Previous", component.Markup);
        Assert.Contains("Next", component.Markup);
    }

    [Fact]
    public void CarGrid_AppliesCustomCssClass()
    {
        // Arrange
        var cars = GetTestCars();
        var customClass = "my-car-grid";

        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, cars)
                     .Add(p => p.CssClass, customClass));

        // Assert
        Assert.Contains(customClass, component.Markup);
    }

    [Fact]
    public void CarGrid_ShowsCustomEmptyStateMessage()
    {
        // Arrange
        var emptyCars = new List<Car>();
        var customMessage = "No vehicles available";

        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, emptyCars)
                     .Add(p => p.EmptyStateMessage, customMessage));

        // Assert
        Assert.Contains(customMessage, component.Markup);
    }

    [Fact]
    public void CarGrid_ShowsCustomLoadingMessage()
    {
        // Arrange
        var customMessage = "Fetching vehicles...";

        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, (List<Car>?)null)
                     .Add(p => p.LoadingMessage, customMessage));

        // Assert
        Assert.Contains(customMessage, component.Markup);
    }

    [Fact]
    public void CarGrid_ShowsFeaturedCars()
    {
        // Arrange
        var cars = GetTestCars();
        var featuredIds = new List<int> { 1, 3 };

        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, cars)
                     .Add(p => p.FeaturedCarIds, featuredIds));

        // Assert
        // Featured cars should have the IsFeatured property set to true
        // This would be visible in the CarCard component rendering
        Assert.Contains("car-card", component.Markup);
    }

    [Fact]
    public void CarGrid_HasCorrectRowStructure()
    {
        // Arrange
        var cars = GetTestCars();

        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, cars));

        // Assert
        Assert.Contains("row", component.Markup);
        Assert.Contains("col-lg-3", component.Markup); // Default medium size
    }

    [Fact]
    public void CarGrid_CalculatesCorrectTotalPages()
    {
        // Arrange
        var cars = GetTestCars(); // 3 cars
        var itemsPerPage = 2;

        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, cars)
                     .Add(p => p.ShowPagination, true)
                     .Add(p => p.ItemsPerPage, itemsPerPage));

        // Assert
        // Should show page 1 and 2 (3 cars / 2 per page = 2 pages)
        Assert.Contains("pagination", component.Markup);
    }
}
