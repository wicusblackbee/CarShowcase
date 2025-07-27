using Bunit;
using CarShowcase.Shared;
using AngleSharp.Dom;

namespace CarShowcase.Tests.Components;

public class NavMenuTests : TestContext
{
    [Fact]
    public void NavMenu_RendersCorrectly()
    {
        // Act
        var component = RenderComponent<NavMenu>();

        // Assert
        Assert.Contains("CarShowcase", component.Markup);
        Assert.Contains("navbar-brand", component.Markup);
        Assert.Contains("navbar-toggler", component.Markup);
    }

    [Fact]
    public void NavMenu_ContainsAllNavigationLinks()
    {
        // Act
        var component = RenderComponent<NavMenu>();

        // Assert
        // Home link
        var homeLinks = component.FindAll("a[href='']");
        Assert.NotEmpty(homeLinks);
        Assert.Contains("Home", component.Markup);
        
        // Browse Cars link
        var carsLinks = component.FindAll("a[href='cars']");
        Assert.NotEmpty(carsLinks);
        Assert.Contains("Browse Cars", component.Markup);
        
        // Counter link
        var counterLinks = component.FindAll("a[href='counter']");
        Assert.NotEmpty(counterLinks);
        Assert.Contains("Counter", component.Markup);
        
        // Weather link has been removed from the NavMenu
    }

    [Fact]
    public void NavMenu_HasCorrectIcons()
    {
        // Act
        var component = RenderComponent<NavMenu>();

        // Assert
        Assert.Contains("oi-home", component.Markup);
        Assert.Contains("oi-list-rich", component.Markup);
        Assert.Contains("oi-plus", component.Markup);
    }

    [Fact]
    public void NavMenu_HasToggleButton()
    {
        // Act
        var component = RenderComponent<NavMenu>();

        // Assert
        var toggleButton = component.Find(".navbar-toggler");
        Assert.NotNull(toggleButton);
        Assert.Contains("Navigation menu", toggleButton.GetAttribute("title"));
    }

    [Fact]
    public void NavMenu_InitiallyCollapsed()
    {
        // Act
        var component = RenderComponent<NavMenu>();

        // Assert
        var navMenu = component.Find(".nav-scrollable");
        Assert.Contains("collapse", navMenu.ClassList);
    }

    [Fact]
    public void NavMenu_ToggleExpansion_WhenButtonClicked()
    {
        // Arrange
        var component = RenderComponent<NavMenu>();
        var toggleButton = component.Find(".navbar-toggler");
        var navMenu = component.Find(".nav-scrollable");

        // Verify initially collapsed
        Assert.Contains("collapse", navMenu.ClassList);

        // Act - Click toggle button
        toggleButton.Click();

        // Assert - Should be expanded (no collapse class)
        navMenu = component.Find(".nav-scrollable");
        Assert.DoesNotContain("collapse", navMenu.ClassList);

        // Act - Click toggle button again
        toggleButton.Click();

        // Assert - Should be collapsed again
        navMenu = component.Find(".nav-scrollable");
        Assert.Contains("collapse", navMenu.ClassList);
    }

    [Fact]
    public void NavMenu_CollapseWhenNavMenuClicked()
    {
        // Arrange
        var component = RenderComponent<NavMenu>();
        var toggleButton = component.Find(".navbar-toggler");
        var navMenu = component.Find(".nav-scrollable");

        // Expand the menu first
        toggleButton.Click();
        navMenu = component.Find(".nav-scrollable");
        Assert.DoesNotContain("collapse", navMenu.ClassList);

        // Act - Click on the nav menu area
        navMenu.Click();

        // Assert - Should be collapsed
        navMenu = component.Find(".nav-scrollable");
        Assert.Contains("collapse", navMenu.ClassList);
    }

    [Fact]
    public void NavMenu_HasCorrectNavLinkStructure()
    {
        // Act
        var component = RenderComponent<NavMenu>();

        // Assert
        var navItems = component.FindAll(".nav-item");
        Assert.Equal(3, navItems.Count); // Home, Browse Cars, Counter

        var navLinks = component.FindAll(".nav-link");
        Assert.Equal(3, navLinks.Count);

        // Check that each nav-item contains a nav-link
        foreach (var navItem in navItems)
        {
            var navLink = navItem.QuerySelector(".nav-link");
            Assert.NotNull(navLink);
        }
    }

    [Fact]
    public void NavMenu_HomeLink_HasCorrectMatchAttribute()
    {
        // Act
        var component = RenderComponent<NavMenu>();

        // Assert
        var homeNavLink = component.Find("a[href='']");
        // Note: In bUnit, we can't easily test the Match attribute directly,
        // but we can verify the link exists and has the correct href
        Assert.NotNull(homeNavLink);
        Assert.Equal("", homeNavLink.GetAttribute("href"));
    }
}
