using Bunit;
using CarShowcase.Shared;
using AngleSharp.Dom;

namespace CarShowcase.Tests.Components;

public class SurveyPromptTests : TestContext
{
    [Fact]
    public void SurveyPrompt_WithTitle_RendersCorrectly()
    {
        // Arrange
        var title = "How is Blazor working for you?";

        // Act
        var component = RenderComponent<SurveyPrompt>(parameters => 
            parameters.Add(p => p.Title, title));

        // Assert
        Assert.Contains(title, component.Markup);
        Assert.Contains("alert alert-secondary", component.Markup);
        Assert.Contains("oi-pencil", component.Markup);
    }

    [Fact]
    public void SurveyPrompt_WithoutTitle_RendersWithoutTitle()
    {
        // Act
        var component = RenderComponent<SurveyPrompt>();

        // Assert
        Assert.Contains("alert alert-secondary", component.Markup);
        Assert.Contains("Please take our", component.Markup);
        Assert.Contains("brief survey", component.Markup);
        Assert.Contains("tell us what you think", component.Markup);
    }

    [Fact]
    public void SurveyPrompt_ContainsSurveyLink()
    {
        // Act
        var component = RenderComponent<SurveyPrompt>();

        // Assert
        var surveyLink = component.Find("a[target='_blank']");
        Assert.NotNull(surveyLink);
        Assert.Contains("brief survey", surveyLink.TextContent);
        Assert.Contains("font-weight-bold", surveyLink.ClassList);
        Assert.Contains("link-dark", surveyLink.ClassList);
        
        var href = surveyLink.GetAttribute("href");
        Assert.Contains("microsoft.com", href);
    }

    [Fact]
    public void SurveyPrompt_HasCorrectIcon()
    {
        // Act
        var component = RenderComponent<SurveyPrompt>();

        // Assert
        var icon = component.Find(".oi-pencil");
        Assert.NotNull(icon);
        Assert.Contains("me-2", icon.ClassList);
        Assert.Equal("true", icon.GetAttribute("aria-hidden"));
    }

    [Fact]
    public void SurveyPrompt_HasCorrectStructure()
    {
        // Arrange
        var title = "Test Title";

        // Act
        var component = RenderComponent<SurveyPrompt>(parameters => 
            parameters.Add(p => p.Title, title));

        // Assert
        var alertDiv = component.Find(".alert");
        Assert.NotNull(alertDiv);
        Assert.Contains("alert-secondary", alertDiv.ClassList);
        Assert.Contains("mt-4", alertDiv.ClassList);

        var strongElement = alertDiv.QuerySelector("strong");
        Assert.NotNull(strongElement);
        Assert.Equal(title, strongElement.TextContent);

        var textSpan = alertDiv.QuerySelector(".text-nowrap");
        Assert.NotNull(textSpan);
    }

    [Fact]
    public void SurveyPrompt_WithCustomTitle_DisplaysCustomTitle()
    {
        // Arrange
        var customTitle = "Custom Survey Title";

        // Act
        var component = RenderComponent<SurveyPrompt>(parameters => 
            parameters.Add(p => p.Title, customTitle));

        // Assert
        Assert.Contains(customTitle, component.Markup);
        
        var strongElement = component.Find("strong");
        Assert.Equal(customTitle, strongElement.TextContent);
    }

    [Fact]
    public void SurveyPrompt_WithNullTitle_DoesNotRenderStrongElement()
    {
        // Act
        var component = RenderComponent<SurveyPrompt>(parameters =>
            parameters.Add(p => p.Title, (string?)null));

        // Assert
        var strongElements = component.FindAll("strong");
        // Should still render but with empty content
        Assert.NotEmpty(strongElements);
        Assert.Equal(string.Empty, strongElements.First().TextContent);
    }

    [Fact]
    public void SurveyPrompt_WithEmptyTitle_RendersEmptyStrong()
    {
        // Act
        var component = RenderComponent<SurveyPrompt>(parameters => 
            parameters.Add(p => p.Title, string.Empty));

        // Assert
        var strongElement = component.Find("strong");
        Assert.Equal(string.Empty, strongElement.TextContent);
    }

    [Fact]
    public void SurveyPrompt_LinkOpensInNewTab()
    {
        // Act
        var component = RenderComponent<SurveyPrompt>();

        // Assert
        var surveyLink = component.Find("a");
        Assert.Equal("_blank", surveyLink.GetAttribute("target"));
    }
}
