using Fintracker.Application.Exceptions;

namespace Fintracker.Application.Responses.API_Responses;

public class BaseResponse
{
    
    public Guid TraceId { get; set; }

    public DateTime When { get; set; }

    public string Reason { get; set; } = default!;

    public string Message { get; set; } = default!;

    public List<ExceptionDetails> Details { get; set; } = default!;

    public int StatusCode { get; set; }
}