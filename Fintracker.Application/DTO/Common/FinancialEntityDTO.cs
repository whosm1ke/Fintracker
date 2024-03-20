using Fintracker.Application.DTO.Currency;

namespace Fintracker.Application.DTO.Common;

public class FinancialEntityDTO : IBaseDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;

    public decimal Balance { get; set; }
    
    public decimal TotalSpent { get; set; }
    
    public Guid CurrencyId { get; set; }
    
    public CurrencyDTO Currency { get; set; } = default!;
}