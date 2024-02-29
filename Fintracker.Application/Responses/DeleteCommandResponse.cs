namespace Fintracker.Application.Responses;

public class DeleteCommandResponse<T> : BaseCommandResponse
{
    public T? DeletedObj { get; set; }
}