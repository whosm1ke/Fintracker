namespace Fintracker.Application.DTO.Transaction;

public class GroupedTransactionByDateDTO
{
    public DateTime Date { get; set; }

    public IList<TransactionBaseDTO> Transactions { get; set; }
}