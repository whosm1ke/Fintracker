using Fintracker.Application.DTO.Common;

namespace Fintracker.Application.DTO.Transaction;

public class UpdateTransactionDTO : IBaseDto, ITransactionDto
{
    public Guid Id { get; set; }
    
    public Guid CategoryId { get; set; }
    
    public Guid CurrencyId { get; set; }

    public decimal Amount { get; set; }

    public string? Note { get; set; }
    
    public string? Label { get; set; }
    
    public DateTime Date { get; set; }
}