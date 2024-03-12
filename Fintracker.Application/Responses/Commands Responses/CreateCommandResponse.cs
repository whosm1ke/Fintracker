namespace Fintracker.Application.Responses.Commands_Responses;

public class CreateCommandResponse<T> : BaseCommandResponse
{
    public T? CreatedObject { get; set; }
}