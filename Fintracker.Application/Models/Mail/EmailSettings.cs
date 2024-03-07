namespace Fintracker.Application.Models.Mail;

public class EmailSettings
{
    public string ApiKey { get; set; }
    public string FromName { get; set; }
    public string FromPassword { get; set; }
    public string FromAddress { get; set; }
}