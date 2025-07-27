using Bunit;
using Microsoft.AspNetCore.Components.Web;
using CarShowcase.Components;
using System.Threading.Tasks;
using System;
// Using alias to avoid ambiguous reference with System.IO.ErrorEventArgs
using WebErrorEventArgs = Microsoft.AspNetCore.Components.Web.ErrorEventArgs;

namespace CarShowcase.Tests.Components;

public class CarImageTests : TestContext
{
    [Fact]
    public void CarImage_RendersCorrectly()
    {
        // Arrange
        var make = "Toyota";
        var model = "Camry";
        var alt = "Toyota Camry";
        var cssClass = "test-class";
        var style = "width: 100%";

        // Act
        var component = RenderComponent<CarImage>(parameters => parameters
            .Add(p => p.Make, make)
            .Add(p => p.Model, model)
            .Add(p => p.Alt, alt)
            .Add(p => p.CssClass, cssClass)
            .Add(p => p.Style, style));

        // Assert
        var img = component.Find("img");
        Assert.Equal(alt, img.GetAttribute("alt"));
        Assert.Equal(cssClass, img.GetAttribute("class"));
        Assert.Equal(style, img.GetAttribute("style"));
        Assert.Contains("https://picsum.photos", img.GetAttribute("src")); // Initial strategy
    }

    [Fact]
    public void CarImage_UsesConsistentSeed()
    {
        // Arrange
        var make1 = "Toyota";
        var model1 = "Camry";
        var make2 = "Toyota";
        var model2 = "Camry";

        // Act
        var component1 = RenderComponent<CarImage>(parameters => parameters
            .Add(p => p.Make, make1)
            .Add(p => p.Model, model1));

        var component2 = RenderComponent<CarImage>(parameters => parameters
            .Add(p => p.Make, make2)
            .Add(p => p.Model, model2));

        // Assert
        var img1 = component1.Find("img").GetAttribute("src");
        var img2 = component2.Find("img").GetAttribute("src");
        Assert.Equal(img1, img2); // Same make/model should result in same image URL
    }

    [Fact]
    public void CarImage_DifferentCarsGetDifferentImages()
    {
        // Arrange
        var make1 = "Toyota";
        var model1 = "Camry";
        var make2 = "Honda";
        var model2 = "Civic";

        // Act
        var component1 = RenderComponent<CarImage>(parameters => parameters
            .Add(p => p.Make, make1)
            .Add(p => p.Model, model1));

        var component2 = RenderComponent<CarImage>(parameters => parameters
            .Add(p => p.Make, make2)
            .Add(p => p.Model, model2));

        // Assert
        var img1 = component1.Find("img").GetAttribute("src");
        var img2 = component2.Find("img").GetAttribute("src");
        Assert.NotEqual(img1, img2); // Different make/model should result in different image URL
    }

    [Fact]
    public async Task CarImage_FallsBackToSecondStrategy_OnError()
    {
        // Arrange
        var make = "Toyota";
        var model = "Camry";
        
        // Act
        var component = RenderComponent<CarImage>(parameters => parameters
            .Add(p => p.Make, make)
            .Add(p => p.Model, model));
        
        var img = component.Find("img");
        var initialSrc = img.GetAttribute("src");
        Assert.Contains("https://picsum.photos", initialSrc); // Initial strategy
        
        // Trigger error event to simulate image loading failure
        await component.InvokeAsync(() => img.TriggerEvent("onerror", new WebErrorEventArgs()));
        
        // Assert
        var newSrc = component.Find("img").GetAttribute("src");
        Assert.Contains("https://dummyimage.com", newSrc); // Second strategy
        Assert.NotEqual(initialSrc, newSrc);
    }

    [Fact]
    public async Task CarImage_FallsBackToThirdStrategy_OnSecondError()
    {
        // Arrange
        var make = "Toyota";
        var model = "Camry";
        
        // Act
        var component = RenderComponent<CarImage>(parameters => parameters
            .Add(p => p.Make, make)
            .Add(p => p.Model, model));
        
        var img = component.Find("img");
        
        // First failure
        await component.InvokeAsync(() => img.TriggerEvent("onerror", new WebErrorEventArgs()));
        var secondSrc = component.Find("img").GetAttribute("src");
        Assert.Contains("https://dummyimage.com", secondSrc);
        
        // Second failure
        await component.InvokeAsync(() => img.TriggerEvent("onerror", new WebErrorEventArgs()));
        
        // Assert
        var thirdSrc = component.Find("img").GetAttribute("src");
        Assert.Contains("https://jsonplaceholder.typicode.com", thirdSrc); // Third strategy
        Assert.NotEqual(secondSrc, thirdSrc);
    }

    [Fact]
    public async Task CarImage_FallsBackToLocalSvg_OnAllErrors()
    {
        // Arrange
        var make = "Toyota";
        var model = "Camry";
        
        // Act
        var component = RenderComponent<CarImage>(parameters => parameters
            .Add(p => p.Make, make)
            .Add(p => p.Model, model));
        
        var img = component.Find("img");
        
        // Trigger three error events to reach the SVG fallback
        await component.InvokeAsync(() => img.TriggerEvent("onerror", new WebErrorEventArgs()));
        await component.InvokeAsync(() => img.TriggerEvent("onerror", new WebErrorEventArgs()));
        await component.InvokeAsync(() => img.TriggerEvent("onerror", new WebErrorEventArgs()));
        
        // Assert
        var finalSrc = component.Find("img").GetAttribute("src");
        Assert.Contains("data:image/svg+xml;base64,", finalSrc); // SVG fallback
    }

    [Fact]
    public void CarImage_GeneratesValidSvgWithCarDetails()
    {
        // Arrange
        var make = "Toyota";
        var model = "Camry";
        
        // Act - Force use of SVG by manually triggering all errors
        var component = RenderComponent<CarImage>(parameters => parameters
            .Add(p => p.Make, make)
            .Add(p => p.Model, model));
        
        var img = component.Find("img");
        
        // Trigger three error events to reach the SVG fallback
        img.TriggerEvent("onerror", new WebErrorEventArgs());
        img.TriggerEvent("onerror", new WebErrorEventArgs());
        img.TriggerEvent("onerror", new WebErrorEventArgs());
        
        // Assert
        var svgSrc = component.Find("img").GetAttribute("src");
        Assert.Contains("data:image/svg+xml;base64,", svgSrc);
        
        // Decode the base64 SVG and verify it contains car details
        var base64Data = svgSrc.Replace("data:image/svg+xml;base64,", "");
        var svgXml = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64Data));
        
        Assert.Contains(make, svgXml);
        Assert.Contains(model, svgXml);
        Assert.Contains("<svg", svgXml);
        Assert.Contains("</svg>", svgXml);
    }

    [Fact]
    public void CarImage_HandlesMissingParameters()
    {
        // Arrange & Act
        var component = RenderComponent<CarImage>();
        
        // Assert
        var img = component.Find("img");
        Assert.NotNull(img);
        Assert.Contains("https://picsum.photos", img.GetAttribute("src"));
        Assert.Equal(string.Empty, img.GetAttribute("alt"));
        Assert.Equal(string.Empty, img.GetAttribute("class"));
    }
}
