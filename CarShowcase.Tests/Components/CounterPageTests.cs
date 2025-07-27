using Bunit;
using CarShowcase.Pages;
using AngleSharp.Dom;

namespace CarShowcase.Tests.Components;

public class CounterPageTests : TestContext
{
    [Fact]
    public void CounterPage_RendersCorrectly()
    {
        // Act
        var component = RenderComponent<Counter>();

        // Assert
        Assert.Contains("Counter", component.Markup);
        Assert.Contains("Current count: 0", component.Markup);
        Assert.Contains("Click me", component.Markup);
    }

    [Fact]
    public void CounterPage_InitialCount_IsZero()
    {
        // Act
        var component = RenderComponent<Counter>();

        // Assert
        var countDisplay = component.Find("p[role='status']");
        Assert.Contains("Current count: 0", countDisplay.TextContent);
    }

    [Fact]
    public void CounterPage_HasClickButton()
    {
        // Act
        var component = RenderComponent<Counter>();

        // Assert
        var button = component.Find("button");
        Assert.NotNull(button);
        Assert.Contains("btn", button.ClassList);
        Assert.Contains("btn-primary", button.ClassList);
        Assert.Equal("Click me", button.TextContent);
    }

    [Fact]
    public void CounterPage_ClickButton_IncrementsCount()
    {
        // Arrange
        var component = RenderComponent<Counter>();
        var button = component.Find("button");
        var countDisplay = component.Find("p[role='status']");

        // Verify initial state
        Assert.Contains("Current count: 0", countDisplay.TextContent);

        // Act
        button.Click();

        // Assert
        countDisplay = component.Find("p[role='status']");
        Assert.Contains("Current count: 1", countDisplay.TextContent);
    }

    [Fact]
    public void CounterPage_MultipleClicks_IncrementsCorrectly()
    {
        // Arrange
        var component = RenderComponent<Counter>();
        var button = component.Find("button");

        // Act - Click multiple times
        for (int i = 0; i < 5; i++)
        {
            button.Click();
        }

        // Assert
        var countDisplay = component.Find("p[role='status']");
        Assert.Contains("Current count: 5", countDisplay.TextContent);
    }

    [Fact]
    public void CounterPage_HasCorrectPageTitle()
    {
        // Act
        var component = RenderComponent<Counter>();

        // Assert
        Assert.Contains("Counter", component.Markup);
        
        // Check for h1 element
        var heading = component.Find("h1");
        Assert.Equal("Counter", heading.TextContent);
    }

    [Fact]
    public void CounterPage_CountDisplay_HasCorrectRole()
    {
        // Act
        var component = RenderComponent<Counter>();

        // Assert
        var countDisplay = component.Find("p[role='status']");
        Assert.NotNull(countDisplay);
        Assert.Equal("status", countDisplay.GetAttribute("role"));
    }

    [Fact]
    public void CounterPage_ButtonClick_UpdatesDisplayImmediately()
    {
        // Arrange
        var component = RenderComponent<Counter>();
        var button = component.Find("button");

        // Act & Assert - Test immediate updates
        for (int expectedCount = 1; expectedCount <= 3; expectedCount++)
        {
            button.Click();
            var countDisplay = component.Find("p[role='status']");
            Assert.Contains($"Current count: {expectedCount}", countDisplay.TextContent);
        }
    }

    [Fact]
    public void CounterPage_Structure_IsCorrect()
    {
        // Act
        var component = RenderComponent<Counter>();

        // Assert
        // Check for PageTitle (this would be in the rendered markup as a comment or meta tag)
        // We can't directly test PageTitle component, but we can verify the structure

        // Check for h1
        var heading = component.Find("h1");
        Assert.NotNull(heading);
        Assert.Equal("Counter", heading.TextContent);

        // Check for count display paragraph
        var countParagraph = component.Find("p[role='status']");
        Assert.NotNull(countParagraph);

        // Check for button
        var button = component.Find("button.btn.btn-primary");
        Assert.NotNull(button);
    }
}
