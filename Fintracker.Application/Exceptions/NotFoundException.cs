namespace Fintracker.Application.Exceptions;

public class NotFoundException : BaseError
{

    public NotFoundException(List<ExceptionDetails> errors, string type) : base(errors)
    {
        Type = type;
    }

    public NotFoundException(ExceptionDetails details, string type): base(details)
    {
        Type = type;
    }

    public string Type { get; set; } = default!;
}