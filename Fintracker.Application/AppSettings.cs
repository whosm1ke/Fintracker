namespace Fintracker.Application;

public class AppSettings
{
    public string BaseUrl { get; set; } = default!;
    public string UiUrl { get; set; } = default!;
    public string[] AllowedExtensions { get; set; }
    
}