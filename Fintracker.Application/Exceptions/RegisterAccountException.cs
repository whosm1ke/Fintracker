namespace Fintracker.Application.Exceptions;

public class RegisterAccountException : BaseError
{
    public RegisterAccountException(List<ExceptionDetails> errors) : base(errors)
    {
    }
    public RegisterAccountException(ExceptionDetails details): base(details)
    {
        
    }
}