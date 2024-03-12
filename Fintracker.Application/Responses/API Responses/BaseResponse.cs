using Fintracker.Application.Exceptions;

namespace Fintracker.Application.Responses.API_Responses;

public class BaseResponse
{
    
    public Guid TraceId { get; set; }

    public DateTime When { get; set; }

    public string Reason { get; set; }

    public string Message { get; set; }

    public List<ExceptionDetails> Details { get; set; }

    public int StatusCode { get; set; }
}