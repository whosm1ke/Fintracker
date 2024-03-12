namespace Fintracker.Application.Exceptions;

public class LoginException : BaseError
{
    public LoginException(List<ExceptionDetails> errors) : base(errors)
    {
    }
    
    public LoginException(ExceptionDetails details): base(details)
    {
        
    }
}