using Bunit;
using CarShowcase.Components;
using AngleSharp.Dom;

namespace CarShowcase.Tests.Components;

public class HeroSectionTests : TestContext
{
    [Fact]
    public void HeroSection_RendersCorrectly()
    {
        // Act
        var component = RenderComponent<HeroSection>();

        // Assert
        Assert.Contains("hero-section", component.Markup);
        Assert.Contains("hero-content", component.Markup);
        Assert.Contains("hero-overlay", component.Markup);
    }

    [Fact]
    public void HeroSection_ShowsTitle()
    {
        // Arrange
        var title = "Welcome to Our Site";

        // Act
        var component = RenderComponent<HeroSection>(parameters => 
            parameters.Add(p => p.Title, title));

        // Assert
        Assert.Contains(title, component.Markup);
        Assert.Contains("hero-title", component.Markup);
    }

    [Fact]
    public void HeroSection_ShowsSubtitle()
    {
        // Arrange
        var subtitle = "This is our amazing subtitle";

        // Act
        var component = RenderComponent<HeroSection>(parameters => 
            parameters.Add(p => p.Subtitle, subtitle));

        // Assert
        Assert.Contains(subtitle, component.Markup);
        Assert.Contains("hero-subtitle", component.Markup);
    }

    [Fact]
    public void HeroSection_ShowsPrimaryButton()
    {
        // Arrange
        var buttonText = "Get Started";
        var buttonUrl = "/start";
        var buttonIcon = "arrow-right";

        // Act
        var component = RenderComponent<HeroSection>(parameters => 
            parameters.Add(p => p.PrimaryButtonText, buttonText)
                     .Add(p => p.PrimaryButtonUrl, buttonUrl)
                     .Add(p => p.PrimaryButtonIcon, buttonIcon));

        // Assert
        Assert.Contains(buttonText, component.Markup);
        Assert.Contains($"href=\"{buttonUrl}\"", component.Markup);
        Assert.Contains($"oi-{buttonIcon}", component.Markup);
        Assert.Contains("btn-primary", component.Markup);
    }

    [Fact]
    public void HeroSection_ShowsSecondaryButton()
    {
        // Arrange
        var buttonText = "Learn More";
        var buttonUrl = "/learn";
        var buttonIcon = "info";

        // Act
        var component = RenderComponent<HeroSection>(parameters => 
            parameters.Add(p => p.SecondaryButtonText, buttonText)
                     .Add(p => p.SecondaryButtonUrl, buttonUrl)
                     .Add(p => p.SecondaryButtonIcon, buttonIcon));

        // Assert
        Assert.Contains(buttonText, component.Markup);
        Assert.Contains($"href=\"{buttonUrl}\"", component.Markup);
        Assert.Contains($"oi-{buttonIcon}", component.Markup);
        Assert.Contains("btn-outline-light", component.Markup);
    }

    [Fact]
    public void HeroSection_HidesActionsWhenDisabled()
    {
        // Act
        var component = RenderComponent<HeroSection>(parameters => 
            parameters.Add(p => p.ShowActions, false)
                     .Add(p => p.PrimaryButtonText, "Button")
                     .Add(p => p.PrimaryButtonUrl, "/test"));

        // Assert
        Assert.DoesNotContain("hero-actions", component.Markup);
        Assert.DoesNotContain("Button", component.Markup);
    }

    [Fact]
    public void HeroSection_AppliesCustomCssClass()
    {
        // Arrange
        var customClass = "my-hero";

        // Act
        var component = RenderComponent<HeroSection>(parameters => 
            parameters.Add(p => p.CssClass, customClass));

        // Assert
        Assert.Contains(customClass, component.Markup);
    }

    [Fact]
    public void HeroSection_AppliesCustomTitleClass()
    {
        // Arrange
        var titleClass = "custom-title";
        var title = "Test Title";

        // Act
        var component = RenderComponent<HeroSection>(parameters => 
            parameters.Add(p => p.Title, title)
                     .Add(p => p.TitleClass, titleClass));

        // Assert
        Assert.Contains(titleClass, component.Markup);
    }

    [Fact]
    public void HeroSection_AppliesCustomSubtitleClass()
    {
        // Arrange
        var subtitleClass = "custom-subtitle";
        var subtitle = "Test Subtitle";

        // Act
        var component = RenderComponent<HeroSection>(parameters => 
            parameters.Add(p => p.Subtitle, subtitle)
                     .Add(p => p.SubtitleClass, subtitleClass));

        // Assert
        Assert.Contains(subtitleClass, component.Markup);
    }

    [Fact]
    public void HeroSection_AppliesBackgroundGradient()
    {
        // Arrange
        var gradient = "linear-gradient(45deg, red, blue)";

        // Act
        var component = RenderComponent<HeroSection>(parameters => 
            parameters.Add(p => p.BackgroundGradient, gradient));

        // Assert
        Assert.Contains(gradient, component.Markup);
    }

    [Fact]
    public void HeroSection_AppliesBackgroundImage()
    {
        // Arrange
        var imageUrl = "https://example.com/image.jpg";

        // Act
        var component = RenderComponent<HeroSection>(parameters => 
            parameters.Add(p => p.BackgroundImage, imageUrl));

        // Assert
        Assert.Contains(imageUrl, component.Markup);
        Assert.Contains("background-attachment: fixed", component.Markup);
    }

    [Fact]
    public void HeroSection_AppliesCustomButtonClasses()
    {
        // Arrange
        var primaryClass = "custom-primary";
        var secondaryClass = "custom-secondary";

        // Act
        var component = RenderComponent<HeroSection>(parameters => 
            parameters.Add(p => p.PrimaryButtonText, "Primary")
                     .Add(p => p.PrimaryButtonUrl, "/primary")
                     .Add(p => p.PrimaryButtonClass, primaryClass)
                     .Add(p => p.SecondaryButtonText, "Secondary")
                     .Add(p => p.SecondaryButtonUrl, "/secondary")
                     .Add(p => p.SecondaryButtonClass, secondaryClass));

        // Assert
        Assert.Contains(primaryClass, component.Markup);
        Assert.Contains(secondaryClass, component.Markup);
    }

    [Fact]
    public void HeroSection_HasResponsiveLayout()
    {
        // Act
        var component = RenderComponent<HeroSection>();

        // Assert
        Assert.Contains("container", component.Markup);
        Assert.Contains("row", component.Markup);
        Assert.Contains("col-lg-8", component.Markup);
        Assert.Contains("justify-content-center", component.Markup);
    }

    [Fact]
    public void HeroSection_HasOverlayForBackgroundImages()
    {
        // Act
        var component = RenderComponent<HeroSection>();

        // Assert
        Assert.Contains("hero-overlay", component.Markup);
    }

    [Fact]
    public void HeroSection_SupportsChildContent()
    {
        // Arrange
        var childContent = "<div class=\"custom-content\">Custom Content</div>";

        // Act
        var component = RenderComponent<HeroSection>(parameters => 
            parameters.AddChildContent(childContent));

        // Assert
        Assert.Contains("custom-content", component.Markup);
        Assert.Contains("Custom Content", component.Markup);
        Assert.Contains("hero-custom-content", component.Markup);
    }
}
