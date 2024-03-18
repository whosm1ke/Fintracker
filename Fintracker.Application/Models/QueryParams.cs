namespace Fintracker.Application.Models;

public class QueryParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortBy { get; set; } = default!;
    public bool IsDescending { get; set; }
}