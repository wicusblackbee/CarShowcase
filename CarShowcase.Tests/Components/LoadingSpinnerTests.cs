using Bunit;
using CarShowcase.Components;
using AngleSharp.Dom;

namespace CarShowcase.Tests.Components;

public class LoadingSpinnerTests : TestContext
{
    [Fact]
    public void LoadingSpinner_RendersCorrectly()
    {
        // Act
        var component = RenderComponent<LoadingSpinner>();

        // Assert
        Assert.Contains("spinner-border", component.Markup);
        Assert.Contains("Loading...", component.Markup);
    }

    [Fact]
    public void LoadingSpinner_ShowsCustomMessage()
    {
        // Arrange
        var customMessage = "Please wait while we load your data...";

        // Act
        var component = RenderComponent<LoadingSpinner>(parameters => 
            parameters.Add(p => p.Message, customMessage));

        // Assert
        Assert.Contains(customMessage, component.Markup);
    }

    [Fact]
    public void LoadingSpinner_ShowsCustomLoadingText()
    {
        // Arrange
        var customLoadingText = "Processing...";

        // Act
        var component = RenderComponent<LoadingSpinner>(parameters => 
            parameters.Add(p => p.LoadingText, customLoadingText));

        // Assert
        var spinner = component.Find(".spinner-border");
        var hiddenText = spinner.QuerySelector(".visually-hidden");
        Assert.NotNull(hiddenText);
        Assert.Equal(customLoadingText, hiddenText.TextContent);
    }

    [Fact]
    public void LoadingSpinner_ShowsCardWhenEnabled()
    {
        // Act
        var component = RenderComponent<LoadingSpinner>(parameters => 
            parameters.Add(p => p.ShowCard, true));

        // Assert
        Assert.Contains("card", component.Markup);
        Assert.Contains("card-body", component.Markup);
    }

    [Fact]
    public void LoadingSpinner_HidesCardWhenDisabled()
    {
        // Act
        var component = RenderComponent<LoadingSpinner>(parameters => 
            parameters.Add(p => p.ShowCard, false));

        // Assert
        Assert.DoesNotContain("card", component.Markup);
        Assert.DoesNotContain("card-body", component.Markup);
    }

    [Fact]
    public void LoadingSpinner_AppliesCustomCssClass()
    {
        // Arrange
        var customClass = "my-custom-spinner";

        // Act
        var component = RenderComponent<LoadingSpinner>(parameters => 
            parameters.Add(p => p.CssClass, customClass));

        // Assert
        Assert.Contains(customClass, component.Markup);
    }

    [Fact]
    public void LoadingSpinner_AppliesCustomContainerClass()
    {
        // Arrange
        var customContainerClass = "my-container";

        // Act
        var component = RenderComponent<LoadingSpinner>(parameters => 
            parameters.Add(p => p.ContainerClass, customContainerClass)
                     .Add(p => p.ShowCard, false));

        // Assert
        Assert.Contains(customContainerClass, component.Markup);
    }

    [Fact]
    public void LoadingSpinner_AppliesCustomSpinnerClass()
    {
        // Arrange
        var customSpinnerClass = "text-success";

        // Act
        var component = RenderComponent<LoadingSpinner>(parameters => 
            parameters.Add(p => p.SpinnerClass, customSpinnerClass));

        // Assert
        Assert.Contains(customSpinnerClass, component.Markup);
    }

    [Fact]
    public void LoadingSpinner_AppliesSmallSize()
    {
        // Act
        var component = RenderComponent<LoadingSpinner>(parameters => 
            parameters.Add(p => p.Size, SpinnerSize.Small));

        // Assert
        Assert.Contains("spinner-sm", component.Markup);
    }

    [Fact]
    public void LoadingSpinner_AppliesLargeSize()
    {
        // Act
        var component = RenderComponent<LoadingSpinner>(parameters => 
            parameters.Add(p => p.Size, SpinnerSize.Large));

        // Assert
        Assert.Contains("spinner-lg", component.Markup);
    }

    [Fact]
    public void LoadingSpinner_HasCorrectRole()
    {
        // Act
        var component = RenderComponent<LoadingSpinner>();

        // Assert
        var spinner = component.Find(".spinner-border");
        Assert.Equal("status", spinner.GetAttribute("role"));
    }

    [Fact]
    public void LoadingSpinner_HasAccessibleText()
    {
        // Act
        var component = RenderComponent<LoadingSpinner>();

        // Assert
        var hiddenText = component.Find(".visually-hidden");
        Assert.NotNull(hiddenText);
        Assert.Equal("Loading...", hiddenText.TextContent);
    }

    [Fact]
    public void LoadingSpinner_CentersContent()
    {
        // Act
        var component = RenderComponent<LoadingSpinner>();

        // Assert
        Assert.Contains("text-center", component.Markup);
    }

    [Fact]
    public void LoadingSpinner_HasSpinnerContainer()
    {
        // Act
        var component = RenderComponent<LoadingSpinner>();

        // Assert
        Assert.Contains("spinner-container", component.Markup);
    }
}
