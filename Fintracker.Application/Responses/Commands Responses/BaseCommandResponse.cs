namespace Fintracker.Application.Responses.Commands_Responses;

public class BaseCommandResponse
{
    public Guid Id { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = default!;
    public List<string> Errors { get; set; } = default!;
    
}