namespace Fintracker.Application.Responses.Commands_Responses;

public class DeleteCommandResponse<T> : BaseCommandResponse
{
    public T? DeletedObj { get; set; }
}