namespace Fintracker.Application.Responses;

public class CreateCommandResponse<T> : BaseCommandResponse
{
    public T? CreatedObject { get; set; }
}