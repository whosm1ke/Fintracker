namespace Fintracker.Application.Responses;

public class DeleteCommandResponse<T> : BaseCommandResponse
{
    public T? Old { get; set; }
}