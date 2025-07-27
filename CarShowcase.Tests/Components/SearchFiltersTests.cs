using Bunit;
using CarShowcase.Components;
using AngleSharp.Dom;

namespace CarShowcase.Tests.Components;

public class SearchFiltersTests : TestContext
{
    [Fact]
    public void SearchFilters_RendersCorrectly()
    {
        // Act
        var component = RenderComponent<SearchFilters>();

        // Assert
        Assert.Contains("search-filters", component.Markup);
        Assert.Contains("Search Filters", component.Markup);
        Assert.Contains("card", component.Markup);
    }

    [Fact]
    public void SearchFilters_ShowsCustomTitle()
    {
        // Arrange
        var customTitle = "Advanced Search";

        // Act
        var component = RenderComponent<SearchFilters>(parameters => 
            parameters.Add(p => p.Title, customTitle));

        // Assert
        Assert.Contains(customTitle, component.Markup);
    }

    [Fact]
    public void SearchFilters_ShowsMakeFilterWhenEnabled()
    {
        // Arrange
        var makes = new List<string> { "Toyota", "Honda", "Ford" };

        // Act
        var component = RenderComponent<SearchFilters>(parameters => 
            parameters.Add(p => p.Makes, makes)
                     .Add(p => p.ShowMakeFilter, true));

        // Assert
        Assert.Contains("Make", component.Markup);
        Assert.Contains("Toyota", component.Markup);
        Assert.Contains("Honda", component.Markup);
        Assert.Contains("Ford", component.Markup);
    }

    [Fact]
    public void SearchFilters_HidesMakeFilterWhenDisabled()
    {
        // Act
        var component = RenderComponent<SearchFilters>(parameters => 
            parameters.Add(p => p.ShowMakeFilter, false));

        // Assert
        Assert.DoesNotContain("makeFilter", component.Markup);
    }

    [Fact]
    public void SearchFilters_ShowsModelFilterWhenEnabled()
    {
        // Act
        var component = RenderComponent<SearchFilters>(parameters => 
            parameters.Add(p => p.ShowModelFilter, true));

        // Assert
        Assert.Contains("Model", component.Markup);
        Assert.Contains("modelFilter", component.Markup);
    }

    [Fact]
    public void SearchFilters_ShowsYearFiltersWhenEnabled()
    {
        // Act
        var component = RenderComponent<SearchFilters>(parameters => 
            parameters.Add(p => p.ShowYearFilters, true));

        // Assert
        Assert.Contains("Min Year", component.Markup);
        Assert.Contains("Max Year", component.Markup);
        Assert.Contains("minYear", component.Markup);
        Assert.Contains("maxYear", component.Markup);
    }

    [Fact]
    public void SearchFilters_ShowsPriceFilterWhenEnabled()
    {
        // Act
        var component = RenderComponent<SearchFilters>(parameters => 
            parameters.Add(p => p.ShowPriceFilter, true));

        // Assert
        Assert.Contains("Max Price", component.Markup);
        Assert.Contains("maxPrice", component.Markup);
        Assert.Contains("$", component.Markup);
    }

    [Fact]
    public void SearchFilters_ShowsFuelTypeFilterWhenEnabled()
    {
        // Act
        var component = RenderComponent<SearchFilters>(parameters => 
            parameters.Add(p => p.ShowFuelTypeFilter, true));

        // Assert
        Assert.Contains("Fuel Type", component.Markup);
        Assert.Contains("Gasoline", component.Markup);
        Assert.Contains("Electric", component.Markup);
        Assert.Contains("Hybrid", component.Markup);
    }

    [Fact]
    public void SearchFilters_ShowsTransmissionFilterWhenEnabled()
    {
        // Act
        var component = RenderComponent<SearchFilters>(parameters => 
            parameters.Add(p => p.ShowTransmissionFilter, true));

        // Assert
        Assert.Contains("Transmission", component.Markup);
        Assert.Contains("Automatic", component.Markup);
        Assert.Contains("Manual", component.Markup);
        Assert.Contains("CVT", component.Markup);
    }

    [Fact]
    public void SearchFilters_ShowsActionsWhenEnabled()
    {
        // Act
        var component = RenderComponent<SearchFilters>(parameters => 
            parameters.Add(p => p.ShowActions, true));

        // Assert
        Assert.Contains("Clear Filters", component.Markup);
        Assert.Contains("Search", component.Markup);
    }

    [Fact]
    public void SearchFilters_HidesActionsWhenDisabled()
    {
        // Act
        var component = RenderComponent<SearchFilters>(parameters => 
            parameters.Add(p => p.ShowActions, false));

        // Assert
        Assert.DoesNotContain("Clear Filters", component.Markup);
        Assert.DoesNotContain("Search", component.Markup);
    }

    [Fact]
    public void SearchFilters_ShowsSaveSearchWhenEnabled()
    {
        // Act
        var component = RenderComponent<SearchFilters>(parameters => 
            parameters.Add(p => p.ShowSaveSearch, true));

        // Assert
        Assert.Contains("Save Search", component.Markup);
    }

    [Fact]
    public void SearchFilters_AppliesCustomCssClass()
    {
        // Arrange
        var customClass = "my-search-filters";

        // Act
        var component = RenderComponent<SearchFilters>(parameters => 
            parameters.Add(p => p.CssClass, customClass));

        // Assert
        Assert.Contains(customClass, component.Markup);
    }

    [Fact]
    public void SearchFilters_HasCorrectFormStructure()
    {
        // Act
        var component = RenderComponent<SearchFilters>();

        // Assert
        Assert.Contains("form-label", component.Markup);
        Assert.Contains("form-select", component.Markup);
        Assert.Contains("form-control", component.Markup);
    }

    [Fact]
    public void SearchFilters_HasAccessibleLabels()
    {
        // Act
        var component = RenderComponent<SearchFilters>();

        // Assert
        var makeLabel = component.Find("label[for='makeFilter']");
        Assert.NotNull(makeLabel);
        Assert.Equal("Make", makeLabel.TextContent.Trim());
    }

    [Fact]
    public void SearchFilters_HasCorrectInputTypes()
    {
        // Act
        var component = RenderComponent<SearchFilters>();

        // Assert
        var yearInputs = component.FindAll("input[type='number']");
        Assert.NotEmpty(yearInputs);
        
        var selectElements = component.FindAll("select");
        Assert.NotEmpty(selectElements);
    }
}
