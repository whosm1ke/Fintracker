namespace Fintracker.Application.Models;

public class QueryParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public string SortBy { get; set; } = "Id";
    public bool IsDescending { get; set; } = false;
}

public class TransactionQueryParams : QueryParams
{
    public int? TransactionsPerDate { get; set; } = 10;
}