namespace Fintracker.Application.Exceptions;

public class ForbiddenException : BaseError
{
    public ForbiddenException(ExceptionDetails details): base(details)
    {
        
    }
}