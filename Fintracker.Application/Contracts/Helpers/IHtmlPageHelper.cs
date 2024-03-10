namespace Fintracker.Application.Contracts.Helpers;

public interface IHtmlPageHelper
{
    public Task<string> GetPageContent(string page);
}