namespace Fintracker.Application.Exceptions;

public class LoginException : RegisterAccountException
{
    public LoginException(List<string> errors) : base(errors)
    {
        
    }

    public LoginException(string error) : base(new List<string>() {error})
    {
    }
}