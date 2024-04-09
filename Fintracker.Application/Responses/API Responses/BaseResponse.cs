using Fintracker.Application.Exceptions;

namespace Fintracker.Application.Responses.API_Responses;

public class BaseResponse
{
    public Guid TraceId { get; set; }

    public DateTime When { get; set; }

    public string Reason { get; set; } = default!;

    public string Message { get; set; } = default!;

    private List<ExceptionDetails> _details;
    public List<ExceptionDetails> Details
    {
        get
        {
            _details.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.PropertyName))
                {
                    x.PropertyName = char.ToLowerInvariant(x.PropertyName[0]) + x.PropertyName.Substring(1);
                }
            });
            return _details;
        }
        set => _details = value;
    }

    public int StatusCode { get; set; }
}