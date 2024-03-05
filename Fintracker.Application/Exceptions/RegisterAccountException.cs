namespace Fintracker.Application.Exceptions;

public class RegisterAccountException : ApplicationException
{
    public List<string> Errors { get; set; } = new();

    public RegisterAccountException(List<string> errors)
    {
        errors.ForEach(x => Errors.Add(x));
    }
}