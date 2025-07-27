using Microsoft.AspNetCore.Components;

namespace CarShowcase.Components;

public class PageAction
{
    public string Text { get; set; } = "";
    public string Icon { get; set; } = "";
    public string Url { get; set; } = "";
    public string CssClass { get; set; } = "btn-primary";
    public bool IsButton { get; set; } = false;
    public EventCallback OnClick { get; set; }
}
