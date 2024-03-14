namespace Fintracker.Application.Models.Mail;

public class EmailSettings
{
    public string ApiKey { get; set; } = default!;
    public string FromName { get; set; } = default!;
    public string FromPassword { get; set; } = default!;
    public string FromAddress { get; set; } = default!;
}