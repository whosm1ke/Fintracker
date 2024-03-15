namespace Fintracker.Application.Exceptions;

public class ForbidenException : BaseError
{
    public ForbidenException(ExceptionDetails details): base(details)
    {
        
    }
}