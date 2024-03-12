namespace Fintracker.Application.Exceptions;

public class BadRequestException : BaseError
{
    public BadRequestException(List<ExceptionDetails> errors) : base(errors)
    {
    }

    public BadRequestException(ExceptionDetails details): base(details)
    {
        
    }
}