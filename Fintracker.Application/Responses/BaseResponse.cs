namespace Fintracker.Application.Responses;

public class BaseResponse
{
    
    public Guid Id { get; set; }

    public DateTime When { get; set; }

    public string Reason { get; set; }

    public string Details { get; set; }

    public int StatusCode { get; set; }
}