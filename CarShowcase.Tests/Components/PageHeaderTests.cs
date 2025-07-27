using Bunit;
using CarShowcase.Components;
using AngleSharp.Dom;

namespace CarShowcase.Tests.Components;

public class PageHeaderTests : TestContext
{
    [Fact]
    public void PageHeader_RendersCorrectly()
    {
        // Act
        var component = RenderComponent<PageHeader>();

        // Assert
        Assert.Contains("page-header", component.Markup);
    }

    [Fact]
    public void PageHeader_ShowsTitle()
    {
        // Arrange
        var title = "Test Page Title";

        // Act
        var component = RenderComponent<PageHeader>(parameters => 
            parameters.Add(p => p.Title, title));

        // Assert
        Assert.Contains(title, component.Markup);
        Assert.Contains("page-title", component.Markup);
    }

    [Fact]
    public void PageHeader_ShowsSubtitle()
    {
        // Arrange
        var subtitle = "Test page subtitle";

        // Act
        var component = RenderComponent<PageHeader>(parameters => 
            parameters.Add(p => p.Subtitle, subtitle));

        // Assert
        Assert.Contains(subtitle, component.Markup);
        Assert.Contains("page-subtitle", component.Markup);
    }

    [Fact]
    public void PageHeader_ShowsIcon()
    {
        // Arrange
        var icon = "home";
        var title = "Home Page";

        // Act
        var component = RenderComponent<PageHeader>(parameters => 
            parameters.Add(p => p.Title, title)
                     .Add(p => p.Icon, icon));

        // Assert
        Assert.Contains($"oi-{icon}", component.Markup);
    }

    [Fact]
    public void PageHeader_ShowsBreadcrumbs()
    {
        // Arrange
        var breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Text = "Home", Url = "/" },
            new BreadcrumbItem { Text = "Cars", Url = "/cars", IsActive = true }
        };

        // Act
        var component = RenderComponent<PageHeader>(parameters => 
            parameters.Add(p => p.Breadcrumbs, breadcrumbs)
                     .Add(p => p.ShowBreadcrumb, true));

        // Assert
        Assert.Contains("breadcrumb", component.Markup);
        Assert.Contains("Home", component.Markup);
        Assert.Contains("Cars", component.Markup);
    }

    [Fact]
    public void PageHeader_HidesBreadcrumbsWhenDisabled()
    {
        // Arrange
        var breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Text = "Home", Url = "/" }
        };

        // Act
        var component = RenderComponent<PageHeader>(parameters => 
            parameters.Add(p => p.Breadcrumbs, breadcrumbs)
                     .Add(p => p.ShowBreadcrumb, false));

        // Assert
        Assert.DoesNotContain("breadcrumb", component.Markup);
    }

    [Fact]
    public void PageHeader_ShowsBadge()
    {
        // Arrange
        var title = "Test Title";
        var badgeText = "New";

        // Act
        var component = RenderComponent<PageHeader>(parameters => 
            parameters.Add(p => p.Title, title)
                     .Add(p => p.ShowBadge, true)
                     .Add(p => p.BadgeText, badgeText));

        // Assert
        Assert.Contains("badge", component.Markup);
        Assert.Contains(badgeText, component.Markup);
    }

    [Fact]
    public void PageHeader_ShowsActions()
    {
        // Arrange
        var actions = new List<PageAction>
        {
            new PageAction { Text = "Add Item", Icon = "plus", CssClass = "btn-primary", IsButton = true },
            new PageAction { Text = "Settings", Url = "/settings", CssClass = "btn-secondary" }
        };

        // Act
        var component = RenderComponent<PageHeader>(parameters => 
            parameters.Add(p => p.Actions, actions)
                     .Add(p => p.ShowActions, true));

        // Assert
        Assert.Contains("Add Item", component.Markup);
        Assert.Contains("Settings", component.Markup);
        Assert.Contains("page-actions", component.Markup);
    }

    [Fact]
    public void PageHeader_HidesActionsWhenDisabled()
    {
        // Arrange
        var actions = new List<PageAction>
        {
            new PageAction { Text = "Add Item", CssClass = "btn-primary", IsButton = true }
        };

        // Act
        var component = RenderComponent<PageHeader>(parameters => 
            parameters.Add(p => p.Actions, actions)
                     .Add(p => p.ShowActions, false));

        // Assert
        Assert.DoesNotContain("page-actions", component.Markup);
    }

    [Fact]
    public void PageHeader_ShowsDivider()
    {
        // Act
        var component = RenderComponent<PageHeader>(parameters => 
            parameters.Add(p => p.ShowDivider, true));

        // Assert
        Assert.Contains("page-divider", component.Markup);
    }

    [Fact]
    public void PageHeader_HidesDividerWhenDisabled()
    {
        // Act
        var component = RenderComponent<PageHeader>(parameters => 
            parameters.Add(p => p.ShowDivider, false));

        // Assert
        Assert.DoesNotContain("page-divider", component.Markup);
    }

    [Fact]
    public void PageHeader_AppliesCustomCssClass()
    {
        // Arrange
        var customClass = "my-page-header";

        // Act
        var component = RenderComponent<PageHeader>(parameters => 
            parameters.Add(p => p.CssClass, customClass));

        // Assert
        Assert.Contains(customClass, component.Markup);
    }

    [Fact]
    public void PageHeader_AppliesCustomTitleClass()
    {
        // Arrange
        var title = "Test Title";
        var titleClass = "custom-title";

        // Act
        var component = RenderComponent<PageHeader>(parameters => 
            parameters.Add(p => p.Title, title)
                     .Add(p => p.TitleClass, titleClass));

        // Assert
        Assert.Contains(titleClass, component.Markup);
    }

    [Fact]
    public void PageHeader_AppliesCustomSubtitleClass()
    {
        // Arrange
        var subtitle = "Test Subtitle";
        var subtitleClass = "custom-subtitle";

        // Act
        var component = RenderComponent<PageHeader>(parameters => 
            parameters.Add(p => p.Subtitle, subtitle)
                     .Add(p => p.SubtitleClass, subtitleClass));

        // Assert
        Assert.Contains(subtitleClass, component.Markup);
    }

    [Fact]
    public void PageHeader_SupportsChildContent()
    {
        // Arrange
        var childContent = "<div class=\"custom-content\">Custom Content</div>";

        // Act
        var component = RenderComponent<PageHeader>(parameters => 
            parameters.AddChildContent(childContent));

        // Assert
        Assert.Contains("custom-content", component.Markup);
        Assert.Contains("Custom Content", component.Markup);
    }

    [Fact]
    public void PageHeader_HasResponsiveLayout()
    {
        // Act
        var component = RenderComponent<PageHeader>();

        // Assert
        Assert.Contains("row", component.Markup);
        Assert.Contains("col-md-8", component.Markup);
        Assert.Contains("col-md-4", component.Markup);
    }

    [Fact]
    public void PageHeader_ShowsActiveBreadcrumbCorrectly()
    {
        // Arrange
        var breadcrumbs = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Text = "Home", Url = "/" },
            new BreadcrumbItem { Text = "Current", IsActive = true }
        };

        // Act
        var component = RenderComponent<PageHeader>(parameters => 
            parameters.Add(p => p.Breadcrumbs, breadcrumbs));

        // Assert
        Assert.Contains("breadcrumb-item active", component.Markup);
        Assert.Contains("aria-current=\"page\"", component.Markup);
    }
}
