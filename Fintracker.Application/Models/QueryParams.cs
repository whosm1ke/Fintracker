namespace Fintracker.Application.Models;

public class QueryParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public string SortBy { get; set; } = "id";
    public bool IsDescending { get; set; } = false;
}

public class TransactionQueryParams : QueryParams
{
    public int? TransactionsPerDate { get; set; }

    public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-7);

    public DateTime EndDate { get; set; } = DateTime.Now;
}

public class BudgetQueryParams : QueryParams
{
    public bool? IsPublic { get; set; }
}