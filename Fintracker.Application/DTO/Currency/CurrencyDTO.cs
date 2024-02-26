using Fintracker.Application.DTO.Common;

namespace Fintracker.Application.DTO.Currency;

public class CurrencyDTO : IBaseDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Symbol { get; set; }
}