namespace Fintracker.Application.Models.Mail;

public class EmailModel
{
    public string Email { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string PlainMessage { get; set; } = default!;
    public string HtmlPath { get; set; } = default!;
    public string HtmlMessage { get; set; } = default!;
    public string? Name { get; set; }
}