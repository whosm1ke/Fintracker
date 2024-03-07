namespace Fintracker.Application.Models.Mail;

public class EmailModel
{
    public string Email { get; set; }
    public string Subject { get; set; }
    public string PlainMessage { get; set; }
    
    public string HtmlMessage { get; set; }
    public string? Name { get; set; }
}