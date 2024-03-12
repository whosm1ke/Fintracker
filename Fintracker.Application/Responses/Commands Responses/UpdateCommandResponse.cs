namespace Fintracker.Application.Responses.Commands_Responses;

public class UpdateCommandResponse<T> : BaseCommandResponse
{
    public T Old { get; set; }
    public T? New { get; set; }
}