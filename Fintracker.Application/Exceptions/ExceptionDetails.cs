namespace Fintracker.Application.Exceptions;

public class ExceptionDetails
{
    public string ErrorMessage { get; set; } = default!;
    public string? PropertyName { get; set; }
}