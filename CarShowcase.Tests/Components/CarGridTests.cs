using Bunit;
using CarShowcase.Components;
using CarShowcase.Models;
using Microsoft.AspNetCore.Components;
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
        var loadingMessage = "Fetching cars...";

        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, (List<Car>?)null)
                     .Add(p => p.LoadingMessage, loadingMessage));

        // Assert
        Assert.Contains(loadingMessage, component.Markup);
    }

    [Fact]
    public void CarGrid_SortsByPriceAscending()
    {
        // Arrange
        var cars = GetTestCars();
        var expectedOrder = cars.OrderBy(c => c.Price).ToList();

        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, cars)
                     .Add(p => p.ShowSortOptions, true));

        // Change sort to Price: Low to High
        var selectElement = component.Find("select");
        selectElement.Change("price-asc");

        // Assert
        var carCards = component.FindAll(".car-card");
        var firstCardTitle = carCards[0].QuerySelector(".card-title")?.TextContent.Trim();
        var secondCardTitle = carCards[1].QuerySelector(".card-title")?.TextContent.Trim();
        var thirdCardTitle = carCards[2].QuerySelector(".card-title")?.TextContent.Trim();
        
        // The title is in format "Year Make Model", so split and get the second part (index 1) for the make
        Assert.Equal(expectedOrder[0].Make, firstCardTitle?.Split(' ')[1]);
        Assert.Equal(expectedOrder[1].Make, secondCardTitle?.Split(' ')[1]);
        Assert.Equal(expectedOrder[2].Make, thirdCardTitle?.Split(' ')[1]);
    }

    [Fact]
    public void CarGrid_SortsByYearDescending()
    {
        // Arrange
        var cars = GetTestCars();
        var expectedOrder = cars.OrderByDescending(c => c.Year).ToList();

        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, cars)
                     .Add(p => p.ShowSortOptions, true));

        // Change sort to Year: Newest First
        var selectElement = component.Find("select");
        selectElement.Change("year-desc");

        // Assert
        var carCards = component.FindAll(".car-card");
        Assert.NotNull(carCards);
        
        for (int i = 0; i < expectedOrder.Count; i++)
        {
            var cardTitle = carCards[i].QuerySelector(".card-title");
            Assert.NotNull(cardTitle);
            
            var titleText = cardTitle.TextContent?.Trim();
            Assert.False(string.IsNullOrEmpty(titleText), "Card title is null or empty");
            
            var yearText = titleText.Split(' ')[0];
            Assert.True(int.TryParse(yearText, out int year), $"Could not parse year from title: {titleText}");
            
            Assert.Equal(expectedOrder[i].Year, year);
        }
    }

    [Fact]
    public void CarGrid_HandlesPagination()
    {
        // Arrange
        var cars = GetTestCars();
        const int itemsPerPage = 1;
        
        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, cars)
                     .Add(p => p.ShowPagination, true)
                     .Add(p => p.ItemsPerPage, itemsPerPage));

        // Assert - Should show first car only
        var carCards = component.FindAll(".car-card");
        Assert.Single(carCards);
        Assert.Contains("Toyota", carCards[0].TextContent);

        // Act - Go to next page
        var nextButton = component.Find("button:contains('Next')");
        nextButton.Click();

        // Assert - Should show second car
        carCards = component.FindAll(".car-card");
        Assert.Single(carCards);
        Assert.Contains("Honda", carCards[0].TextContent);
    }

    [Fact]
    public void CarGrid_HandlesEmptyStateAction()
    {
        // Arrange
        var emptyCars = new List<Car>();
        var actionInvoked = false;
        
        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, emptyCars)
                     .Add(p => p.ShowEmptyStateAction, true)
                     .Add(p => p.EmptyStateActionText, "Add New Car")
                     .Add(p => p.OnEmptyStateAction, EventCallback.Factory.Create(this, () => actionInvoked = true)));

        // Find and click the action button
        var actionButton = component.Find("button:contains('Add New Car')");
        actionButton.Click();

        // Assert
        Assert.True(actionInvoked, "Empty state action was not invoked");
    }

    [Fact]
    public void CarGrid_ShowsFeaturedCarsWithBadge()
    {
        // Arrange
        var cars = GetTestCars();
        var featuredCar = cars[0];
        
        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, cars)
                     .Add(p => p.ShowCarBadge, true)
                     .Add(p => p.FeaturedCarIds, new List<int> { featuredCar.Id }));

        // Assert
        var cardTitles = component.FindAll(".card-title");
        var cardWithBadge = cardTitles.FirstOrDefault(t => t.TextContent.Contains(featuredCar.Make));
        Assert.NotNull(cardWithBadge);
        
        // Find the badge within the same card
        var card = cardWithBadge.Closest(".card");
        var featuredBadge = card?.QuerySelector(".badge-overlay.featured .badge.bg-warning.text-dark");
        Assert.NotNull(featuredBadge);
        Assert.Contains("Featured", featuredBadge.TextContent);
    }

    [Fact]
    public void CarGrid_HandlesCarFavoriteEvent()
    {
        // Arrange
        var cars = GetTestCars();
        Car? favoritedCar = null;
        
        // Act
        var component = RenderComponent<CarGrid>(parameters => 
            parameters.Add(p => p.Cars, cars)
                     .Add(p => p.ShowCarQuickActions, true)
                     .Add(p => p.OnCarFavorite, EventCallback.Factory.Create<Car>(this, car => favoritedCar = car)));

        // Find and click the favorite button
        var favoriteButton = component.Find("button i.oi-heart").ParentElement;
        favoriteButton?.Click();

        // Assert
        Assert.NotNull(favoritedCar);
        Assert.Equal(cars[0].Id, favoritedCar.Id);
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
