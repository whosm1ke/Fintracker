using Fintracker.Domain.Entities;

namespace Fintracker.Application.Models;

public class GroupedTransactionByDate
{
    public DateTime Date { get; set; }

    public IList<Transaction> Transactions { get; set; }
}