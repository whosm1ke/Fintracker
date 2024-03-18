using Fintracker.Application.DTO.Common;

namespace Fintracker.Application.DTO.Currency;

public class CurrencyDTO : IBaseDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;
    
    public string Symbol { get; set; } = default!;
    
    public int Code { get; set; }
}