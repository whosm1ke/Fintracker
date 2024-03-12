namespace Fintracker.Application.Exceptions;

public abstract class BaseError : ApplicationException
{
    public List<ExceptionDetails> Errors { get; }

    public BaseError(List<ExceptionDetails> errors) : base(
        ConvertAllErrorMessagesToString(errors.Select(x => x.ErrorMessage).ToList()))
    {
        Errors = errors;
    }
    
    public BaseError(ExceptionDetails details) : this(new List<ExceptionDetails>{details})
    {
        
    }

    private static string ConvertAllErrorMessagesToString(List<string> errorMsgs)
    {
        if (errorMsgs.Count == 0)
            return "No error messages was provided";
        return string.Join(", ", errorMsgs);
    }
}