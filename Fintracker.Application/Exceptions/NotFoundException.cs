namespace Fintracker.Application.Exceptions;

public class NotFoundException : ApplicationException
{
    public NotFoundException(string name, object key) : base($"{name} was not found [{key}]")
    {
        
    }
}