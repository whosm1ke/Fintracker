namespace Fintracker.Application.Exceptions;

public class NotFoundException : ApplicationException
{
    public string Name { get; }
    public object Key { get; }
    public NotFoundException(string name, object key) : base($"{name} was not found [{key}]")
    {
        Name = name;
        Key = key;
    }
}